from __future__ import annotations

import argparse
import asyncio
import threading
from pathlib import Path

from mavsdk import System

from mavlink_replay import replay


async def first(stream, timeout: float = 15.0):
    return await asyncio.wait_for(anext(stream), timeout=timeout)


async def first_moving_velocity(stream, timeout: float = 15.0):
    async def receive():
        async for velocity in stream:
            if abs(velocity.north_m_s) + abs(velocity.east_m_s) > 0.05:
                return velocity

    return await asyncio.wait_for(receive(), timeout=timeout)


async def verify(log_dir: Path, port: int) -> None:
    drone = System(port=50051)
    sender = threading.Thread(
        target=replay,
        args=(log_dir, f"udpout:127.0.0.1:{port}", 10.0, 20.0),
        daemon=True,
    )
    connect_task = asyncio.create_task(
        drone.connect(system_address=f"udpin://0.0.0.0:{port}")
    )
    await asyncio.sleep(1.0)
    sender.start()
    await connect_task

    async def connected():
        async for state in drone.core.connection_state():
            if state.is_connected:
                return state

    await asyncio.wait_for(connected(), timeout=15.0)
    position, attitude, velocity = await asyncio.gather(
        first(drone.telemetry.position()),
        first(drone.telemetry.attitude_euler()),
        first_moving_velocity(drone.telemetry.velocity_ned()),
    )
    print("MAVSDK connected: true")
    print(
        "position:",
        f"lat={position.latitude_deg:.7f}",
        f"lon={position.longitude_deg:.7f}",
        f"abs_alt={position.absolute_altitude_m:.2f}m",
    )
    print(
        "attitude:",
        f"roll={attitude.roll_deg:.2f}deg",
        f"pitch={attitude.pitch_deg:.2f}deg",
        f"yaw={attitude.yaw_deg:.2f}deg",
    )
    print(
        "velocity_ned:",
        f"north={velocity.north_m_s:.2f}m/s",
        f"east={velocity.east_m_s:.2f}m/s",
        f"down={velocity.down_m_s:.2f}m/s",
    )
    sender.join(timeout=10.0)


def main() -> None:
    parser = argparse.ArgumentParser(description="Verify CSV MAVLink replay with MAVSDK")
    parser.add_argument("log_dir", type=Path)
    parser.add_argument("--port", type=int, default=14540)
    args = parser.parse_args()
    asyncio.run(verify(args.log_dir, args.port))


if __name__ == "__main__":
    main()
