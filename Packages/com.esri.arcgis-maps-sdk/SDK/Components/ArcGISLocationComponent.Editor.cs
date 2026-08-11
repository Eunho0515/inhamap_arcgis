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
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Components
{
#if UNITY_EDITOR
	public partial class ArcGISLocationComponent
	{
		[Flags]
		public enum Properties
		{
			None = 0,
			Position = 1 << 0,
			Rotation = 1 << 1,
			SurfacePlacementMode = 1 << 2,
			SurfacePlacementOffset = 1 << 3,
		}

		private Properties updatedProperties = Properties.None;

		private ArcGISPointInstanceData lastPosition;

		private ArcGISRotation? lastRotation;

		private ArcGISSurfacePlacementMode? lastSurfacePlacementMode;

		private double? lastSurfacePlacementOffset;

		[NonSerialized]
		private bool lastValuesInitialized = false;

		private void OnEnableEditor()
		{
			if (Undo.isProcessing)
			{
				return;
			}

			lastPosition = (ArcGISPointInstanceData)position?.Clone();

			lastRotation = rotation;

			lastSurfacePlacementMode = surfacePlacementMode;

			lastSurfacePlacementOffset = surfacePlacementOffset;

			lastValuesInitialized = true;
		}

		private void OnValidate()
		{
			if (!lastValuesInitialized)
			{
				return;
			}

			if (!EqualityComparer<ArcGISPointInstanceData>.Default.Equals(lastPosition, position))
			{
				lastPosition = (ArcGISPointInstanceData)position?.Clone();

				updatedProperties |= Properties.Position;
			}

			if (lastRotation != rotation)
			{
				lastRotation = rotation;

				updatedProperties |= Properties.Rotation;
			}

			if (lastSurfacePlacementMode != surfacePlacementMode)
			{
				lastSurfacePlacementMode = surfacePlacementMode;

				updatedProperties |= Properties.SurfacePlacementMode;
			}

			if (lastSurfacePlacementOffset != surfacePlacementOffset)
			{
				lastSurfacePlacementOffset = surfacePlacementOffset;

				updatedProperties |= Properties.SurfacePlacementOffset;
			}
		}

		private void UpdateEditor()
		{
			try
			{
				if (updatedProperties.HasFlag(Properties.Position))
				{
					positionObjectPtr = position?.FromInstanceData();

					OnPositionChanged();
				}

				if (updatedProperties.HasFlag(Properties.Rotation))
				{
					OnRotationChanged();
				}

				if (updatedProperties.HasFlag(Properties.SurfacePlacementMode) ||
					updatedProperties.HasFlag(Properties.SurfacePlacementOffset))
				{
					OnSurfacePlacementChanged();
				}
			}
			finally
			{
				updatedProperties = Properties.None;
			}
		}
	}
#endif
}
