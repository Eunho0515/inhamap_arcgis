from __future__ import annotations

import argparse
import json
import mmap
import os
import queue
import socket
import struct
import subprocess
import sys
import threading
import time
from pathlib import Path

_config_dir = Path(__file__).resolve().parent / ".config" / "ultralytics"
_config_dir.mkdir(parents=True, exist_ok=True)
os.environ.setdefault("YOLO_CONFIG_DIR", str(_config_dir))
os.environ.setdefault("MPLCONFIGDIR", str(_config_dir / "matplotlib"))

import cv2
import numpy as np
from ultralytics import YOLO


CANONICAL_CLASSES = {"fire", "smoke"}
SHARED_MEMORY_HEADER_SIZE = 64
SHARED_MEMORY_MAGIC = b"PGVF"
DETECTION_MEMORY_NAME = "PehGcsYoloDetections"
DETECTION_MEMORY_SIZE = 4096
DETECTION_MAGIC = b"PGDT"
MAX_SHARED_DETECTIONS = 32


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Receive RTP/H.264, run fire/smoke YOLO, and display boxes."
    )
    parser.add_argument("--port", type=int, default=5600)
    parser.add_argument("--payload-type", type=int, default=96)
    parser.add_argument("--latency-ms", type=int, default=40)
    parser.add_argument("--frame-port", type=int, default=15600)
    parser.add_argument("--metadata-port", type=int, default=5601)
    parser.add_argument("--fps", type=int, default=10)
    parser.add_argument("--width", type=int, default=720)
    parser.add_argument("--height", type=int, default=1280)
    parser.add_argument(
        "--rotation",
        choices=(
            "none",
            "clockwise",
            "counterclockwise",
            "rotate-180",
            "horizontal-flip",
            "vertical-flip",
            "upper-left-diagonal",
            "upper-right-diagonal",
        ),
        default="upper-right-diagonal",
    )
    parser.add_argument(
        "--model",
        type=Path,
        default=Path(__file__).resolve().parent / "models" / "hazard" / "best.onnx",
    )
    parser.add_argument("--confidence", type=float, default=0.25)
    parser.add_argument("--imgsz", type=int, default=640)
    parser.add_argument("--device", default="cpu")
    parser.add_argument("--shared-memory", default="PehGcsYoloFrame")
    parser.add_argument("--no-window", action="store_true")
    return parser.parse_args()


def gst_executable(name: str) -> str:
    candidates = [
        Path(r"C:\gstreamer\bin") / name,
        Path(r"C:\gstreamer\1.0\msvc_x86_64\bin") / name,
    ]
    for candidate in candidates:
        if candidate.is_file():
            return str(candidate)
    return name


def receiver_command(args: argparse.Namespace) -> list[str]:
    caps = (
        "application/x-rtp,media=video,clock-rate=90000,"
        f"encoding-name=H264,payload={args.payload_type}"
    )
    raw_caps = (
        f"video/x-raw,format=BGR,width={args.width},height={args.height},"
        f"framerate={args.fps}/1"
    )
    return [
        gst_executable("gst-launch-1.0.exe"),
        "-q",
        "udpsrc",
        f"port={args.port}",
        f"caps={caps}",
        "!",
        "rtpjitterbuffer",
        f"latency={args.latency_ms}",
        "drop-on-latency=true",
        "!",
        "rtph264depay",
        "!",
        "h264parse",
        "!",
        "avdec_h264",
        "!",
        "videoconvert",
        "!",
        "videoflip",
        f"method={args.rotation}",
        "!",
        "videoscale",
        "!",
        "videorate",
        "!",
        raw_caps,
        "!",
        "queue",
        "max-size-buffers=1",
        "max-size-bytes=0",
        "max-size-time=0",
        "leaky=downstream",
        "!",
        "tcpserversink",
        "host=127.0.0.1",
        f"port={args.frame_port}",
        "sync=false",
    ]


def display_command(args: argparse.Namespace) -> list[str]:
    frame_size = args.width * args.height * 3
    return [
        gst_executable("gst-launch-1.0.exe"),
        "-q",
        "fdsrc",
        "fd=0",
        f"blocksize={frame_size}",
        "!",
        "rawvideoparse",
        "format=bgr",
        f"width={args.width}",
        f"height={args.height}",
        f"framerate={args.fps}/1",
        "!",
        "videoconvert",
        "!",
        "autovideosink",
        "sync=false",
    ]


def connect_frame_socket(port: int, process: subprocess.Popen) -> socket.socket:
    deadline = time.monotonic() + 10.0
    while time.monotonic() < deadline:
        if process.poll() is not None:
            raise RuntimeError(f"GStreamer receiver exited with code {process.returncode}")
        try:
            connection = socket.create_connection(("127.0.0.1", port), timeout=1.0)
            connection.settimeout(None)
            return connection
        except OSError:
            time.sleep(0.1)
    raise TimeoutError(f"Could not connect to the local frame bridge on port {port}")


