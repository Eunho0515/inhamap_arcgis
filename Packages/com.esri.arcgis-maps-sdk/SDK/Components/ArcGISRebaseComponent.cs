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
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Components
{
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(HPTransform))]
	[AddComponentMenu("Tools/ArcGIS Maps SDK/ArcGIS Rebase")]
	public class ArcGISRebaseComponent : MonoBehaviour
	{
		private ArcGISMapComponent mapComponent;
		private HPTransform hpTransform;
		private double3 lastPosition;

		void OnEnable()
		{
			hpTransform = GetComponent<HPTransform>();

			UpdateMapComponent();

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				enabled = false;
			}
#endif
		}

		void Start()
		{
			if (!mapComponent)
			{
				Debug.LogError("An ArcGISMapComponent could not be found. Please make sure this GameObject is a child of a GameObject with an ArcGISMapComponent attached");

				enabled = false;
				return;
			}
		}

		private void LateUpdate()
		{
			if (!mapComponent || !mapComponent.HasSpatialReference())
			{
				// Defer until we have a spatial reference
				return;
			}

			if (!lastPosition.Equals(hpTransform.UniversePosition))
			{
				lastPosition = hpTransform.UniversePosition;

				// Avoid conversion to geographic coordinates
				mapComponent.UniversePosition = lastPosition;
			}
		}

		void OnTransformParentChanged()
		{
			UpdateMapComponent();
		}

		private void UpdateMapComponent()
		{
			mapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();
		}
	}
}
