from __future__ import annotations

import argparse
import asyncio
import json
import socket
import threading
from pathlib import Path

import mavsdk_to_unity
from mavlink_replay import replay


async def verify(log_dir: Path, mavlink_port: int, unity_port: int) -> None:
    args = argparse.Namespace(
        mavlink=f"udpin://0.0.0.0:{mavlink_port}",
        unity_host="127.0.0.1",
        unity_port=unity_port,
        vehicle_id="inha-log-2",
        rate_hz=20.0,
    )
    bridge = asyncio.create_task(mavsdk_to_unity.run(args))
    await asyncio.sleep(1.0)
    sender = threading.Thread(
        target=replay,
        args=(log_dir, f"udpout:127.0.0.1:{mavlink_port}", 10.0, 20.0),
        daemon=True,
    )
    sender.start()
    loop = asyncio.get_running_loop()
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp:
        udp.bind(("127.0.0.1", unity_port))
        udp.setblocking(False)
        data, _ = await asyncio.wait_for(loop.sock_recvfrom(udp, 65535), timeout=15.0)
    packet = json.loads(data)
    print(json.dumps(packet, indent=2))
    assert packet["schema_version"] == "1.0"
    assert packet["timestamp_ns"] > 1_000_000_000_000_000_000
    assert packet["position_valid"]
    bridge.cancel()
    await asyncio.gather(bridge, return_exceptions=True)


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("log_dir", type=Path)
    args = parser.parse_args()
    asyncio.run(verify(args.log_dir, 14542, 5006))


if __name__ == "__main__":
    main()
