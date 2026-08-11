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
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Components
{
	public partial class ArcGISLocationComponent
	{
		private enum SyncAction
		{
			None,
			Pull,
			Push,
			PushInternal
		};

		private HPTransform hpTransform;

		[SerializeField]
		private bool isInitialized = false;

		private SyncAction syncAction = SyncAction.None;

		private double3 universePosition;
		private quaternion universeRotation;

		private void LateUpdateSync()
		{
			// If changes to HPTransform were not pushed by us, recalculate position and rotation
			var hasPositionChanged = !universePosition.Equals(hpTransform.UniversePosition);
			var hasRotationChanged = !universeRotation.Equals(hpTransform.UniverseRotation);

			StartSync(hasPositionChanged || hasRotationChanged ? SyncAction.Pull : syncAction);
		}

		private void OnEnableSync()
		{
			// Our OnEnable runs after HPTransform's
			universePosition = hpTransform.UniversePosition;
			universeRotation = hpTransform.UniverseRotation;

			// If isInitialized is false, it means the component has just been added so we need to
			// calculate originPosition from the hpRoot
			StartSync(!isInitialized ? SyncAction.Pull : SyncAction.Push);
		}

		private bool PullChangesFromHPTransform()
		{
			var geographic = mapComponent.View.WorldToGeographic(hpTransform.UniversePosition);

			if (geographic?.IsEmpty ?? true)
			{
				return false;
			}

			universePosition = hpTransform.UniversePosition;
			universeRotation = hpTransform.UniverseRotation;

			try
			{
				// When creating a location component with a specific SR and then sliding it around or updating the HPTransform
				// this method can change the SR of the Location component which is strange behavior
				positionObjectPtr = GeoUtils.ProjectToSpatialReference(geographic, position.SpatialReference.FromInstanceData()); // this is a no-op if the sr is already the same
			}
			catch
			{
				positionObjectPtr = geographic;
			}

			position = positionObjectPtr.ToInstanceData();

			var cartesianPosition = universePosition;
			var cartesianRotation = universeRotation.ToQuaterniond();

			rotation = GeoUtils.FromCartesianRotation(cartesianPosition, cartesianRotation, mapComponent.View.SpatialReference, mapComponent.MapType);

#if UNITY_EDITOR
			// Avoid a false change detection
			lastPosition = (ArcGISPointInstanceData)position?.Clone();
#endif

			return true;
		}

		private bool PushChangesToHPTransform()
		{
			if (positionObjectPtr?.IsEmpty ?? true)
			{
				return false;
			}

			var cartesianPosition = mapComponent.View.GeographicToWorld(positionObjectPtr);

			if (!cartesianPosition.IsValid())
			{
				return false;
			}

			var cartesianRotation = GeoUtils.ToCartesianRotation(cartesianPosition, rotation, mapComponent.View.SpatialReference, mapComponent.MapType);

			universePosition = cartesianPosition;
			universeRotation = cartesianRotation.ToQuaternion();

			hpTransform.UniversePosition = universePosition;
			hpTransform.UniverseRotation = universeRotation;

			return true;
		}

		private void StartSync(SyncAction syncAction)
		{
			this.syncAction = syncAction;

			SyncPositionWithHPTransform();
		}

		private void SyncPositionWithHPTransform()
		{
			if (syncAction == SyncAction.None || !mapComponent || !mapComponent.HasSpatialReference())
			{
				return;
			}

			bool changeWasApplied;

			if (syncAction == SyncAction.Push || syncAction == SyncAction.PushInternal)
			{
				changeWasApplied = PushChangesToHPTransform();
			}
			else
			{
				changeWasApplied = PullChangesFromHPTransform();
			}

			if (changeWasApplied)
			{
				if (syncAction != SyncAction.PushInternal)
				{
					UpdateElevationMonitor();
				}

				isInitialized = true;
				syncAction = SyncAction.None;
			}
		}

		private void UpdateSync()
		{
			SyncPositionWithHPTransform();
		}
	}
}
