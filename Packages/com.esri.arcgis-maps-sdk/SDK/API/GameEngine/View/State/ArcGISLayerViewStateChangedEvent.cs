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
using System.Runtime.InteropServices;
using System;

namespace Esri.GameEngine.View.State
{
    /// <summary>
    /// Called when a layer's view state has changed.
    /// </summary>
    /// <remarks>
    /// This will be called when a layers view state has changed.
    /// You should do very little in this function.
    /// </remarks>
    /// <seealso cref="GameEngine.View.ArcGISView">ArcGISView</seealso>
    /// <since>1.0.0</since>
    public delegate void ArcGISLayerViewStateChangedEvent(GameEngine.Layers.Base.ArcGISLayer layer, ArcGISLayerViewState arcGISLayerViewState);
    
    internal delegate void ArcGISLayerViewStateChangedEventInternal(IntPtr userData, IntPtr layer, IntPtr arcGISLayerViewState);
    
    internal class ArcGISLayerViewStateChangedEventHandler : Unity.ArcGISEventHandler<ArcGISLayerViewStateChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISLayerViewStateChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr layer, IntPtr arcGISLayerViewState)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISLayerViewStateChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            GameEngine.Layers.Base.ArcGISLayer localLayer = null;
            
            if (layer != IntPtr.Zero)
            {
                var objectType = GameEngine.Layers.Base.PInvoke.RT_GELayer_getObjectType(layer, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGIS3DObjectSceneLayer:
                        localLayer = new GameEngine.Layers.ArcGIS3DObjectSceneLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISBuildingSceneLayer:
                        localLayer = new GameEngine.Layers.ArcGISBuildingSceneLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGaussianSplatLayer:
                        localLayer = new GameEngine.Layers.ArcGISGaussianSplatLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGroupLayer:
                        localLayer = new GameEngine.Layers.ArcGISGroupLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISImageLayer:
                        localLayer = new GameEngine.Layers.ArcGISImageLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISIntegratedMeshLayer:
                        localLayer = new GameEngine.Layers.ArcGISIntegratedMeshLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISOGC3DTilesLayer:
                        localLayer = new GameEngine.Layers.ArcGISOGC3DTilesLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointCloudLayer:
                        localLayer = new GameEngine.Layers.ArcGISPointCloudLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointSceneLayer:
                        localLayer = new GameEngine.Layers.ArcGISPointSceneLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnknownLayer:
                        localLayer = new GameEngine.Layers.ArcGISUnknownLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnsupportedLayer:
                        localLayer = new GameEngine.Layers.ArcGISUnsupportedLayer(layer);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISVectorTileLayer:
                        localLayer = new GameEngine.Layers.ArcGISVectorTileLayer(layer);
                        break;
                    default:
                        localLayer = new GameEngine.Layers.Base.ArcGISLayer(layer);
                        break;
                }
            }
            
            ArcGISLayerViewState localArcGISLayerViewState = null;
            
            if (arcGISLayerViewState != IntPtr.Zero)
            {
                localArcGISLayerViewState = new ArcGISLayerViewState(arcGISLayerViewState);
            }
            
            callback(localLayer, localArcGISLayerViewState);
        }
    }
}