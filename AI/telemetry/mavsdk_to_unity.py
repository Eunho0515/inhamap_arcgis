from __future__ import annotations

import argparse
import asyncio
import json
import socket
import time
from dataclasses import dataclass

from mavsdk import System


@dataclass
class LatestTelemetry:
    timestamp_ns: int = 0
    position: object | None = None
    attitude: object | None = None
    velocity: object | None = None
    battery: object | None = None
    armed: bool = False
    in_air: bool = False
    flight_mode: str = "UNKNOWN"


async def update(stream, state: LatestTelemetry, field: str) -> None:
    async for value in stream:
        setattr(state, field, value)


async def update_timestamp(drone: System, state: LatestTelemetry) -> None:
    async for timestamp_us in drone.telemetry.unix_epoch_time():
        state.timestamp_ns = int(timestamp_us) * 1000


async def publish(
    drone: System,
    state: LatestTelemetry,
    unity_host: str,
    unity_port: int,
    vehicle_id: str,
    rate_hz: float,
) -> None:
    target = (unity_host, unity_port)
    sequence = 0
    period = 1.0 / rate_hz
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp:
        while True:
            started = time.monotonic()
            if state.position is not None:
                timestamp_ns = state.timestamp_ns or time.time_ns()
                attitude = state.attitude
                velocity = state.velocity
                battery = state.battery
                packet = {
                    "schema_version": "1.0",
                    "sequence_number": sequence,
                    "timestamp_ns": timestamp_ns,
                    "vehicle_id": vehicle_id,
                    "position_valid": True,
                    "latitude_deg": state.position.latitude_deg,
                    "longitude_deg": state.position.longitude_deg,
                    "altitude_m": state.position.absolute_altitude_m,
                    "altitude_reference": "amsl",
                    "attitude_valid": attitude is not None,
                    "yaw_deg": attitude.yaw_deg if attitude else 0.0,
                    "velocity_valid": velocity is not None,
                    "velocity_north_mps": velocity.north_m_s if velocity else 0.0,
                    "velocity_east_mps": velocity.east_m_s if velocity else 0.0,
                    "velocity_down_mps": velocity.down_m_s if velocity else 0.0,
                    "battery_valid": battery is not None,
                    "battery_remaining_percent": max(0.0, min(100.0, battery.remaining_percent * 100.0)) if battery else 0.0,
                    "battery_voltage_v": battery.voltage_v if battery else 0.0,
                    "armed": state.armed,
                    "in_air": state.in_air,
                    "flight_mode": getattr(state.flight_mode, "name", str(state.flight_mode)),
                }
                udp.sendto(json.dumps(packet, separators=(",", ":")).encode(), target)
                sequence += 1
            await asyncio.sleep(max(0.0, period - (time.monotonic() - started)))


async def run(args: argparse.Namespace) -> None:
    drone = System()
    await drone.connect(system_address=args.mavlink)
    print(f"Waiting for MAVLink system on {args.mavlink} ...", flush=True)
    async for connection in drone.core.connection_state():
        if connection.is_connected:
            break
    print(f"Forwarding MAVSDK telemetry to {args.unity_host}:{args.unity_port}", flush=True)

    state = LatestTelemetry()
    tasks = [
        asyncio.create_task(update_timestamp(drone, state)),
        asyncio.create_task(update(drone.telemetry.position(), state, "position")),
        asyncio.create_task(update(drone.telemetry.attitude_euler(), state, "attitude")),
        asyncio.create_task(update(drone.telemetry.velocity_ned(), state, "velocity")),
        asyncio.create_task(update(drone.telemetry.battery(), state, "battery")),
        asyncio.create_task(update(drone.telemetry.armed(), state, "armed")),
        asyncio.create_task(update(drone.telemetry.in_air(), state, "in_air")),
        asyncio.create_task(update(drone.telemetry.flight_mode(), state, "flight_mode")),
    ]
    try:
        await publish(drone, state, args.unity_host, args.unity_port, args.vehicle_id, args.rate_hz)
    finally:
        for task in tasks:
            task.cancel()
        await asyncio.gather(*tasks, return_exceptions=True)


def main() -> None:
    parser = argparse.ArgumentParser(description="Forward MAVSDK telemetry to Unity JSON/UDP")
    parser.add_argument("--mavlink", default="udpin://0.0.0.0:14540")
    parser.add_argument("--unity-host", default="127.0.0.1")
    parser.add_argument("--unity-port", type=int, default=5005)
    parser.add_argument("--vehicle-id", default="inha-log-2")
    parser.add_argument("--rate-hz", type=float, default=20.0)
    args = parser.parse_args()
    if args.rate_hz <= 0:
        parser.error("--rate-hz must be positive")
    asyncio.run(run(args))


if __name__ == "__main__":
    main()
