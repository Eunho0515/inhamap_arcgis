[CmdletBinding()]
param(
    [ValidateRange(1, 65535)]
    [int]$Port = 5600,

    [ValidateRange(0, 127)]
    [int]$PayloadType = 96,

    [ValidateRange(0, 5000)]
    [int]$LatencyMs = 100,

    [string]$BindAddress = "0.0.0.0"
)

$ErrorActionPreference = "Stop"

$gstLaunch = Get-Command "gst-launch-1.0.exe" -ErrorAction SilentlyContinue
if ($null -eq $gstLaunch) {
    $knownPaths = @(
        "C:\gstreamer\bin\gst-launch-1.0.exe",
        "C:\gstreamer\1.0\msvc_x86_64\bin\gst-launch-1.0.exe",
        "C:\Program Files\gstreamer\1.0\msvc_x86_64\bin\gst-launch-1.0.exe"
    )
    $installedPath = $knownPaths | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
    if ($installedPath) {
        $gstExecutable = $installedPath
    }
    else {
        throw @"
gst-launch-1.0.exe was not found.
Install the 64-bit GStreamer MSVC Runtime and Development packages, then add
C:\gstreamer\bin to PATH.
"@
    }
}
else {
    $gstExecutable = $gstLaunch.Source
}

$caps = "application/x-rtp,media=video,clock-rate=90000,encoding-name=H264,payload=$PayloadType"

Write-Host "Listening for Ubuntu RTP/H.264 on ${BindAddress}:$Port"
Write-Host "Payload type: $PayloadType, jitter-buffer latency: $LatencyMs ms"
Write-Host "Press Ctrl+C to stop. The window opens after an H.264 keyframe arrives."

& $gstExecutable -e -v `
    udpsrc "address=$BindAddress" "port=$Port" "caps=$caps" `
    '!' rtpjitterbuffer "latency=$LatencyMs" 'drop-on-latency=true' `
    '!' rtph264depay `
    '!' h264parse `
    '!' avdec_h264 `
    '!' videoconvert `
    '!' fpsdisplaysink 'video-sink=autovideosink' 'sync=false' 'text-overlay=true'

if ($LASTEXITCODE -ne 0) {
    throw "GStreamer exited with code $LASTEXITCODE."
}
