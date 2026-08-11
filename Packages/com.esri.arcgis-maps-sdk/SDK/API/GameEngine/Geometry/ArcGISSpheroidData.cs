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

namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// The spheroid data for an <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> - <see cref="GameEngine.Geometry.ArcGISSpatialReference.SpheroidData">ArcGISSpatialReference.SpheroidData</see>.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public struct ArcGISSpheroidData
    {
        /// <summary>
        /// The major semi axis (a) of the spheroid, in meters.
        /// </summary>
        public double MajorSemiAxis;
        
        /// <summary>
        /// The spheroid's squared eccentricity, equal to (1 - c^2/a^2)
        /// </summary>
        public double SquaredEccentricity;
        
        /// <summary>
        /// The minor semi axis (c) of the spheroid, in meters.
        /// </summary>
        public double MinorSemiAxis;
        
        /// <summary>
        /// The spheroid's flattening, equal to ((a - c) / a)
        /// </summary>
        public double Flattening;
    }
}