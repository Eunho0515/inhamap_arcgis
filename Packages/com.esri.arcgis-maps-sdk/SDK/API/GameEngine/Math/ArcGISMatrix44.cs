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
    /// A four-by-four matrix made of floats.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISMatrix44
    {
        /// <summary>
        /// The m00 parameter.
        /// </summary>
        public float M00;
        
        /// <summary>
        /// The m01 parameter.
        /// </summary>
        public float M01;
        
        /// <summary>
        /// The m02 parameter.
        /// </summary>
        public float M02;
        
        /// <summary>
        /// The m03 parameter.
        /// </summary>
        public float M03;
        
        /// <summary>
        /// The m10 parameter.
        /// </summary>
        public float M10;
        
        /// <summary>
        /// The m11 parameter.
        /// </summary>
        public float M11;
        
        /// <summary>
        /// The m12 parameter.
        /// </summary>
        public float M12;
        
        /// <summary>
        /// The m13 parameter.
        /// </summary>
        public float M13;
        
        /// <summary>
        /// The m20 parameter.
        /// </summary>
        public float M20;
        
        /// <summary>
        /// The m21 parameter.
        /// </summary>
        public float M21;
        
        /// <summary>
        /// The m22 parameter.
        /// </summary>
        public float M22;
        
        /// <summary>
        /// The m23 parameter.
        /// </summary>
        public float M23;
        
        /// <summary>
        /// The m30 parameter.
        /// </summary>
        public float M30;
        
        /// <summary>
        /// The m31 parameter.
        /// </summary>
        public float M31;
        
        /// <summary>
        /// The m32 parameter.
        /// </summary>
        public float M32;
        
        /// <summary>
        /// The m33 parameter.
        /// </summary>
        public float M33;
    }
}