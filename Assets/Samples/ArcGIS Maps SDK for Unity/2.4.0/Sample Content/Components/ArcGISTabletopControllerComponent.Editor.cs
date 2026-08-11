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
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils;
using System;
using System.Collections.Generic;

namespace Esri.ArcGISMapsSDK.Samples.Components
{
#if UNITY_EDITOR
	public partial class ArcGISTabletopControllerComponent
	{
		[Flags]
		public enum Properties
		{
			None = 0,
			AutomaticElevationEnabled = 1 << 0,
			Center = 1 << 1,
			ElevationOffset = 1 << 2,
			Height = 1 << 3,
			Shape = 1 << 4,
			Width = 1 << 5,
		}

		private Properties updatedProperties = Properties.None;

		private bool? lastAutomaticElevationEnabled;

		private ArcGISPointInstanceData lastCenter;

		private double? lastElevationOffset;

		private double? lastHeight;

		private MapExtentShapes? lastShape;

		private double? lastWidth;

		[NonSerialized]
		private bool lastValuesInitialized = false;

		private void OnEnableEditor()
		{
			lastAutomaticElevationEnabled = automaticElevationEnabled;

			lastCenter = (ArcGISPointInstanceData)center?.Clone();

			lastElevationOffset = elevationOffset;

			lastHeight = height;

			lastShape = shape;

			lastWidth = width;

			lastValuesInitialized = true;
		}

		private void OnValidate()
		{
			if (!lastValuesInitialized)
			{
				return;
			}

			if (lastAutomaticElevationEnabled != automaticElevationEnabled)
			{
				lastAutomaticElevationEnabled = automaticElevationEnabled;

				updatedProperties |= Properties.AutomaticElevationEnabled;
			}

			if (!EqualityComparer<ArcGISPointInstanceData>.Default.Equals(lastCenter, center))
			{
				lastCenter = (ArcGISPointInstanceData)center.Clone();

				updatedProperties |= Properties.Center;
			}

			if (lastElevationOffset != elevationOffset)
			{
				lastElevationOffset = elevationOffset;

				updatedProperties |= Properties.ElevationOffset;
			}

			if (lastHeight != height)
			{
				lastHeight = height;

				updatedProperties |= Properties.Height;
			}

			if (lastShape != shape)
			{
				lastShape = shape;

				updatedProperties |= Properties.Shape;
			}

			if (lastWidth != width)
			{
				lastWidth = width;

				updatedProperties |= Properties.Width;
			}
		}

		private void UpdateEditor()
		{
			try
			{
				if (updatedProperties.HasFlag(Properties.AutomaticElevationEnabled))
				{
					OnAutomaticElevationEnabledChanged();
				}

				if (updatedProperties.HasFlag(Properties.Center) ||
					updatedProperties.HasFlag(Properties.Height) ||
					updatedProperties.HasFlag(Properties.Shape) ||
					updatedProperties.HasFlag(Properties.Width))
				{
					if (updatedProperties.HasFlag(Properties.Center))
					{
						centerObjectPtr = center?.FromInstanceData();
					}

					UpdateExtent();
				}

				if (updatedProperties.HasFlag(Properties.ElevationOffset))
				{
					UpdateFixedElevationOffset();
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
