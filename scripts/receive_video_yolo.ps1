[CmdletBinding()]
param(
    [ValidateRange(1, 65535)] [int]$Port = 5600,
    [ValidateRange(0, 127)] [int]$PayloadType = 96,
    [ValidateRange(0, 5000)] [int]$LatencyMs = 40,
    [ValidateRange(1, 65535)] [int]$MetadataPort = 5601,
    [ValidateRange(1, 120)] [int]$Fps = 10,
    [ValidateRange(16, 7680)] [int]$Width = 720,
    [ValidateRange(16, 4320)] [int]$Height = 1280,
    [ValidateSet("none", "clockwise", "counterclockwise", "rotate-180", "horizontal-flip", "vertical-flip", "upper-left-diagonal", "upper-right-diagonal")]
    [string]$Rotation = "upper-right-diagonal",
    [ValidateRange(0.01, 1.0)] [double]$Confidence = 0.25,
    [string]$Device = "cpu"
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $PSScriptRoot
$python = Join-Path $projectRoot "AI\.venv\Scripts\python.exe"
$script = Join-Path $projectRoot "AI\live_rtp_hazard.py"

if (-not (Test-Path -LiteralPath $python)) {
    throw "Python environment not found: $python"
}

$env:PATH = "C:\gstreamer\bin;$env:PATH"
$env:GST_PLUGIN_SCANNER = "C:\gstreamer\libexec\gstreamer-1.0\gst-plugin-scanner.exe"

& $python $script `
    --port $Port `
    --payload-type $PayloadType `
    --latency-ms $LatencyMs `
    --metadata-port $MetadataPort `
    --fps $Fps `
    --width $Width `
    --height $Height `
    --rotation $Rotation `
    --confidence $Confidence `
    --device $Device `
    --no-window

if ($LASTEXITCODE -ne 0) {
    throw "Live YOLO receiver exited with code $LASTEXITCODE."
}
