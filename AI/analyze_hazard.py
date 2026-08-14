from __future__ import annotations

import argparse
import hashlib
import json
import os
from datetime import datetime, timezone
from pathlib import Path

_config_dir = Path(__file__).resolve().parent / ".config" / "ultralytics"
_config_dir.mkdir(parents=True, exist_ok=True)
os.environ.setdefault("YOLO_CONFIG_DIR", str(_config_dir))
os.environ.setdefault("MPLCONFIGDIR", str(_config_dir / "matplotlib"))

import cv2
import onnxruntime as ort
from ultralytics import YOLO


CANONICAL_CLASSES = {"fire", "smoke"}


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Run fire/smoke YOLO and export detections for Unity."
    )
    parser.add_argument("video", type=Path)
    parser.add_argument(
        "--model", type=Path, default=Path("models/hazard/best.onnx")
    )
    parser.add_argument(
        "--output", type=Path, default=Path("output/detections.json")
    )
    parser.add_argument("--confidence", type=float, default=0.25)
    parser.add_argument("--sample-fps", type=float, default=2.0)
    parser.add_argument("--start-ns", type=int, default=0)
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    if not args.video.is_file():
        raise FileNotFoundError(f"Video not found: {args.video}")
    if not args.model.is_file():
        raise FileNotFoundError(f"Model not found: {args.model}")
    if not 0 < args.confidence <= 1 or args.sample_fps <= 0:
        raise ValueError("confidence must be in (0, 1] and sample-fps must be positive")

    session = ort.InferenceSession(
        str(args.model), providers=["CPUExecutionProvider"]
    )
    model = YOLO(str(args.model), task="detect")
    names = {int(key): str(value).lower() for key, value in model.names.items()}
    unknown = set(names.values()) - CANONICAL_CLASSES
    if unknown or set(names.values()) != CANONICAL_CLASSES:
        raise RuntimeError(
            f"Expected exactly fire/smoke classes, model reports: {names}"
        )

    capture = cv2.VideoCapture(str(args.video))
    if not capture.isOpened():
        raise RuntimeError(f"Cannot open video: {args.video}")
    fps = float(capture.get(cv2.CAP_PROP_FPS))
    frame_count = int(capture.get(cv2.CAP_PROP_FRAME_COUNT))
    width = int(capture.get(cv2.CAP_PROP_FRAME_WIDTH))
    height = int(capture.get(cv2.CAP_PROP_FRAME_HEIGHT))
    if fps <= 0 or frame_count <= 0 or width <= 0 or height <= 0:
        raise RuntimeError("Video timing or dimensions are invalid")

    interval = max(1, round(fps / args.sample_fps))
    samples: list[dict[str, object]] = []
    frame_index = 0
    try:
        while True:
            ok, frame = capture.read()
            if not ok:
                break
            if frame_index % interval == 0:
                relative_ns = round(frame_index / fps * 1_000_000_000)
                result = model.predict(
                    frame, conf=args.confidence, verbose=False, device="cpu"
                )[0]
                objects = []
                for object_index, box in enumerate(result.boxes):
                    class_id = int(box.cls.item())
                    xyxy = box.xyxyn[0].tolist()
                    x1, y1, x2, y2 = (float(value) for value in xyxy)
                    objects.append(
                        {
                            "object_id": f"det-{frame_index:08d}-{object_index:03d}",
                            "class_name": names[class_id],
                            "confidence": float(box.conf.item()),
                            "adjusted_confidence": float(box.conf.item()),
                            "bounding_box": {
                                "x": max(0.0, min(1.0, x1)),
                                "y": max(0.0, min(1.0, y1)),
                                "width": max(0.0, min(1.0, x2) - max(0.0, x1)),
                                "height": max(0.0, min(1.0, y2) - max(0.0, y1)),
                            },
                            "source_model": "DarshanM0di/fireandsmoke",
                        }
                    )
                samples.append(
                    {
                        "session_time_ns": relative_ns,
                        "timestamp_ns": args.start_ns + relative_ns
                        if args.start_ns
                        else 0,
                        "objects": objects,
                    }
                )
            frame_index += 1
    finally:
        capture.release()

    digest = hashlib.sha256(args.model.read_bytes()).hexdigest()
    payload = {
        "schema_version": "1.0.0",
        "time_basis": "session_time_ns",
        "timestamp_basis": "utc_unix_ns" if args.start_ns else "unknown",
        "video": {
            "path": str(args.video.resolve()),
            "width": width,
            "height": height,
            "fps": fps,
            "frame_count": frame_count,
            "duration_ns": round(frame_count / fps * 1_000_000_000),
        },
        "analysis": {
            "model": "DarshanM0di/fireandsmoke",
            "model_sha256": digest,
            "classes": ["fire", "smoke"],
            "confidence_threshold": args.confidence,
            "sample_fps": args.sample_fps,
            "device": session.get_providers()[0],
            "generated_at_utc": datetime.now(timezone.utc).isoformat(),
        },
        "samples": samples,
    }
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(payload, ensure_ascii=False), encoding="utf-8")
    count = sum(len(sample["objects"]) for sample in samples)
    print(f"Wrote {len(samples)} samples / {count} detections to {args.output}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
