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
using Esri.ArcGISMapsSDK.Utils;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Elevation;
using Esri.GameEngine.Geometry;
using Esri.HPFramework;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Components
{
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(HPTransform))]
	[AddComponentMenu("Tools/ArcGIS Maps SDK/ArcGIS Location")]
	public partial class ArcGISLocationComponent : MonoBehaviour, ISerializationCallbackReceiver
	{
		private ArcGISElevationMonitor elevationMonitor;
		protected ArcGISMapComponent mapComponent;

		[SerializeField]
		private ArcGISPointInstanceData position = null;
		private ArcGISPoint positionObjectPtr;
		public ArcGISPoint Position
		{
			get => positionObjectPtr;
			set
			{
				if (position == null || value == null || !position.Equals(value))
				{
					position = value?.ToInstanceData();

#if UNITY_EDITOR
					lastPosition = (ArcGISPointInstanceData)position?.Clone();
					EditorUtility.SetDirty(this);
#endif
				}

				if (positionObjectPtr == null || value == null || !positionObjectPtr.Equals(value))
				{
					positionObjectPtr = value;

					OnPositionChanged();
				}
			}
		}

		[SerializeField]
		private ArcGISRotation rotation;
		public ArcGISRotation Rotation
		{
			get => rotation;
			set
			{
				if (rotation.Equals(value))
				{
					return;
				}

				rotation = value;

#if UNITY_EDITOR
				lastRotation = rotation;
				EditorUtility.SetDirty(this);
#endif

				OnRotationChanged();
			}
		}

		[SerializeField]
		private ArcGISSurfacePlacementMode surfacePlacementMode = ArcGISSurfacePlacementMode.AbsoluteHeight;
		public ArcGISSurfacePlacementMode SurfacePlacementMode
		{
			get => surfacePlacementMode;
			set
			{
				if (surfacePlacementMode == value)
				{
					return;
				}

				surfacePlacementMode = value;

#if UNITY_EDITOR
				lastSurfacePlacementMode = surfacePlacementMode;
				EditorUtility.SetDirty(this);
#endif

				OnSurfacePlacementChanged();
			}
		}

		[SerializeField]
		private double surfacePlacementOffset = 0.0;
		public double SurfacePlacementOffset
		{
			get => surfacePlacementOffset;
			set
			{
				if (surfacePlacementOffset == value)
				{
					return;
				}

				surfacePlacementOffset = value;

#if UNITY_EDITOR
				lastSurfacePlacementOffset = value;
				EditorUtility.SetDirty(this);
#endif

				OnSurfacePlacementChanged();
			}
		}

		protected void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			hpTransform = GetComponent<HPTransform>();

			mapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();

			if (!mapComponent)
			{
				Debug.LogWarning("Unable to find a parent ArcGISMapComponent.");

				return;
			}

			// When SR changes recalculate from geographic position
			mapComponent.View.SpatialReferenceChanged += () =>
			{
				if (syncAction != SyncAction.None)
				{
					return;
				}

				StartSync(!isInitialized ? SyncAction.Pull : SyncAction.Push);
			};
		}

		protected void LateUpdate()
		{
			LateUpdateSync();
		}

		public enum Version
		{
			// Before any version changes were made
			BeforeCustomVersionWasAdded = 0,

			// Add isInitialized
			Initialized_2_1 = 1,

			// -----<new versions can be added above this line>-------------------------------------------------
			VersionPlusOne,

			LatestVersion = VersionPlusOne - 1
		}

		[SerializeField]
		Version version = Version.BeforeCustomVersionWasAdded;

		public void OnAfterDeserialize()
		{
			if (version < Version.Initialized_2_1)
			{
				isInitialized = true;
			}

			version = Version.LatestVersion;
		}

		public void OnBeforeSerialize()
		{
			version = Version.LatestVersion;
		}

		protected void OnEnable()
		{
			positionObjectPtr = position?.FromInstanceData();

			OnEnableSync();

#if UNITY_EDITOR
			OnEnableEditor();
#endif
		}

		private void OnPositionChanged()
		{
			StartSync(SyncAction.Push);
		}

		private void OnRotationChanged()
		{
			StartSync(SyncAction.Push);
		}

		private void OnSurfacePlacementChanged()
		{
			// Reset the position to the user's
			positionObjectPtr = position?.FromInstanceData();

			StartSync(SyncAction.Push);
		}

		protected void OnTransformParentChanged()
		{
			Initialize();

			// We consider the parent change a transform change so we need to calculate position
			// and rotation from the transform
			StartSync(SyncAction.Pull);
		}

		public static void SetPositionAndRotation(GameObject gameObject, ArcGISPoint geographicPosition, ArcGISRotation geographicRotation)
		{
			var locationComponent = gameObject.GetComponent<ArcGISLocationComponent>();

			if (locationComponent)
			{
				locationComponent.Position = geographicPosition;
				locationComponent.Rotation = geographicRotation;

				return;
			}

			var hpTransform = gameObject.GetComponent<HPTransform>();

			if (!hpTransform)
			{
				throw new InvalidOperationException(gameObject.name + " requires an HPTransform");
			}

			var mapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();

			if (!mapComponent)
			{
				throw new InvalidOperationException(gameObject.name + " should be child of a ArcGISMapComponent");
			}

			if (!mapComponent.HasSpatialReference())
			{
				throw new InvalidOperationException("View must have a spatial reference");
			}

			var cartesianPosition = mapComponent.View.GeographicToWorld(geographicPosition);

			hpTransform.UniversePosition = cartesianPosition;
			hpTransform.UniverseRotation = GeoUtils.ToCartesianRotation(cartesianPosition, geographicRotation, mapComponent.View.SpatialReference, mapComponent.MapType).ToQuaternion();
		}

		private void Update()
		{
			UpdateSync();

#if UNITY_EDITOR
			UpdateEditor();
#endif
		}

		private void UpdateElevationMonitor()
		{
			if (elevationMonitor)
			{
				elevationMonitor.PositionChanged = null;
			}

			if (!mapComponent)
			{
				return;
			}

			if (elevationMonitor)
			{
				mapComponent.View.ElevationProvider.UnregisterMonitor(elevationMonitor);
			}

			if (surfacePlacementMode == ArcGISSurfacePlacementMode.AbsoluteHeight)
			{
				return;
			}

			elevationMonitor = new ArcGISElevationMonitor(
				position.FromInstanceData(),
				ArcGISElevationMode.RelativeToGround,
				surfacePlacementMode == ArcGISSurfacePlacementMode.OnTheGround ? 0 : surfacePlacementOffset
			);

			elevationMonitor.PositionChanged += delegate (ArcGISPoint position)
			{
				positionObjectPtr = position;

				StartSync(SyncAction.PushInternal);
			};

			mapComponent.View.ElevationProvider.RegisterMonitor(elevationMonitor);
		}
	}
}
