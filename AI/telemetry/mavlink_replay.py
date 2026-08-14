from __future__ import annotations

import argparse
import bisect
import csv
import math
import time
from dataclasses import dataclass
from pathlib import Path

from pymavlink import mavutil


@dataclass(frozen=True)
class Location:
    timestamp_ns: int
    elapsed: float
    latitude: float
    longitude: float
    altitude: float
    speed: float
    bearing: float
    horizontal_accuracy: float
    vertical_accuracy: float


@dataclass(frozen=True)
class Attitude:
    elapsed: float
    roll: float
    pitch: float
    yaw: float


def load_rows(log_dir: Path) -> tuple[list[Location], list[Attitude]]:
    with (log_dir / "Location.csv").open(encoding="utf-8-sig", newline="") as file:
        locations = [
            Location(
                timestamp_ns=int(row["time"]),
                elapsed=float(row["seconds_elapsed"]),
                latitude=float(row["latitude"]),
                longitude=float(row["longitude"]),
                altitude=float(row["altitude"]),
                speed=max(0.0, float(row["speed"])),
                bearing=float(row["bearing"]) % 360.0,
                horizontal_accuracy=max(0.0, float(row["horizontalAccuracy"])),
                vertical_accuracy=max(0.0, float(row["verticalAccuracy"])),
            )
            for row in csv.DictReader(file)
        ]
    with (log_dir / "Orientation.csv").open(encoding="utf-8-sig", newline="") as file:
        attitudes = [
            Attitude(
                elapsed=float(row["seconds_elapsed"]),
                roll=float(row["roll"]),
                pitch=float(row["pitch"]),
                yaw=float(row["yaw"]),
            )
            for row in csv.DictReader(file)
        ]
    if not locations or not attitudes:
        raise ValueError("Location.csv and Orientation.csv must contain samples")
    return locations, attitudes


def nearest_index(times: list[float], value: float) -> int:
    right = bisect.bisect_left(times, value)
    if right == 0:
        return 0
    if right == len(times):
        return right - 1
    return right if times[right] - value < value - times[right - 1] else right - 1


def replay(
    log_dir: Path,
    endpoint: str = "udpout:127.0.0.1:14540",
    speed: float = 1.0,
    rate_hz: float = 20.0,
) -> None:
    if speed <= 0 or rate_hz <= 0:
        raise ValueError("speed and rate-hz must be positive")
    locations, attitudes = load_rows(log_dir)
    location_times = [row.elapsed for row in locations]
    attitude_times = [row.elapsed for row in attitudes]
    start = min(location_times[0], attitude_times[0])
    end = max(location_times[-1], attitude_times[-1])
    link = mavutil.mavlink_connection(
        endpoint, source_system=1, source_component=1, dialect="common"
    )
    started_at = time.monotonic()
    next_heartbeat = 0.0
    sample = 0
    try:
        while True:
            elapsed = start + sample / rate_hz
            if elapsed > end:
                break
            deadline = started_at + (elapsed - start) / speed
            remaining = deadline - time.monotonic()
            if remaining > 0:
                time.sleep(remaining)

            location = locations[nearest_index(location_times, elapsed)]
            attitude = attitudes[nearest_index(attitude_times, elapsed)]
            boot_ms = min(4_294_967_295, round((elapsed - start) * 1000))
            if elapsed >= next_heartbeat:
                link.mav.heartbeat_send(
                    mavutil.mavlink.MAV_TYPE_QUADROTOR,
                    mavutil.mavlink.MAV_AUTOPILOT_PX4,
                    mavutil.mavlink.MAV_MODE_FLAG_CUSTOM_MODE_ENABLED,
                    0,
                    mavutil.mavlink.MAV_STATE_ACTIVE,
                )
                next_heartbeat = elapsed + 1.0

            # Preserve the Sensor Logger UTC epoch across MAVLink. MAVSDK exposes
            # SYSTEM_TIME through telemetry.unix_epoch_time().
            link.mav.system_time_send(location.timestamp_ns // 1000, boot_ms)

            bearing_rad = math.radians(location.bearing)
            north = location.speed * math.cos(bearing_rad)
            east = location.speed * math.sin(bearing_rad)
            latitude = round(location.latitude * 10_000_000)
            longitude = round(location.longitude * 10_000_000)
            altitude_mm = round(location.altitude * 1000)
            heading_cdeg = round(location.bearing * 100) % 36000
            link.mav.global_position_int_send(
                boot_ms,
                latitude,
                longitude,
                altitude_mm,
                altitude_mm,
                round(north * 100),
                round(east * 100),
                0,
                heading_cdeg,
            )
            link.mav.gps_raw_int_send(
                location.timestamp_ns // 1000,
                3,
                latitude,
                longitude,
                altitude_mm,
                min(65_535, round(location.horizontal_accuracy * 100)),
                min(65_535, round(location.vertical_accuracy * 100)),
                min(65_535, round(location.speed * 100)),
                heading_cdeg,
                10,
            )
            link.mav.attitude_send(
                boot_ms,
                attitude.roll,
                attitude.pitch,
                attitude.yaw,
                0.0,
                0.0,
                0.0,
            )
            sample += 1
    finally:
        link.close()


def main() -> None:
    parser = argparse.ArgumentParser(description="Replay Sensor Logger CSV as MAVLink UDP")
    parser.add_argument("log_dir", type=Path)
    parser.add_argument("--endpoint", default="udpout:127.0.0.1:14540")
    parser.add_argument("--speed", type=float, default=1.0)
    parser.add_argument("--rate-hz", type=float, default=20.0)
    args = parser.parse_args()
    replay(args.log_dir, args.endpoint, args.speed, args.rate_hz)


if __name__ == "__main__":
    main()
