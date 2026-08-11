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

namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// The spatial reference specifies how geometry coordinates relate to real-world space.
    /// </summary>
    /// <remarks>
    /// Instances of this class represent a specific coordinate system identified by a well-known ID (WKID) number
    /// or well-known text (WKT) string. There are two broad classes of coordinate systems:
    /// * Geographic coordinate systems use a 3-dimensional spherical surface to define locations on the earth.
    /// * Projected coordinate systems use a flat, 2-dimensional surface.
    /// 
    /// See <see cref="">https://developers.arcgis.com/documentation/spatial-references/</see> for more information about spatial references.
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> ensures that you can accurately view, query, and analyze the layers of a <see cref="">GeoModel</see>.
    /// 
    /// The spatial reference value is available from a map or scene after loading has completed, and is immutable.
    /// If you want to set this value for a new map or scene, use the <see cref="">Map.Map(SpatialReference)</see> or
    /// <see cref="">Scene.Scene(SceneViewTilingScheme)</see> constructors.
    /// 
    /// This API supports many spatial reference WKIDs, which are documented in the Coordinate Systems and
    /// Transformations Tables available in the <see cref="downloads area of developers.arcgis.com">https://developers.arcgis.com/downloads</see>,
    /// under the relevant SDK.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISSpatialReference :
        GameEngine.Io.ArcGISJSONSerializable<ArcGISSpatialReference>
    {
        #region Constructors
        /// <summary>
        /// Creates a spatial reference based on WKID.
        /// </summary>
        /// <remarks>
        /// The method will create a spatial reference that has only horizontal coordinate system and does not have vertical
        /// coordinate system associated with it.
        /// </remarks>
        /// <param name="WKID">The well-known ID of the horizontal coordinate system. Must be a supported well-known ID.</param>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference(int WKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReference_create(WKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference based on WKID for the horizontal coordinate system and vertical coordinate system.
        /// </summary>
        /// <remarks>
        /// The method creates a spatial reference that has both horizontal and vertical coordinate systems.
        /// When the vertical WKID is less than or equal to 0, this is equivalent to calling <see cref="GameEngine.Geometry.ArcGISSpatialReference.ArcGISSpatialReference">ArcGISSpatialReference.ArcGISSpatialReference</see>,
        /// and the spatial reference will not have a vertical coordinate system associated with it.
        /// If verticalWKID is greater than zero but is not a supported well-known ID, an exception will be thrown.
        /// </remarks>
        /// <param name="WKID">The well-known ID of the horizontal coordinate system. Must be a supported well-known ID.</param>
        /// <param name="verticalWKID">The well-known ID of the vertical  coordinate system. Must be a supported well-known ID.</param>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference(int WKID, int verticalWKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReference_createVerticalWKID(WKID, verticalWKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference based on well-known text.
        /// </summary>
        /// <param name="wkText">The well-known text of the spatial reference to create.</param>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference(string wkText)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReference_createFromWKText(wkText, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// If the given spatial reference is a projected coordinate system, then this returns the geographic coordinate system of that system.
        /// </summary>
        /// <remarks>
        /// If the spatial reference is a projected coordinate system, then a spatial reference object representing the underlying geographic
        /// coordinate system is returned. Every projected coordinate system has an underlying geographic coordinate system. If the
        /// spatial reference is a geographic coordinate system, then a reference to itself is returned.
        /// If the spatial reference is a local spatial reference, a null is returned with an error.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference BaseGeographic
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getBaseGeographic(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The full world extent for the spatial reference.
        /// </summary>
        /// <remarks>
        /// The envelope defines the valid range of coordinates for the spatial reference.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</seealso>
        /// <since>1.0.0</since>
        public ArcGISEnvelope FullWorldExtent
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getFullWorldExtent(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISEnvelope localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISEnvelope(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// True if spatial reference has a vertical coordinate system set; false otherwise.
        /// </summary>
        /// <remarks>
        /// A spatial reference can optionally include a definition for a vertical coordinate system (VCS), which can be used to interpret the z-values of a geometry. A VCS defines linear units of measure, the origin of z-values, and whether z-values are 'positive up' (representing heights above a surface) or 'positive down' (indicating that values are depths below a surface). 
        /// 
        /// An <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> may have a VCS set, for example, by calling the <see cref="GameEngine.Geometry.ArcGISSpatialReference.ArcGISSpatialReference">ArcGISSpatialReference.ArcGISSpatialReference</see> constructor. <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalWKID">ArcGISSpatialReference.VerticalWKID</see>, <see cref="GameEngine.Geometry.ArcGISSpatialReference.WKText">ArcGISSpatialReference.WKText</see>, and <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</see> provide more information about the specific VCS set on this instance. 
        /// 
        /// VCSs are used when projecting geometries using an <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see>.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalWKID">ArcGISSpatialReference.VerticalWKID</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</seealso>
        /// <since>1.0.0</since>
        public bool HasVertical
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getHasVertical(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if spatial reference is a Geographic Coordinate System.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsGeographic
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getIsGeographic(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if coordinate system is horizontally pannable.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsPannable
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getIsPannable(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if spatial reference is a Projected Coordinate System.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsProjected
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getIsProjected(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The minimum distance that separates unique x,y coordinate values when stored in an <see cref="">ArcGISFeatureTable</see>.
        /// </summary>
        /// <remarks>
        /// The resolution represents the detail in which a feature class records the location and shape of features,
        /// defining the number of decimal places stored. It is the minimum distance that
        /// separates x,y coordinate values in the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> of an <see cref="">ArcGISFeature</see>. Any coordinates that differ by
        /// less than the resolution will be stored as the same coordinate value. The units of
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.Resolution">ArcGISSpatialReference.Resolution</see> are the units of the horizontal coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReference.Unit">ArcGISSpatialReference.Unit</see>)
        /// defined when this spatial reference was created.
        /// 
        /// A separate resolution, <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalResolution">ArcGISSpatialReference.VerticalResolution</see>, is defined for the distance between
        /// z-values.
        /// 
        /// Spatial references have default precision properties, and Esri strongly recommends using the default values
        /// in most cases. However some high-accuracy workflows customize the default values to preserve their data with
        /// higher precision. To store coordinates using a non-default resolution, create a new dataset that uses a
        /// custom resolution spatial reference - for example by setting <see cref="">TableDescription.spatialReference</see> when
        /// creating a new geodatabase table. See <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> for more information about creating spatial
        /// references with custom precision values.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</seealso>
        /// <seealso cref="The properties of a spatial reference">https://pro.arcgis.com/en/pro-app/latest/help/data/geodatabases/overview/the-properties-of-a-spatial-reference.htm</seealso>
        /// <since>2.1.0</since>
        public double Resolution
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getResolution(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The spheroid data for the spatial reference.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISSpheroidData SpheroidData
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getSpheroidData(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The minimum distance that determines if two x,y coordinates are considered to be at the same location for
        /// relational and topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations.
        /// </summary>
        /// <remarks>
        /// This value is used in relational and topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations when determining whether two
        /// points are close enough, in the horizontal plane, to be considered as the same coordinate value when
        /// calculating the result. The units of <see cref="GameEngine.Geometry.ArcGISSpatialReference.Tolerance">ArcGISSpatialReference.Tolerance</see> are the units of the horizontal
        /// coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReference.Unit">ArcGISSpatialReference.Unit</see>) defined when this spatial reference was created.
        /// 
        /// A separate tolerance, <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalTolerance">ArcGISSpatialReference.VerticalTolerance</see>, is defined for the difference between
        /// z-values.
        /// 
        /// Spatial references have default precision properties, and Esri strongly recommends using the default values
        /// in most cases. However some high-accuracy workflows customize the default values to work with a higher
        /// precision. To perform operations using a non-default tolerance, create a <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> with your custom
        /// tolerance spatial reference, and pass that geometry to the relational or topological operation. See
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> for more information about creating spatial references with custom precision
        /// values.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</seealso>
        /// <seealso cref="The properties of a spatial reference">https://pro.arcgis.com/en/pro-app/latest/help/data/geodatabases/overview/the-properties-of-a-spatial-reference.htm</seealso>
        /// <since>2.1.0</since>
        public double Tolerance
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getTolerance(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The unit of measure for the horizontal coordinate system of this spatial reference.
        /// </summary>
        /// <remarks>
        /// Also the unit of measure for <see cref="GameEngine.Geometry.ArcGISSpatialReference.Tolerance">ArcGISSpatialReference.Tolerance</see> and <see cref="GameEngine.Geometry.ArcGISSpatialReference.Resolution">ArcGISSpatialReference.Resolution</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISUnit Unit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    var objectType = GameEngine.Geometry.PInvoke.RT_Unit_getObjectType(localResult, IntPtr.Zero);
                    
                    switch (objectType)
                    {
                        case GameEngine.Geometry.ArcGISUnitType.AngularUnit:
                            localLocalResult = new ArcGISAngularUnit(localResult);
                            break;
                        case GameEngine.Geometry.ArcGISUnitType.AreaUnit:
                            localLocalResult = new ArcGISAreaUnit(localResult);
                            break;
                        case GameEngine.Geometry.ArcGISUnitType.LinearUnit:
                            localLocalResult = new ArcGISLinearUnit(localResult);
                            break;
                        default:
                            localLocalResult = new ArcGISUnit(localResult);
                            break;
                    }
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The minimum distance that separates unique z-values when stored in an <see cref="">ArcGISFeatureTable</see>.
        /// </summary>
        /// <remarks>
        /// The vertical resolution represents the detail in which a feature class records the location and shape of features,
        /// defining the number of decimal places or significant digits stored. It is the minimum distance that
        /// separates z-values in the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> of an <see cref="">ArcGISFeature</see>. Any z-values that differ by less than the
        /// vertical resolution will be stored as the same z-value. The units of <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalResolution">ArcGISSpatialReference.VerticalResolution</see>
        /// are the units of the vertical coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</see>) defined when this
        /// spatial reference was created, if one was set.
        /// 
        /// A separate resolution, <see cref="GameEngine.Geometry.ArcGISSpatialReference.Resolution">ArcGISSpatialReference.Resolution</see>, is defined for the distance between x,y coordinates.
        /// 
        /// Spatial references have default precision properties, and Esri strongly recommends using the default values
        /// in most cases. However some high-accuracy workflows customize the default values to preserve their data with
        /// higher precision. To store coordinates using a non-default resolution, create a new dataset that uses a
        /// custom resolution spatial reference - for example by setting <see cref="">TableDescription.spatialReference</see> when
        /// creating a new geodatabase table. See <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> for more information about creating spatial
        /// references with custom precision values.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</seealso>
        /// <seealso cref="The properties of a spatial reference">https://pro.arcgis.com/en/pro-app/latest/help/data/geodatabases/overview/the-properties-of-a-spatial-reference.htm</seealso>
        /// <since>2.1.0</since>
        public double VerticalResolution
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getVerticalResolution(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The minimum distance that determines if two z-values are considered to be at the same location for
        /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations that compare z-values.
        /// </summary>
        /// <remarks>
        /// This value is used in relational and topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations when determining whether two
        /// z-values are close enough to be considered as the same value when calculating the result. The units of
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalTolerance">ArcGISSpatialReference.VerticalTolerance</see> are the units of the vertical coordinate system
        /// (<see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</see>) defined when this spatial reference was created, if one was set.
        /// 
        /// Note that <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalTolerance">ArcGISSpatialReference.VerticalTolerance</see> is currently only used by
        /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see> and when the input is a z-aware <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>; it is not used in other
        /// methods.
        /// 
        /// A separate tolerance, <see cref="GameEngine.Geometry.ArcGISSpatialReference.Tolerance">ArcGISSpatialReference.Tolerance</see>, is defined for the difference between x,y coordinates.
        /// 
        /// Spatial references have default precision properties, and Esri strongly recommends using the default values
        /// in most cases. However some high-accuracy workflows customize the default values to work with a higher
        /// precision. To perform operations using a non-default tolerance, create a <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> with your custom
        /// tolerance spatial reference, and pass that geometry to the operation. See <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> for
        /// more information about creating spatial references with custom precision values.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</seealso>
        /// <seealso cref="The properties of a spatial reference">https://pro.arcgis.com/en/pro-app/latest/help/data/geodatabases/overview/the-properties-of-a-spatial-reference.htm</seealso>
        /// <since>2.1.0</since>
        public double VerticalTolerance
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getVerticalTolerance(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The unit of measure for the vertical coordinate system of this spatial reference.
        /// </summary>
        /// <remarks>
        /// Is null if <see cref="GameEngine.Geometry.ArcGISSpatialReference.HasVertical">ArcGISSpatialReference.HasVertical</see> is false.
        /// 
        /// Also the unit of measure for <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalTolerance">ArcGISSpatialReference.VerticalTolerance</see> and
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalResolution">ArcGISSpatialReference.VerticalResolution</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit VerticalUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getVerticalUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISLinearUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISLinearUnit(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The well-known ID for the vertical coordinate system (VCS), or 0 if the spatial reference
        /// has no VCS or has a custom VCS.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.ArcGISSpatialReference">ArcGISSpatialReference.ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public int VerticalWKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getVerticalWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known ID for the horizontal coordinate system, or 0 if the spatial reference
        /// has a custom horizontal coordinate system.
        /// </summary>
        /// <since>1.0.0</since>
        public int WKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known text for the horizontal and vertical coordinate systems.
        /// </summary>
        /// <remarks>
        /// If this coordinate system can only be represented in WKT2 format this property returns an empty string.
        /// </remarks>
        /// <since>1.0.0</since>
        public string WKText
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getWKText(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The well-known text for the horizontal and vertical coordinate systems as defined by the OGC standard.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.WKText">ArcGISSpatialReference.WKText</seealso>
        /// <since>2.3.0</since>
        public string WKText2
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getWKText2(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Calculate the grid convergence for a spatial reference at a given point.
        /// </summary>
        /// <remarks>
        /// The grid convergence is the angle between True North and Grid North
        /// at a point on a map. The grid convergence can be used to convert a
        /// horizontal direction expressed as an azimuth in a geographic
        /// coordinate system (relative to True North) to a direction expressed
        /// as a bearing in a projected coordinate system (relative to Grid
        /// North), and vice versa.
        /// 
        /// Sign convention
        /// 
        /// The grid convergence returned by this method is positive when Grid
        /// North lies east of True North. The following formula demonstrates
        /// how to obtain a bearing (b) from an azimuth (a) using the grid
        /// convergence (c) returned by this method:
        /// 
        /// b = a - c
        /// 
        /// This sign convention is sometimes named the Gauss-Bomford convention.
        /// 
        /// Other notes:
        /// * Returns 0 if the spatial reference is a geographic coordinate system
        /// * Returns NAN if the point is outside the projection's horizon or on error
        /// * If the point has no spatial reference, it is assumed to be in the
        ///   given spatial reference
        /// * If the point's spatial reference differs from the spatial
        ///   reference given, its location is transformed automatically to
        ///   the given spatial reference
        /// </remarks>
        /// <param name="point">The point.</param>
        /// <returns>
        /// The grid convergence in degrees.
        /// </returns>
        /// <since>1.0.0</since>
        public double GetConvergenceAngle(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_SpatialReference_getConvergenceAngle(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Creates a spatial reference based on web Mercator.
        /// </summary>
        /// <remarks>
        /// The method creates a web Mercator spatial reference.
        /// </remarks>
        /// <returns>
        /// A spatial reference. This is passed to spatial reference functions.
        /// null if an error occurs.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISSpatialReference WebMercator()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_webMercator(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSpatialReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISSpatialReference(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a spatial reference based on WGS84.
        /// </summary>
        /// <remarks>
        /// The method creates a WGS84 spatial reference.
        /// </remarks>
        /// <returns>
        /// A spatial reference. This is passed to spatial reference functions.
        /// null if an error occurs.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISSpatialReference WGS84()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_WGS84(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSpatialReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISSpatialReference(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISSpatialReference>
        public static ArcGISSpatialReference FromJSON(string JSON)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_fromJSON(JSON, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSpatialReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISSpatialReference(localResult);
            }
            
            return localLocalResult;
        }
        
        public string ToJSON()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_toJSON(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISSpatialReference>
        
        #region Internal Members
        internal ArcGISSpatialReference(IntPtr handle) => Handle = handle;
        
        ~ArcGISSpatialReference()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReference_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            var right = obj as ArcGISSpatialReference;
            
            if (right == null)
            {
                return false;
            }
            
            var localRight = right.Handle;
            
            if (Handle == localRight)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_equals(Handle, localRight, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public override int GetHashCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_getHash(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return (int)localResult.ToUInt64();
        }
        
        public static implicit operator bool(ArcGISSpatialReference other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_create(int WKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_createVerticalWKID(int WKID, int verticalWKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_createFromWKText([MarshalAs(UnmanagedType.LPStr)]string wkText, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getBaseGeographic(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getFullWorldExtent(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getHasVertical(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getIsGeographic(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getIsPannable(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getIsProjected(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReference_getResolution(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISSpheroidData RT_SpatialReference_getSpheroidData(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReference_getTolerance(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReference_getVerticalResolution(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReference_getVerticalTolerance(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getVerticalUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_SpatialReference_getVerticalWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_SpatialReference_getWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getWKText(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getWKText2(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReference_getConvergenceAngle(IntPtr handle, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_webMercator(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_WGS84(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReference_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_equals(IntPtr handle, IntPtr right, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_SpatialReference_getHash(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISSpatialReference> P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_fromJSON([MarshalAs(UnmanagedType.LPStr)]string JSON, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_toJSON(IntPtr handle, IntPtr errorHandler);
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISSpatialReference> P-Invoke Declarations
    }
}