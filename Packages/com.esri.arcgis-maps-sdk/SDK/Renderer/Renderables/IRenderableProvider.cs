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
using Esri.GameEngine.RCQ;
using System.Collections.Generic;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.Renderables
{
	internal interface IRenderableProvider
	{
		public IEnumerable<IRenderable> TerrainMaskingMeshes { get; }

		public IReadOnlyDictionary<uint, IRenderable> Renderables { get; }

		public IRenderable CreateRenderable(uint id, ArcGISRenderableType renderableType, ulong layerId);
		public void DestroyRenderable(uint id);

		public IRenderable GetRenderableFrom(GameObject gameObject);
	}
}
