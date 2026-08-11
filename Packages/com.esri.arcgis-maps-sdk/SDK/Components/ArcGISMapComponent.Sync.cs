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
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.View;
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Esri.ArcGISMapsSDK.Components
{
	public partial class ArcGISMapComponent
	{
		private enum SyncAction
		{
			None,
			Pull,
			Push
		};

		public UnityEvent RootChanged = new();

		private HPRoot hpRoot;

		[SerializeField]
		private bool isInitialized = false;

		private SyncAction syncAction = SyncAction.None;

		private double3 universePosition;
		public double3 UniversePosition
		{
			get => hpRoot.RootUniversePosition;
			set
			{
				if (universePosition.Equals(value) && hpRoot.RootUniversePosition.Equals(value))
				{
					return;
				}

				universePosition = value;

				OnUniversePositionChanged();
			}
		}

		private Quaternion universeRotation;
		public Quaternion UniverseRotation
		{
			get => hpRoot.RootUniverseRotation;
		}

		public double4x4 WorldMatrix
		{
			get
			{
				return hpRoot.WorldMatrix;
			}
		}

		private void LateUpdateSync()
		{
			// If changes to HPRoot were not pushed by us, recalculate originPosition
			var hasPositionChanged = !universePosition.Equals(hpRoot.RootUniversePosition);
			var hasRotationChanged = !universeRotation.Equals(hpRoot.RootUniverseRotation);

			StartSync(hasPositionChanged || hasRotationChanged ? SyncAction.Pull : syncAction);
		}

		private void OnEnableSync()
		{
			// Our OnEnable runs after HPRoot's
			universePosition = hpRoot.RootUniversePosition;
			universeRotation = hpRoot.RootUniverseRotation;

			// If isInitialized is false, it means the component has just been added so we need to
			// calculate originPosition from the hpRoot
			StartSync(!isInitialized ? SyncAction.Pull : SyncAction.Push);
		}

		private void OnUniversePositionChanged()
		{
			var tangentToWorld = View.GetENUReference(universePosition).ToQuaterniond();

			universeRotation = tangentToWorld.ToQuaternion();

			UpdateHPRoot();
			SyncPositionWithHPRoot();

			RootChanged.Invoke();
		}

		private bool PullChangesFromHPRoot()
		{
			var geographic = View.WorldToGeographic(hpRoot.RootUniversePosition);

			if (geographic?.IsEmpty ?? true)
			{
				return false;
			}

			universePosition = hpRoot.RootUniversePosition;
			universeRotation = hpRoot.RootUniverseRotation;

			originPositionObjectPtr = geographic;
			originPosition = geographic.ToInstanceData();

#if UNITY_EDITOR
			// Avoid a false change detection
			lastOriginPosition = (ArcGISPointInstanceData)originPosition?.Clone();
#endif

			return true;
		}

		private bool PushChangesToHPRoot()
		{
			if (originPositionObjectPtr?.IsEmpty ?? true)
			{
				return false;
			}

			var cartesianPosition = View.GeographicToWorld(originPositionObjectPtr);

			if (!cartesianPosition.IsValid())
			{
				return false;
			}

			UniversePosition = cartesianPosition;

			return true;
		}

		private void StartSync(SyncAction syncAction)
		{
			this.syncAction = syncAction;

			SyncPositionWithHPRoot();
		}

		private void SyncPositionWithHPRoot()
		{
			if (syncAction == SyncAction.None || !HasSpatialReference())
			{
				return;
			}

			bool changeWasApplied;

			if (syncAction == SyncAction.Push)
			{
				changeWasApplied = PushChangesToHPRoot();
			}
			else
			{
				changeWasApplied = PullChangesFromHPRoot();
			}

			if (changeWasApplied)
			{
				isInitialized = true;
				syncAction = SyncAction.None;
			}
		}

		public void UpdateHPRoot()
		{
			hpRoot.SetRootTR(universePosition, universeRotation);
		}
	}
}
