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

namespace Esri.GameEngine.Math
{
    /// <summary>
    /// A four-dimensional vector made of floats.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISVector4
    {
        /// <summary>
        /// The x parameter.
        /// </summary>
        public float X;
        
        /// <summary>
        /// The y parameter.
        /// </summary>
        public float Y;
        
        /// <summary>
        /// The z parameter.
        /// </summary>
        public float Z;
        
        /// <summary>
        /// The w parameter.
        /// </summary>
        public float W;
    }
}