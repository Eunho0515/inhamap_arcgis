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

namespace Esri.Unity
{
    /// <summary>
    /// A callback invoked when an element has been added to the vector.
    /// </summary>
    /// <since>1.0.0</since>
    public delegate void ArcGISMutableArrayElementAddedEvent<T>(ulong position, T element);
    
    internal delegate void ArcGISMutableArrayElementAddedEventInternal(IntPtr userData, UIntPtr position, IntPtr element);
    
    internal class ArcGISMutableArrayElementAddedEventHandler<T> : ArcGISEventHandler<ArcGISMutableArrayElementAddedEvent<T>>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISMutableArrayElementAddedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, UIntPtr position, IntPtr element)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISMutableArrayElementAddedEventHandler<T>)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            Standard.ArcGISElement localElement = null;
            
            if (element != IntPtr.Zero)
            {
                localElement = new Standard.ArcGISElement(element);
            }
            
            callback(position.ToUInt64(), Unity.Convert.FromArcGISElement<T>(localElement));
        }
    }
}