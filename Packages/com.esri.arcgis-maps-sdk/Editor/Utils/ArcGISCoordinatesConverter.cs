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
using Esri.GameEngine.Geometry;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	public interface IArcGISCoordinatesConverterInterface
	{
		public delegate void OnRootChangedEventHandler();

		event OnRootChangedEventHandler OnRootChanged;

		Vector3 GeographicToEngine(ArcGISPoint point);
		ArcGISPoint EngineToGeographic(Vector3 vector);
	}

	internal class ArcGISCoordinatesConverter : IArcGISCoordinatesConverterInterface
	{
		private IArcGISMapComponentInterface mapComponentInterface;

		public event IArcGISCoordinatesConverterInterface.OnRootChangedEventHandler OnRootChanged;

		public ArcGISCoordinatesConverter(IArcGISMapComponentInterface mapComponentInterface)
		{
			this.mapComponentInterface = mapComponentInterface;

			if (mapComponentInterface is ArcGISMapComponent mapComponent)
			{
				mapComponent.RootChanged.AddListener(OnRootChangedCallback);
			}
		}

		public Vector3 GeographicToEngine(ArcGISPoint point)
		{
			return mapComponentInterface.GeographicToEngine(point);
		}

		public ArcGISPoint EngineToGeographic(Vector3 vector)
		{
			return mapComponentInterface.EngineToGeographic(vector);
		}

		private void OnRootChangedCallback()
		{
			if (OnRootChanged != null)
			{
				OnRootChanged();
			}
		}
	}
}
