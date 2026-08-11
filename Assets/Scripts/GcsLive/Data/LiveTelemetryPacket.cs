using System;

namespace Peh.Gcs.Live.Data
{
    /// <summary>
    /// Versioned JSON message accepted by the Unity live telemetry receiver.
    /// Units are SI except latitude/longitude and attitude, which are degrees.
    /// Missing optional values should be accompanied by the corresponding validity flag.
    /// </summary>
    [Serializable]
    public sealed class LiveTelemetryPacket
    {
        public string schema_version = "1.0";
        public long sequence_number;
        public long timestamp_ns;
        public string vehicle_id = "vehicle-1";

        public bool position_valid;
        public double latitude_deg;
        public double longitude_deg;
        public double altitude_m;
        public string altitude_reference = "unknown";

        public bool attitude_valid;
        public float roll_deg;
        public float pitch_deg;
        public float yaw_deg;

        public bool velocity_valid;
        public float velocity_north_mps;
        public float velocity_east_mps;
        public float velocity_down_mps;

        public bool battery_valid;
        public float battery_remaining_percent;
        public float battery_voltage_v;

        public bool armed;
        public bool in_air;
        public string flight_mode = "UNKNOWN";

        public bool TryValidate(out string error)
        {
            if (schema_version != "1.0")
            {
                error = $"Unsupported schema_version '{schema_version}'.";
                return false;
            }

            if (sequence_number < 0 || timestamp_ns < 0)
            {
                error = "sequence_number and timestamp_ns must be non-negative.";
                return false;
            }

            if (position_valid &&
                (latitude_deg < -90.0 || latitude_deg > 90.0 ||
                 longitude_deg < -180.0 || longitude_deg > 180.0 ||
                 double.IsNaN(altitude_m) || double.IsInfinity(altitude_m)))
            {
                error = "WGS84 position is outside its valid range.";
                return false;
            }

            if (battery_valid &&
                (battery_remaining_percent < 0f || battery_remaining_percent > 100f))
            {
                error = "battery_remaining_percent must be in the range 0..100.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
