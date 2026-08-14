#!/usr/bin/env bash
set -euo pipefail

port=5600
bind_address="0.0.0.0"
payload_type=96
latency_ms=100

usage() {
  cat <<'EOF'
Receive and display an RTP/H.264 UDP stream with GStreamer.

Usage:
  ./scripts/receive_video_rtp_h264.sh [options]

Options:
  --port PORT             UDP listen port (default: 5600)
  --bind-address ADDRESS  Local address to bind (default: 0.0.0.0)
  --payload-type PT       RTP dynamic payload type (default: 96)
  --latency-ms MS         Jitter-buffer latency (default: 100)
  -h, --help              Show this help
EOF
}

while (($# > 0)); do
  case "$1" in
    --port)
      port="${2:?--port requires a value}"
      shift 2
      ;;
    --bind-address)
      bind_address="${2:?--bind-address requires a value}"
      shift 2
      ;;
    --payload-type)
      payload_type="${2:?--payload-type requires a value}"
      shift 2
      ;;
    --latency-ms)
      latency_ms="${2:?--latency-ms requires a value}"
      shift 2
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown option: $1" >&2
      usage >&2
      exit 2
      ;;
  esac
done

for value in "$port" "$payload_type" "$latency_ms"; do
  if [[ ! "$value" =~ ^[0-9]+$ ]]; then
    echo "Port, payload type, and latency must be non-negative integers." >&2
    exit 2
  fi
done

if ((port < 1 || port > 65535)); then
  echo "Port must be between 1 and 65535." >&2
  exit 2
fi

if ! command -v gst-launch-1.0 >/dev/null 2>&1; then
  echo "gst-launch-1.0 was not found." >&2
  echo "Install it with:" >&2
  echo "  sudo apt install gstreamer1.0-tools gstreamer1.0-plugins-base \\" >&2
  echo "    gstreamer1.0-plugins-good gstreamer1.0-plugins-bad gstreamer1.0-libav" >&2
  exit 127
fi

echo "Listening for RTP/H.264 on ${bind_address}:${port} (PT=${payload_type}, latency=${latency_ms} ms)"
echo "Press Ctrl+C to stop. The video window opens after the first decodable keyframe arrives."

exec gst-launch-1.0 -e -v \
  udpsrc address="$bind_address" port="$port" \
    caps="application/x-rtp,media=video,clock-rate=90000,encoding-name=H264,payload=${payload_type}" \
  ! rtpjitterbuffer latency="$latency_ms" drop-on-latency=true \
  ! rtph264depay \
  ! h264parse \
  ! avdec_h264 \
  ! videoconvert \
  ! fpsdisplaysink video-sink=autovideosink sync=false text-overlay=true
