from __future__ import annotations

import argparse
import csv
import json
import os
import platform
import statistics
import time
from pathlib import Path

import cv2
import numpy as np
import onnx
import onnxruntime as ort


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Benchmark YOLO ONNX variants fairly on one machine.")
    parser.add_argument("models", nargs="+", type=Path)
    parser.add_argument("--source", type=Path, required=True)
    parser.add_argument("--runs", type=int, default=100)
    parser.add_argument("--warmup", type=int, default=10)
    parser.add_argument("--output", type=Path, required=True)
    return parser.parse_args()


def load_frame(path: Path) -> np.ndarray:
    if path.suffix.lower() in {".mp4", ".avi", ".mov", ".mkv"}:
        capture = cv2.VideoCapture(str(path))
        ok, frame = capture.read()
        capture.release()
        if not ok:
            raise RuntimeError(f"Could not read a frame from {path}")
        return frame
    frame = cv2.imread(str(path))
    if frame is None:
        raise RuntimeError(f"Could not read image {path}")
    return frame


def metadata(model_path: Path) -> dict[str, str]:
    model = onnx.load(model_path, load_external_data=False)
    return {item.key: item.value for item in model.metadata_props}


def percentile(values: list[float], q: float) -> float:
    return float(np.percentile(np.asarray(values, dtype=np.float64), q))


def benchmark(model_path: Path, frame: np.ndarray, warmup: int, runs: int) -> dict:
    available = ort.get_available_providers()
    providers = ["CPUExecutionProvider"]
    session = ort.InferenceSession(str(model_path), providers=providers)
    input_info = session.get_inputs()[0]
    shape = input_info.shape
    height = int(shape[2])
    width = int(shape[3])

    resized = cv2.resize(frame, (width, height), interpolation=cv2.INTER_LINEAR)
    rgb = cv2.cvtColor(resized, cv2.COLOR_BGR2RGB)
    tensor = np.ascontiguousarray(rgb.transpose(2, 0, 1), dtype=np.float32) / 255.0
    tensor = tensor[None, ...]

    for _ in range(warmup):
        session.run(None, {input_info.name: tensor})

    timings_ms: list[float] = []
    for _ in range(runs):
        started = time.perf_counter_ns()
        session.run(None, {input_info.name: tensor})
        timings_ms.append((time.perf_counter_ns() - started) / 1_000_000.0)

    mean_ms = statistics.fmean(timings_ms)
    meta = metadata(model_path)
    return {
        "model": model_path.name,
        "path": str(model_path.resolve()),
        "description": meta.get("description", ""),
        "classes": meta.get("names", ""),
        "format": "ONNX FP32",
        "input": f"{width}x{height}",
        "provider": session.get_providers()[0],
        "available_providers": available,
        "file_mb": model_path.stat().st_size / 1_000_000.0,
        "runs": runs,
        "warmup": warmup,
        "mean_ms": mean_ms,
        "median_ms": statistics.median(timings_ms),
        "p95_ms": percentile(timings_ms, 95),
        "min_ms": min(timings_ms),
        "max_ms": max(timings_ms),
        "fps_from_mean": 1000.0 / mean_ms,
    }


def main() -> None:
    args = parse_args()
    if args.runs < 10 or args.warmup < 1:
        raise ValueError("Use at least 10 measured runs and one warmup run.")
    frame = load_frame(args.source)
    results = [benchmark(path, frame, args.warmup, args.runs) for path in args.models]
    payload = {
        "benchmark_scope": "architecture/inference speed only; not an accuracy evaluation",
        "fairness_conditions": "same machine, CPUExecutionProvider, ONNX FP32, batch 1, 640x640, same frame",
        "system": {
            "platform": platform.platform(),
            "processor": platform.processor(),
            "python": platform.python_version(),
            "onnxruntime": ort.__version__,
            "cpu_count": os.cpu_count(),
        },
        "source": str(args.source.resolve()),
        "results": results,
    }
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(payload, ensure_ascii=False, indent=2), encoding="utf-8")
    csv_path = args.output.with_suffix(".csv")
    fields = ["model", "description", "file_mb", "input", "provider", "mean_ms", "median_ms", "p95_ms", "fps_from_mean", "runs"]
    with csv_path.open("w", encoding="utf-8-sig", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=fields, extrasaction="ignore")
        writer.writeheader(); writer.writerows(results)
    print(json.dumps(results, ensure_ascii=False, indent=2))
    print(args.output)
    print(csv_path)


if __name__ == "__main__":
    main()
