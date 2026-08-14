using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Peh.Gcs.Live.Visualization
{
    /// <summary>Manual flight controls with switchable first/third-person cameras.</summary>
    public sealed class DroneThirdPersonController : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float moveSpeedMetresPerSecond = 8f;
        [SerializeField, Min(0.1f)] private float verticalSpeedMetresPerSecond = 5f;
        [SerializeField, Min(1f)] private float yawSpeedDegreesPerSecond = 70f;
        [SerializeField, Min(0f)] private float cameraForwardOffsetMetres = 0.18f;
        [SerializeField, Min(0f)] private float cameraHeightMetres = 0.08f;
        // ArcGIS pitch 90 is horizontal; 100 looks 10 degrees above the horizon.
        [SerializeField, Range(0f, 180f)] private float cameraPitchDegrees = 100f;
        [SerializeField, Min(1f)] private float thirdPersonDistanceMetres = 6f;
        [SerializeField, Min(0f)] private float thirdPersonHeightMetres = 2f;
        [SerializeField, Min(0.1f)] private float thirdPersonFollowSharpness = 8f;

        private const double MetresPerDegreeLatitude = 111320.0;

        private ArcGisTelemetryPresenter telemetryPresenter;
        private ArcGISLocationComponent droneLocation;
        private ArcGISLocationComponent cameraLocation;
        private ArcGISCameraControllerComponent mapCameraController;
        private Camera firstPersonCamera;
        private Camera thirdPersonCamera;
        private AudioListener firstPersonListener;
        private AudioListener thirdPersonListener;
        private GUIStyle cameraButtonStyle;
        private float yawDegrees;
        private bool manualControlActive;
        private bool thirdPersonActive;

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
                    manualControlActive = true;

                    yawDegrees = Mathf.Repeat(
                        yawDegrees + yawInput * yawSpeedDegreesPerSecond * Time.unscaledDeltaTime,
                        360f);

                    MoveDrone(forward, right, vertical);
                }
            }

            // Live telemetry owns the geographic rotation until explicit manual input.
            // Otherwise this controller would overwrite MAVSDK attitude every frame.
            if (manualControlActive)
                droneLocation.Rotation = new ArcGISRotation(yawDegrees, 0, 0);
            UpdateFirstPersonCamera();
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

        private void UpdateFirstPersonCamera()
        {
            var drone = droneLocation.Position;
            var cameraHeading = manualControlActive || telemetryPresenter == null
                ? yawDegrees
                : telemetryPresenter.DisplayedYawDegrees;
            var yawRadians = cameraHeading * Mathf.Deg2Rad;
            var forwardEast = Mathf.Sin(yawRadians) * cameraForwardOffsetMetres;
            var forwardNorth = Mathf.Cos(yawRadians) * cameraForwardOffsetMetres;
            var metresPerDegreeLongitude = MetresPerDegreeLatitude *
                                           System.Math.Cos(drone.Y * Mathf.Deg2Rad);

            cameraLocation.Position = new ArcGISPoint(
                drone.X + forwardEast / metresPerDegreeLongitude,
                drone.Y + forwardNorth / MetresPerDegreeLatitude,
                drone.Z + cameraHeightMetres,
                drone.SpatialReference);
            cameraLocation.Rotation = new ArcGISRotation(
                cameraHeading,
                cameraPitchDegrees,
                0);
        }

        private void EnsureComponents()
        {
            if (droneLocation == null)
                droneLocation = GetComponent<ArcGISLocationComponent>();

            if (firstPersonCamera == null)
            {
                firstPersonCamera = Camera.main;
                if (firstPersonCamera == null)
                    return;
                firstPersonListener = firstPersonCamera.GetComponent<AudioListener>();
            }

            if (cameraLocation == null)
                cameraLocation = firstPersonCamera.GetComponent<ArcGISLocationComponent>();

            if (mapCameraController == null)
                mapCameraController = firstPersonCamera.GetComponent<ArcGISCameraControllerComponent>();

            if (mapCameraController != null && mapCameraController.enabled)
                mapCameraController.enabled = false;

            if (thirdPersonCamera == null)
                CreateThirdPersonCamera();
        }

        private void CreateThirdPersonCamera()
        {
            var cameraObject = new GameObject("Live UAV Third Person Camera");
            thirdPersonCamera = cameraObject.AddComponent<Camera>();
            thirdPersonCamera.CopyFrom(firstPersonCamera);
            thirdPersonCamera.enabled = false;
            cameraObject.tag = "Untagged";

            thirdPersonListener = cameraObject.AddComponent<AudioListener>();
            thirdPersonListener.enabled = false;
            cameraObject.transform.position = firstPersonCamera.transform.position;
            cameraObject.transform.rotation = firstPersonCamera.transform.rotation;
        }

        private void UpdateThirdPersonCamera()
        {
            if (firstPersonCamera == null || thirdPersonCamera == null)
                return;

            var firstTransform = firstPersonCamera.transform;
            var targetPosition = firstTransform.position -
                                 firstTransform.forward * thirdPersonDistanceMetres +
                                 Vector3.up * thirdPersonHeightMetres;
            var blend = 1f - Mathf.Exp(-thirdPersonFollowSharpness * Time.unscaledDeltaTime);
            var thirdTransform = thirdPersonCamera.transform;
            thirdTransform.position = Vector3.Lerp(thirdTransform.position, targetPosition, blend);

            var lookTarget = transform.position + Vector3.up * 0.25f;
            var lookDirection = lookTarget - thirdTransform.position;
            if (lookDirection.sqrMagnitude > 0.001f)
                thirdTransform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        private void SetThirdPersonActive(bool active)
        {
            thirdPersonActive = active;
            if (firstPersonCamera != null)
                firstPersonCamera.enabled = !active;
            if (thirdPersonCamera != null)
                thirdPersonCamera.enabled = active;
            if (firstPersonListener != null)
                firstPersonListener.enabled = !active;
            if (thirdPersonListener != null)
                thirdPersonListener.enabled = active;
        }

        private void OnGUI()
        {
            cameraButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };

            const float width = 230f;
            const float height = 44f;
            var buttonRect = new Rect(
                (Screen.width - width) * 0.5f,
                Screen.height - height - 18f,
                width,
                height);
            var label = thirdPersonActive ? "1인칭 카메라로 전환" : "3인칭 카메라로 전환";
            if (GUI.Button(buttonRect, label, cameraButtonStyle))
                SetThirdPersonActive(!thirdPersonActive);
        }

        private void OnDestroy()
        {
            if (thirdPersonCamera != null)
                Destroy(thirdPersonCamera.gameObject);
        }

        private static float Axis(bool positive, bool negative)
        {
            return (positive ? 1f : 0f) - (negative ? 1f : 0f);
        }
    }
}
