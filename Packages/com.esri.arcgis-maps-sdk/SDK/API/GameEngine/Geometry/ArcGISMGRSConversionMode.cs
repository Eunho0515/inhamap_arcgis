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
    /// Determines the lettering scheme and treatment of coordinates
    /// at 180 degrees longitude when converting MGRS coordinates.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISMGRSConversionMode
    {
        /// <summary>
        /// The choice of MGRS lettering scheme is based on the datum and
        /// ellipsoid of the spatial reference provided. Spatial references with
        /// new datums (e.g. WGS 84) assume new lettering scheme (AA
        /// scheme). This is equivalent to
        /// <see cref="GameEngine.Geometry.ArcGISMGRSConversionMode.New180InZone60">ArcGISMGRSConversionMode.New180InZone60</see>. Spatial references with older
        /// datums (e.g. Clarke 1866, Bessel 1841, Clarke 1880) assume old
        /// lettering scheme (AL scheme). This is equivalent to
        /// <see cref="GameEngine.Geometry.ArcGISMGRSConversionMode.Old180InZone60">ArcGISMGRSConversionMode.Old180InZone60</see>.  When converted, points with
        /// longitude of exactly 180deg are placed in zone 60.
        /// </summary>
        /// <since>1.0.0</since>
        Automatic = 0,
        
        /// <summary>
        /// The MGRS notation uses the new lettering scheme (AA scheme) and,
        /// when converted, places points with longitude of 180deg in zone 01.
        /// </summary>
        /// <since>1.0.0</since>
        New180InZone01 = 1,
        
        /// <summary>
        /// The MGRS notation uses the new lettering scheme (AA scheme) and,
        /// when converted, places points with longitude of 180deg in zone 60.
        /// </summary>
        /// <since>1.0.0</since>
        New180InZone60 = 2,
        
        /// <summary>
        /// The MGRS notation uses the old lettering scheme (AL scheme) and,
        /// when converted, places points with longitude of 180deg in zone 01.
        /// </summary>
        /// <since>1.0.0</since>
        Old180InZone01 = 3,
        
        /// <summary>
        /// The MGRS notation uses the old lettering scheme (AL scheme) and,
        /// when converted, places points with longitude of 180deg in zone 60.
        /// </summary>
        /// <since>1.0.0</since>
        Old180InZone60 = 4
    };
}