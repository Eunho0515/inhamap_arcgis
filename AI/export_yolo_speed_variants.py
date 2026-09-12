from pathlib import Path
import shutil
from ultralytics import YOLO


root = Path(__file__).resolve().parent
source = root / ".venv" / "Lib" / "site-packages" / "ultralytics" / "cfg" / "models" / "26" / "yolo26.yaml"
output = root / "experiments" / "yolo_comparison" / "models"
output.mkdir(parents=True, exist_ok=True)

for scale in ("n", "s"):
    config = output / f"yolo26{scale}.yaml"
    shutil.copyfile(source, config)
    model = YOLO(str(config))
    model.export(format="onnx", imgsz=640, simplify=True, dynamic=False, batch=1, device="cpu")