def read_exact(stream: socket.socket, size: int) -> bytes | None:
    data = bytearray(size)
    view = memoryview(data)
    offset = 0
    while offset < size:
        count = stream.recv_into(view[offset:])
        if not count:
            return None
        offset += count
    return bytes(data)


def publish_shared_frame(
    shared: mmap.mmap,
    frame_bgra: np.ndarray,
    sequence: int,
    timestamp_ns: int,
    has_source_timestamp: bool,
) -> None:
    height, width, channels = frame_bgra.shape
    if channels != 4:
        raise ValueError("Shared frame must be BGRA")
    payload = frame_bgra.tobytes()
    writing_sequence = sequence * 2 + 1
    completed_sequence = writing_sequence + 1
    shared.seek(20)
    shared.write(struct.pack("<q", writing_sequence))
    shared.seek(0)
    shared.write(
        struct.pack(
            "<4siiiiqq",
            SHARED_MEMORY_MAGIC,
            width,
            height,
            width * channels,
            len(payload),
            writing_sequence,
            timestamp_ns,
        )
    )
    shared.seek(SHARED_MEMORY_HEADER_SIZE)
    shared.write(payload)
    shared.seek(36)
    shared.write(struct.pack("<i", 1 if has_source_timestamp else 0))
    shared.seek(20)
    shared.write(struct.pack("<q", completed_sequence))


def publish_shared_detections(
    shared: mmap.mmap,
    boxes: list[tuple[int, int, int, int, str, float]],
    sequence: int,
    timestamp_ns: int,
) -> None:
    selected = boxes[:MAX_SHARED_DETECTIONS]
    writing_sequence = sequence * 2 + 1
    completed_sequence = writing_sequence + 1
    shared.seek(4)
    shared.write(struct.pack("<q", writing_sequence))
    shared.seek(0)
    shared.write(
        struct.pack(
            "<4sqqi",
            DETECTION_MAGIC,
            writing_sequence,
            timestamp_ns,
            len(selected),
        )
    )
    for x1, y1, x2, y2, class_name, confidence in selected:
        class_id = 1 if class_name.lower() == "fire" else 0
        shared.write(
            struct.pack(
                "<fffffi",
                float(x1),
                float(y1),
                float(x2),
                float(y2),
                confidence,
                class_id,
            )
        )
    shared.seek(4)
    shared.write(struct.pack("<q", completed_sequence))


class LatestDetections:
    def __init__(self) -> None:
        self.lock = threading.Lock()
        self.boxes: list[tuple[int, int, int, int, str, float]] = []
        self.fps = 0.0

    def update(
        self,
        boxes: list[tuple[int, int, int, int, str, float]],
        fps: float,
    ) -> None:
        with self.lock:
            self.boxes = boxes
            self.fps = fps

    def snapshot(self) -> tuple[list[tuple[int, int, int, int, str, float]], float]:
        with self.lock:
            return list(self.boxes), self.fps


def metadata_loop(
    port: int,
    metadata: queue.Queue[dict[str, int]],
    stop: threading.Event,
) -> None:
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp:
        udp.bind(("0.0.0.0", port))
        udp.settimeout(0.5)
        while not stop.is_set():
            try:
                payload, _ = udp.recvfrom(4096)
            except socket.timeout:
                continue
            try:
                message = json.loads(payload.decode("utf-8"))
                item = {
                    "timestamp_ns": int(message["timestamp_ns"]),
                    "frame_number": int(message["frame_number"]),
                }
                if item["timestamp_ns"] <= 0 or item["frame_number"] < 0:
                    continue
                try:
                    metadata.put_nowait(item)
                except queue.Full:
                    metadata.get_nowait()
                    metadata.put_nowait(item)
            except (KeyError, TypeError, ValueError, UnicodeDecodeError, json.JSONDecodeError):
                continue


def inference_loop(
    model: YOLO,
    frames: queue.Queue[np.ndarray | None],
    detections: LatestDetections,
    args: argparse.Namespace,
) -> None:
    completed = 0
    started = time.monotonic()
    while True:
        frame = frames.get()
        if frame is None:
            return
        result = model.predict(
            frame,
            conf=args.confidence,
            imgsz=args.imgsz,
            device=args.device,
            verbose=False,
        )[0]
        boxes = []
        for box in result.boxes:
            x1, y1, x2, y2 = (int(value) for value in box.xyxy[0].tolist())
            class_id = int(box.cls.item())
            boxes.append(
                (x1, y1, x2, y2, str(model.names[class_id]), float(box.conf.item()))
            )
        completed += 1
        elapsed = max(time.monotonic() - started, 1e-6)
        detections.update(boxes, completed / elapsed)


def draw_detections(
    frame: np.ndarray,
    boxes: list[tuple[int, int, int, int, str, float]],
) -> None:
    colors = {"fire": (0, 0, 255), "smoke": (160, 160, 160)}
    for x1, y1, x2, y2, class_name, confidence in boxes:
        color = colors.get(class_name.lower(), (0, 255, 255))
        cv2.rectangle(frame, (x1, y1), (x2, y2), color, 3)
        label = f"{class_name} {confidence:.2f}"
        cv2.putText(
            frame,
            label,
            (x1, max(24, y1 - 8)),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.7,
            color,
            2,
            cv2.LINE_AA,
        )


