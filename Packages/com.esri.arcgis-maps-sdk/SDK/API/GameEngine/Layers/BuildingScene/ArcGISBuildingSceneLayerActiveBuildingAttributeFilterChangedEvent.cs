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

namespace Esri.GameEngine.Layers.BuildingScene
{
    /// <summary>
    /// A callback used to indicate that the property <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer.ActiveBuildingAttributeFilter">ArcGISBuildingSceneLayer.ActiveBuildingAttributeFilter</see> has changed.
    /// </summary>
    /// <since>1.4.0</since>
    public delegate void ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEvent();
    
    internal delegate void ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventInternal(IntPtr userData);
    
    internal class ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventHandler : Unity.ArcGISEventHandler<ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            callback();
        }
    }
}