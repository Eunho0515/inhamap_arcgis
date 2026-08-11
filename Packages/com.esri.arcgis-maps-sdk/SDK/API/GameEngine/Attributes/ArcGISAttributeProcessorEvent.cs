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

namespace Esri.GameEngine.Attributes
{
    /// <summary>
    /// A callback invoked when attributes are available to be processed.
    /// </summary>
    /// <since>1.0.0</since>
    public delegate void ArcGISAttributeProcessorEvent(Unity.ArcGISImmutableArray<ArcGISAttribute> layerAttributes, Unity.ArcGISImmutableArray<ArcGISVisualizationAttribute> visualizationAttributes);
    
    internal delegate void ArcGISAttributeProcessorEventInternal(IntPtr userData, IntPtr layerAttributes, IntPtr visualizationAttributes);
    
    internal class ArcGISAttributeProcessorEventHandler : Unity.ArcGISEventHandler<ArcGISAttributeProcessorEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISAttributeProcessorEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr layerAttributes, IntPtr visualizationAttributes)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISAttributeProcessorEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            Unity.ArcGISImmutableArray<ArcGISAttribute> localLayerAttributes = null;
            
            if (layerAttributes != IntPtr.Zero)
            {
                localLayerAttributes = new Unity.ArcGISImmutableArray<ArcGISAttribute>(layerAttributes);
            }
            
            Unity.ArcGISImmutableArray<ArcGISVisualizationAttribute> localVisualizationAttributes = null;
            
            if (visualizationAttributes != IntPtr.Zero)
            {
                localVisualizationAttributes = new Unity.ArcGISImmutableArray<ArcGISVisualizationAttribute>(visualizationAttributes);
            }
            
            callback(localLayerAttributes, localVisualizationAttributes);
        }
    }
}