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
    /// Oriented bounding box. Contains a position vector, extents vector describing the box dimensions along local axes, and a rotation quaternion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISOrientedBoundingBox
    {
        /// <summary>
        /// X coordinate of the center of the oriented bounding box.
        /// </summary>
        public double CenterX;
        
        /// <summary>
        /// Y coordinate of the center of the oriented bounding box.
        /// </summary>
        public double CenterY;
        
        /// <summary>
        /// Z coordinate of the center of the oriented bounding box.
        /// </summary>
        public double CenterZ;
        
        /// <summary>
        /// Half-size of the oriented bounding box along the local X axis.
        /// </summary>
        public float ExtentX;
        
        /// <summary>
        /// Half-size of the oriented bounding box along the local Y axis.
        /// </summary>
        public float ExtentY;
        
        /// <summary>
        /// Half-size of the oriented bounding box along the local Z axis.
        /// </summary>
        public float ExtentZ;
        
        /// <summary>
        /// Orientation of the box - quaternion component X.
        /// </summary>
        public double OrientationX;
        
        /// <summary>
        /// Orientation of the box - quaternion component Y.
        /// </summary>
        public double OrientationY;
        
        /// <summary>
        /// Orientation of the box - quaternion component Z.
        /// </summary>
        public double OrientationZ;
        
        /// <summary>
        /// Orientation of the box - quaternion component W.
        /// </summary>
        public double OrientationW;
    }
}