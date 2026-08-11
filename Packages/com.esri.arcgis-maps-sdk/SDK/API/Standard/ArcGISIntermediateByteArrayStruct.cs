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

namespace Esri.Standard
{
    /// <summary>
    /// An array of bytes.
    /// </summary>
    /// <remarks>
    /// Use this to pass an array of bytes.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISIntermediateByteArrayStruct
    {
        /// <summary>
        /// The pointer to the allocated memory.
        /// </summary>
        public IntPtr Data;
        
        /// <summary>
        /// The size of the array.
        /// </summary>
        public ulong Size;
    }
}