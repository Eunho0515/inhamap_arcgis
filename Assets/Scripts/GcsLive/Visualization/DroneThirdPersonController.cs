using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Peh.Gcs.Live.Visualization
{
    /// <summary>Manual WASD flight controls with a geographic third-person camera.</summary>
    public sealed class DroneThirdPersonController : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float moveSpeedMetresPerSecond = 8f;
        [SerializeField, Min(0.1f)] private float verticalSpeedMetresPerSecond = 5f;
        [SerializeField, Min(1f)] private float yawSpeedDegreesPerSecond = 70f;
        [SerializeField, Min(1f)] private float cameraDistanceMetres = 5f;
        [SerializeField, Min(0.1f)] private float cameraHeightMetres = 4f;
        [SerializeField, Range(0f, 90f)] private float cameraPitchDegrees = 52f;

        private const double MetresPerDegreeLatitude = 111320.0;

        private ArcGisTelemetryPresenter telemetryPresenter;
        private ArcGISLocationComponent droneLocation;
        private ArcGISLocationComponent cameraLocation;
        private ArcGISCameraControllerComponent mapCameraController;
        private float yawDegrees;

        public void Configure(ArcGisTelemetryPresenter presenter)
        {
            telemetryPresenter = presenter;
        }

        private void LateUpdate()
        {
            EnsureComponents();
            if (droneLocation == null || cameraLocation == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                var forward = Axis(keyboard.wKey.isPressed, keyboard.sKey.isPressed);
                var right = Axis(keyboard.dKey.isPressed, keyboard.aKey.isPressed);
                var yawInput = Axis(keyboard.eKey.isPressed, keyboard.qKey.isPressed);
                var vertical = Axis(
                    keyboard.spaceKey.isPressed,
                    keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed);

                if (forward != 0f || right != 0f || yawInput != 0f || vertical != 0f)
                {
                    // Manual input takes ownership so the last telemetry packet cannot
                    // snap the drone back to its previous geographic position.
                    if (telemetryPresenter != null)
                        telemetryPresenter.enabled = false;

                    yawDegrees = Mathf.Repeat(
                        yawDegrees + yawInput * yawSpeedDegreesPerSecond * Time.unscaledDeltaTime,
                        360f);

                    MoveDrone(forward, right, vertical);
                }
            }

            droneLocation.Rotation = new ArcGISRotation(yawDegrees, 0, 0);
            UpdateThirdPersonCamera();
        }

        private void MoveDrone(float forward, float right, float vertical)
        {
            var input = Vector2.ClampMagnitude(new Vector2(right, forward), 1f);
            var yawRadians = yawDegrees * Mathf.Deg2Rad;
            var east = input.y * Mathf.Sin(yawRadians) + input.x * Mathf.Cos(yawRadians);
            var north = input.y * Mathf.Cos(yawRadians) - input.x * Mathf.Sin(yawRadians);
            var distance = moveSpeedMetresPerSecond * Time.unscaledDeltaTime;
            var current = droneLocation.Position;
            var metresPerDegreeLongitude = MetresPerDegreeLatitude *
                                           System.Math.Cos(current.Y * Mathf.Deg2Rad);

            droneLocation.Position = new ArcGISPoint(
                current.X + east * distance / metresPerDegreeLongitude,
                current.Y + north * distance / MetresPerDegreeLatitude,
                current.Z + vertical * verticalSpeedMetresPerSecond * Time.unscaledDeltaTime,
                current.SpatialReference);
        }

        private void UpdateThirdPersonCamera()
        {
            var drone = droneLocation.Position;
            var yawRadians = yawDegrees * Mathf.Deg2Rad;
            var behindEast = -Mathf.Sin(yawRadians) * cameraDistanceMetres;
            var behindNorth = -Mathf.Cos(yawRadians) * cameraDistanceMetres;
            var metresPerDegreeLongitude = MetresPerDegreeLatitude *
                                           System.Math.Cos(drone.Y * Mathf.Deg2Rad);

            cameraLocation.Position = new ArcGISPoint(
                drone.X + behindEast / metresPerDegreeLongitude,
                drone.Y + behindNorth / MetresPerDegreeLatitude,
                drone.Z + cameraHeightMetres,
                drone.SpatialReference);
            cameraLocation.Rotation = new ArcGISRotation(
                yawDegrees,
                cameraPitchDegrees,
                0);
        }

        private void EnsureComponents()
        {
            if (droneLocation == null)
                droneLocation = GetComponent<ArcGISLocationComponent>();

            var mainCamera = Camera.main;
            if (mainCamera == null)
                return;

            if (cameraLocation == null)
                cameraLocation = mainCamera.GetComponent<ArcGISLocationComponent>();

            if (mapCameraController == null)
                mapCameraController = mainCamera.GetComponent<ArcGISCameraControllerComponent>();

            if (mapCameraController != null && mapCameraController.enabled)
                mapCameraController.enabled = false;
        }

        private static float Axis(bool positive, bool negative)
        {
            return (positive ? 1f : 0f) - (negative ? 1f : 0f);
        }
    }
}
