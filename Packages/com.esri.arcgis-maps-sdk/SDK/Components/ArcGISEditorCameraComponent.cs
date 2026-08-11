// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Attn: Contracts and Legal Department
// Environmental Systems Research Institute, Inc.
// 380 New York Street
// Redlands, California 92373
// USA
//
// email: legal@esri.com
#if UNITY_EDITOR
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using System;
using Esri.GameEngine.View;

namespace Esri.ArcGISMapsSDK.Components
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(HPTransform))]
	[AddComponentMenu("")]
	public class ArcGISEditorCameraComponent : ArcGISCameraComponent
	{
		private ArcGISMapComponent mapComponent;
		private double3? lastEditorCameraPosition = null;
		private double3 lastRootPosition;
		private HPTransform hpTransform;

		bool worldRepositionEnabled = false;

		// These values are used to adjust the clipping threshold.
		private const float AltitudeThreshold = 20000.0f;
		private const float FarClipDistance = 10000f;
		private const float NearClipDistance = 0.5f;

		// These values are used to adjust camera speed relative to altitude.
		private const float DefaultMaxSpeed = 10000f;
		private const float MaxSpeed = 600000.0f;
		private const float MinSpeed = 300.0f;
		private const float AltitudeScalar = 0.00001f;
		private const float AltitudePowerValue = 1.2f;
		private const float SpeedScalar = 0.02f;

		public bool WorldRepositionEnabled
		{
			get => worldRepositionEnabled;

			set
			{
				worldRepositionEnabled = value;
			}
		}

		private void Awake()
		{
			Debug.Assert(!Application.isPlaying, "ArcGISEditorCameraComponent shouldn't be used while playing");

			GetComponent<Camera>().enabled = false;

			hpTransform = GetComponent<HPTransform>();

			if (SceneView.lastActiveSceneView?.cameraSettings != null)
			{
				SceneView.lastActiveSceneView.cameraSettings.speedMax = DefaultMaxSpeed;
				SceneView.lastActiveSceneView.cameraSettings.accelerationEnabled = false;
			}
		}

		protected override void Initialize()
		{
			Debug.Assert(!Application.isPlaying, "ArcGISEditorCameraComponent shouldn't be used while playing");

			base.Initialize();

			if (mapComponent)
			{
				mapComponent.MapTypeChanged -= OnMapTypeChanged;
			}

			mapComponent = GetComponentInParent<ArcGISMapComponent>();

			Debug.Assert(mapComponent, "ArcGISEditorCameraComponent should be a child of ArcGISMapComponent");

			worldRepositionEnabled = mapComponent.RebaseWithSceneView;

			Invalidate();

			mapComponent.MapTypeChanged += OnMapTypeChanged;
		}

		private void Invalidate()
		{
			lastEditorCameraPosition = null;
		}

		private new void OnDisable()
		{
			Debug.Assert(!Application.isPlaying, "ArcGISEditorCameraComponent shouldn't be used while playing");

			mapComponent.MapTypeChanged -= OnMapTypeChanged;

			Invalidate();

			base.OnDisable();
		}

		private void OnMapTypeChanged()
		{
			Invalidate();
		}

		private new void Update()
		{
			Debug.Assert(!Application.isPlaying, "ArcGISEditorCameraComponent shouldn't be used while playing");

			if (SceneView.lastActiveSceneView)
			{
				var rootDeltaPosition = lastEditorCameraPosition.HasValue ? mapComponent.UniversePosition - lastRootPosition : double3.zero;
				hpTransform.UniversePosition = math.inverse(mapComponent.WorldMatrix).HomogeneousTransformPoint(SceneView.lastActiveSceneView.camera.transform.position.ToDouble3()) - rootDeltaPosition;
				hpTransform.UniverseRotation = mapComponent.UniverseRotation * SceneView.lastActiveSceneView.camera.transform.rotation;

				var camera = GetComponent<Camera>();
				camera.fieldOfView = SceneView.lastActiveSceneView.cameraSettings.fieldOfView;
				camera.aspect = SceneView.lastActiveSceneView.camera.aspect;

				if (mapComponent.HasSpatialReference())
				{
					var altitude = Math.Abs((float)mapComponent.View.AltitudeAtCartesianPosition(hpTransform.UniversePosition));

					// SceneView Camera NearPlane is updated a frame after update cameraSettings, because of that our GetCameraNearPlane doesn't work.
					// A very simple solution is implemented to resolve near-far distance problem
					var near = altitude > AltitudeThreshold ? FarClipDistance : NearClipDistance;
					var mapType = mapComponent.Map?.MapType ?? GameEngine.Map.ArcGISMapType.Global;
					var spatialReference = mapComponent.View.SpatialReference;

					SceneView.lastActiveSceneView.cameraSettings.nearClip = near;
					SceneView.lastActiveSceneView.cameraSettings.farClip = (float)Math.Max(near, Utils.FrustumHelpers.CalculateFarPlaneDistance(altitude, mapType, spatialReference));

					SceneView.lastActiveSceneView.cameraSettings.speed = (float)(Math.Pow(Math.Min(altitude * AltitudeScalar, 1), AltitudePowerValue) * MaxSpeed + MinSpeed) * SpeedScalar;
				}

				if (!lastEditorCameraPosition.HasValue)
				{
					lastEditorCameraPosition = hpTransform.UniversePosition;
					SceneView.lastActiveSceneView.cameraSettings.dynamicClip = false;
				}
				else
				{
					var delta = lastEditorCameraPosition - hpTransform.UniversePosition;
					lastEditorCameraPosition = hpTransform.UniversePosition;

					if (worldRepositionEnabled && delta.Equals(double3.zero) && !mapComponent.UniversePosition.Equals(hpTransform.UniversePosition))
					{
						mapComponent.UniversePosition = hpTransform.UniversePosition;
						SceneView.lastActiveSceneView.pivot -= SceneView.lastActiveSceneView.camera.transform.position;
					}
				}

				lastRootPosition = mapComponent.UniversePosition;
			}

			base.Update();
		}

		protected override bool ShouldPushUpdates()
		{
			return mapComponent &&
				   mapComponent.ShouldEditorComponentBeUpdated() &&
				   mapComponent.DataFetchWithSceneView && !Application.isPlaying;
		}
	}
}
#endif
