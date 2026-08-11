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
    /// Called when an elevation source view state has changed.
    /// </summary>
    /// <remarks>
    /// This will be called when an elevation source view state has changed.
    /// You should do very little in this function.
    /// </remarks>
    /// <seealso cref="GameEngine.View.ArcGISView">ArcGISView</seealso>
    /// <since>1.0.0</since>
    public delegate void ArcGISElevationSourceViewStateChangedEvent(GameEngine.Elevation.Base.ArcGISElevationSource layer, ArcGISElevationSourceViewState arcGISElevationSourceViewState);
    
    internal delegate void ArcGISElevationSourceViewStateChangedEventInternal(IntPtr userData, IntPtr layer, IntPtr arcGISElevationSourceViewState);
    
    internal class ArcGISElevationSourceViewStateChangedEventHandler : Unity.ArcGISEventHandler<ArcGISElevationSourceViewStateChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISElevationSourceViewStateChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr layer, IntPtr arcGISElevationSourceViewState)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISElevationSourceViewStateChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            GameEngine.Elevation.Base.ArcGISElevationSource localLayer = null;
            
            if (layer != IntPtr.Zero)
            {
                var objectType = GameEngine.Elevation.Base.PInvoke.RT_GEElevationSource_getObjectType(layer, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Elevation.Base.ArcGISElevationSourceType.ArcGISImageElevationSource:
                        localLayer = new GameEngine.Elevation.ArcGISImageElevationSource(layer);
                        break;
                    default:
                        localLayer = new GameEngine.Elevation.Base.ArcGISElevationSource(layer);
                        break;
                }
            }
            
            ArcGISElevationSourceViewState localArcGISElevationSourceViewState = null;
            
            if (arcGISElevationSourceViewState != IntPtr.Zero)
            {
                localArcGISElevationSourceViewState = new ArcGISElevationSourceViewState(arcGISElevationSourceViewState);
            }
            
            callback(localLayer, localArcGISElevationSourceViewState);
        }
    }
}