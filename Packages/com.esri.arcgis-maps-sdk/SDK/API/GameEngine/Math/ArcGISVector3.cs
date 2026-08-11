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
    /// A three-dimensional vector of doubles.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public struct ArcGISVector3
    {
        /// <summary>
        /// The x parameter.
        /// </summary>
        public double X;
        
        /// <summary>
        /// The y parameter.
        /// </summary>
        public double Y;
        
        /// <summary>
        /// The z parameter.
        /// </summary>
        public double Z;
    }
}