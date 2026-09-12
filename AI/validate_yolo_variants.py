from __future__ import annotations

import argparse
import csv
import json
from pathlib import Path

from ultralytics import YOLO


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Validate fine-tuned YOLO variants on one held-out dataset.")
    parser.add_argument("models", nargs="+", type=Path)
    parser.add_argument("--data", type=Path, required=True, help="YOLO dataset YAML with a held-out val split")
    parser.add_argument("--imgsz", type=int, default=640)
    parser.add_argument("--batch", type=int, default=1)
    parser.add_argument("--device", default="cpu")
    parser.add_argument("--output", type=Path, required=True)
    return parser.parse_args()


def main() -> None:
    args = parse_args()
    if not args.data.is_file():
        raise FileNotFoundError(f"Validation dataset YAML not found: {args.data}")
    rows = []
    for model_path in args.models:
        metrics = YOLO(str(model_path)).val(
            data=str(args.data), imgsz=args.imgsz, batch=args.batch,
            device=args.device, plots=True, verbose=False,
        )
        per_class = []
        names = metrics.names
        for index, ap in enumerate(metrics.box.maps):
            per_class.append({"class": names[index], "map50_95": float(ap)})
        rows.append({
            "model": model_path.name,
            "precision": float(metrics.box.mp),
            "recall": float(metrics.box.mr),
            "map50": float(metrics.box.map50),
            "map50_95": float(metrics.box.map),
            "per_class": per_class,
        })

    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(rows, ensure_ascii=False, indent=2), encoding="utf-8")
    csv_path = args.output.with_suffix(".csv")
    with csv_path.open("w", encoding="utf-8-sig", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=["model", "precision", "recall", "map50", "map50_95"], extrasaction="ignore")
        writer.writeheader(); writer.writerows(rows)
    print(json.dumps(rows, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