def main() -> int:
    args = parse_args()
    if not args.model.is_file():
        raise FileNotFoundError(f"Model not found: {args.model}")
    if not 0 < args.confidence <= 1:
        raise ValueError("--confidence must be in (0, 1]")
    if min(args.width, args.height, args.fps, args.port) <= 0:
        raise ValueError("port, dimensions, and FPS must be positive")

    model = YOLO(str(args.model), task="detect")
    names = {int(key): str(value).lower() for key, value in model.names.items()}
    if set(names.values()) != CANONICAL_CLASSES:
        raise RuntimeError(f"Expected fire/smoke classes, model reports: {names}")

    frame_size = args.width * args.height * 3
    receiver = subprocess.Popen(receiver_command(args))
    frame_socket = connect_frame_socket(args.frame_port, receiver)
    display = None if args.no_window else subprocess.Popen(
        display_command(args), stdin=subprocess.PIPE
    )
    shared_size = SHARED_MEMORY_HEADER_SIZE + args.width * args.height * 4
    shared = mmap.mmap(-1, shared_size, tagname=args.shared_memory)
    detection_shared = mmap.mmap(
        -1, DETECTION_MEMORY_SIZE, tagname=DETECTION_MEMORY_NAME
    )
    inference_frames: queue.Queue[np.ndarray | None] = queue.Queue(maxsize=1)
    latest_detections = LatestDetections()
    frame_metadata: queue.Queue[dict[str, int]] = queue.Queue(maxsize=32)
    metadata_stop = threading.Event()
    metadata_thread = threading.Thread(
        target=metadata_loop,
        args=(args.metadata_port, frame_metadata, metadata_stop),
        name="video-frame-metadata",
        daemon=True,
    )
    metadata_thread.start()
    inference_thread = threading.Thread(
        target=inference_loop,
        args=(model, inference_frames, latest_detections, args),
        name="fire-smoke-yolo",
        daemon=True,
    )
    inference_thread.start()
    frame_count = 0
    started = time.monotonic()
    print(
        f"Listening on RTP/H.264 UDP {args.port}; "
        f"YOLO {args.width}x{args.height} @ {args.fps} FPS. Ctrl+C to stop.",
        flush=True,
    )

    try:
        while True:
            payload = read_exact(frame_socket, frame_size)
            if payload is None:
                raise RuntimeError("GStreamer receiver stopped or returned an incomplete frame")

            try:
                metadata = frame_metadata.get_nowait()
            except queue.Empty:
                metadata = None
            frame_timestamp_ns = (
                int(metadata["timestamp_ns"]) if metadata is not None else time.time_ns()
            )

            # frombuffer avoids an extra decode; copy owns the memory after this iteration.
            frame = np.frombuffer(payload, dtype=np.uint8).reshape(
                args.height, args.width, 3
            ).copy()
            try:
                inference_frames.put_nowait(frame.copy())
            except queue.Full:
                try:
                    inference_frames.get_nowait()
                except queue.Empty:
                    pass
                try:
                    inference_frames.put_nowait(frame.copy())
                except queue.Full:
                    pass

            annotated = frame
            boxes, inference_fps = latest_detections.snapshot()
            draw_detections(annotated, boxes)

            frame_count += 1
            elapsed = max(time.monotonic() - started, 1e-6)
            cv2.putText(
                annotated,
                f"VIDEO {frame_count / elapsed:.1f} FPS | "
                f"YOLO {inference_fps:.1f} FPS | detections {len(boxes)}",
                (16, 32),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.8,
                (0, 255, 255),
                2,
                cv2.LINE_AA,
            )
            annotated_bgra = cv2.cvtColor(annotated, cv2.COLOR_BGR2BGRA)
            publish_shared_frame(
                shared,
                annotated_bgra,
                frame_count,
                frame_timestamp_ns,
                metadata is not None,
            )
            publish_shared_detections(
                detection_shared, boxes, frame_count, frame_timestamp_ns
            )
            if display is not None and display.stdin is not None:
                display.stdin.write(annotated.tobytes())
                display.stdin.flush()
    except (KeyboardInterrupt, BrokenPipeError):
        return 0
    finally:
        try:
            inference_frames.put_nowait(None)
        except queue.Full:
            try:
                inference_frames.get_nowait()
            except queue.Empty:
                pass
            inference_frames.put_nowait(None)
        metadata_stop.set()
        frame_socket.close()
        shared.close()
        detection_shared.close()
        if display is not None and display.stdin:
            display.stdin.close()
        receiver.terminate()
        if display is not None:
            display.terminate()
        receiver.wait(timeout=5)
        if display is not None:
            display.wait(timeout=5)
        inference_thread.join(timeout=5)
        metadata_thread.join(timeout=2)


if __name__ == "__main__":
    raise SystemExit(main())
