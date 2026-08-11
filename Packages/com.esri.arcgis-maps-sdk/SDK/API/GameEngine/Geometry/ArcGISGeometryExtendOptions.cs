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
namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// Flags for the type of extend operation to perform.
    /// </summary>
    /// <since>1.0.0</since>
    [System.Flags]
    public enum ArcGISGeometryExtendOptions
    {
        /// <summary>
        /// By default, extension considers both ends of paths. The old ends
        /// remain and new points are added at the extended ends. The new points
        /// have attributes that are extrapolated from adjacent existing
        /// segments.
        /// </summary>
        /// <since>1.0.0</since>
        Default = 0,
        
        /// <summary>
        /// If an extension is performed at an end, relocate the end point to
        /// the new position instead of leaving the old point and adding a new
        /// point at the new position.
        /// </summary>
        /// <since>1.0.0</since>
        RelocateEnds = 1,
        
        /// <summary>
        /// If an extension is performed at an end, do not extrapolate the
        /// end-segment's attributes for the new point. Instead, make its
        /// attributes the same as the current end. Incompatible with
        /// <see cref="GameEngine.Geometry.ArcGISGeometryExtendOptions.NoEndAttributes">ArcGISGeometryExtendOptions.NoEndAttributes</see>.
        /// </summary>
        /// <since>1.0.0</since>
        KeepEndAttributes = 2,
        
        /// <summary>
        /// If an extension is performed at an end, do not extrapolate the
        /// end-segment's attributes for the new point. Instead, make its
        /// attributes be empty. Incompatible with
        /// <see cref="GameEngine.Geometry.ArcGISGeometryExtendOptions.KeepEndAttributes">ArcGISGeometryExtendOptions.KeepEndAttributes</see>.
        /// </summary>
        /// <since>1.0.0</since>
        NoEndAttributes = 4,
        
        /// <summary>
        /// Do not extend the 'from' end of any path.
        /// </summary>
        /// <since>1.0.0</since>
        DoNotExtendFromStartPoint = 8,
        
        /// <summary>
        /// Do not extend the 'to' end of any path.
        /// </summary>
        /// <since>1.0.0</since>
        DoNotExtendFromEndPoint = 16
    };
}