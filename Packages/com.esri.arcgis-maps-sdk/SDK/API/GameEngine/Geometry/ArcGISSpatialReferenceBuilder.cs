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
    /// Allows you to create and modify spatial references with custom tolerance and resolution values.
    /// </summary>
    /// <remarks>
    /// Spatial references have default precision properties when created using <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> constructors from
    /// well-known ID (WKID) numbers or a well-known text (WKT) string. Esri strongly recommends using the default
    /// precision values in most cases because they have proved to perform quite well for most situations. Learn more
    /// about <see cref="the properties of a spatial reference">https://pro.arcgis.com/en/pro-app/latest/help/data/geodatabases/overview/the-properties-of-a-spatial-reference.htm</see>.
    /// 
    /// Users with high-accuracy data capture and storage workflows may require non-default precision values. The
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> allows you to create <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> objects and customize the precision
    /// properties <see cref="GameEngine.Geometry.ArcGISSpatialReference.Tolerance">ArcGISSpatialReference.Tolerance</see> and <see cref="GameEngine.Geometry.ArcGISSpatialReference.Resolution">ArcGISSpatialReference.Resolution</see>, using the following workflow.
    /// 
    /// 1. Create the <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> by WKID, WKT string, or from an existing <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
    /// 1. Change <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> and/or <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see> as required.
    /// 1. Use <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</see> after changing properties to check the new values are valid and
    ///    consistent with each other - for example, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> must be at least twice
    ///    <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>.
    /// 1. Call <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ToSpatialReference">ArcGISSpatialReferenceBuilder.ToSpatialReference</see> to produce a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
    /// 
    /// You can now use the new custom precision <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> as required. For example:
    /// 
    /// * When creating a new geodatabase table, setting <see cref="">TableDescription.spatialReference</see> with a custom
    ///   <see cref="GameEngine.Geometry.ArcGISSpatialReference.Resolution">ArcGISSpatialReference.Resolution</see> determines the precision with which the coordinate values are stored. Note
    ///   that shapefiles and also non-Esri data sources do not support storing coordinate values with resolution, and
    ///   therefore any resolution set will be ignored in such cases.
    /// * Perform relational or topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations with a custom tolerance, by creating a
    ///   <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> with your customized <see cref="GameEngine.Geometry.ArcGISSpatialReference.Tolerance">ArcGISSpatialReference.Tolerance</see>, and passing the geometry to the relational
    ///   or topological operation - see <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> for more information.
    /// 
    /// If you are working with z-values (<see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see>, <see cref="">FeatureTable.hasZ</see>), you may wish to also consider
    /// setting <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> and/or <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>
    /// values.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISUnit">ArcGISUnit</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</seealso>
    /// <since>2.1.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISSpatialReferenceBuilder
    {
        #region Constructors
        /// <summary>
        /// Creates a spatial reference builder based on the given horizontal coordinate system.
        /// </summary>
        /// <remarks>
        /// This method creates a spatial reference builder based on the given horizontal coordinate system WKID, and
        /// will have not have a vertical coordinate system associated with it. It will have default precision values
        /// (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see>,
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>, and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see>).
        /// </remarks>
        /// <param name="WKID">The well-known ID of the horizontal coordinate system. Must be a supported well-known ID.</param>
        /// <since>2.1.0</since>
        public ArcGISSpatialReferenceBuilder(int WKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReferenceBuilder_create(WKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference builder based on the given horizontal and vertical coordinate systems.
        /// </summary>
        /// <remarks>
        /// This method creates a spatial reference builder based on the given horizontal coordinate system WKID and
        /// vertical coordinate system verticalWKID. It will have default precision values
        /// (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see>,
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>, and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see>). If
        /// verticalWKID is less than or equal to 0, this is equivalent to calling <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder.ArcGISSpatialReferenceBuilder</see>,
        /// and the builder will not have a vertical coordinate system associated with it. If verticalWKID is
        /// greater than zero but is not a supported well-known ID, an exception will be thrown.
        /// </remarks>
        /// <param name="WKID">The well-known ID of the horizontal coordinate system. Must be a supported well-known ID.</param>
        /// <param name="verticalWKID">The well-known ID of the vertical coordinate system. Must be a supported well-known ID.</param>
        /// <since>2.1.0</since>
        public ArcGISSpatialReferenceBuilder(int WKID, int verticalWKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReferenceBuilder_createVerticalWKID(WKID, verticalWKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference builder with the specified spatial reference (including any customizations) as the starting point for further modifications.
        /// </summary>
        /// <remarks>
        /// This method creates a spatial reference builder with property values that match the given
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>, including any custom precision values (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>,
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see>, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>, and
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see>).
        /// </remarks>
        /// <param name="spatialReference">The spatial reference to use as a starting point for further modifications</param>
        /// <since>2.1.0</since>
        public ArcGISSpatialReferenceBuilder(ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference.Handle;
            
            Handle = PInvoke.RT_SpatialReferenceBuilder_createFromSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference builder based on well-known text.
        /// </summary>
        /// <remarks>
        /// This method creates a spatial reference build based on the given well-known text definition of a coordinate
        /// system, and so can be used to define a builder with a customized coordinate system. The builder will have
        /// default precision values (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see>,
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>, and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see>).
        /// </remarks>
        /// <param name="wkText">A supported well-known text definition of a coordinate system.</param>
        /// <since>2.1.0</since>
        public ArcGISSpatialReferenceBuilder(string wkText)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReferenceBuilder_createFromWKText(wkText, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// True if a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> can be produced from the current builder properties, false otherwise.
        /// </summary>
        /// <remarks>
        /// A spatial reference builder is only considered valid if:
        /// 
        /// * <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see> and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see> are greater than zero.
        /// * <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> are greater than zero.
        /// * <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> is at least twice the <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>.
        /// * <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> is at least twice the <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ToSpatialReference">ArcGISSpatialReferenceBuilder.ToSpatialReference</seealso>
        /// <since>2.1.0</since>
        public bool IsValid
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getIsValid(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The minimum distance that separates unique x,y coordinate values when stored in an <see cref="">ArcGISFeatureTable</see>.
        /// </summary>
        /// <remarks>
        /// The resolution represents the detail in which a feature class records the location and shape of features,
        /// defining the number of decimal places or significant digits stored. It is the minimum distance that
        /// separates x,y coordinate values in the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> of an <see cref="">ArcGISFeature</see>. Any coordinates that differ by
        /// less than the resolution will still be stored as the same coordinate value. The units of
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see> are the units of the horizontal coordinate system
        /// (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Unit">ArcGISSpatialReferenceBuilder.Unit</see>) defined when this builder was created.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see> must be greater than zero for <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</see> to
        /// be true and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ToSpatialReference">ArcGISSpatialReferenceBuilder.ToSpatialReference</see> to produce a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> must also be at least twice <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>.
        /// 
        /// A separate resolution, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>, is defined for the distance between
        /// z-values.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Unit">ArcGISSpatialReferenceBuilder.Unit</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</seealso>
        /// <since>2.1.0</since>
        public double Resolution
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getResolution(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReferenceBuilder_setResolution(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The minimum distance that determines if two x,y coordinates are considered to be at the same location for
        /// relational and topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations.
        /// </summary>
        /// <remarks>
        /// This value is used in relational and topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations when determining whether two
        /// points are close enough, in the horizontal plane, to be considered as the same coordinate value when
        /// calculating the result. The units of <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> are the units of the horizontal
        /// coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Unit">ArcGISSpatialReferenceBuilder.Unit</see>) defined when this builder was created.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> must be greater than zero and at least twice
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see> for <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</see> to be true and
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ToSpatialReference">ArcGISSpatialReferenceBuilder.ToSpatialReference</see> to produce a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// 
        /// A separate tolerance, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see>, is defined for the difference between
        /// z-values.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Unit">ArcGISSpatialReferenceBuilder.Unit</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</seealso>
        /// <since>2.1.0</since>
        public double Tolerance
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getTolerance(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReferenceBuilder_setTolerance(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The unit of measure for the horizontal coordinate system of this spatial reference.
        /// </summary>
        /// <remarks>
        /// Also the unit of measure for <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> and
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalUnit">ArcGISSpatialReferenceBuilder.VerticalUnit</seealso>
        /// <since>2.1.0</since>
        public ArcGISUnit Unit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getUnit(Handle, errorHandler);
                
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
        /// The vertical resolution represents the detail in which a feature class records the location and shape of
        /// features, defining the number of decimal places or significant digits stored. It is the minimum distance
        /// that separates z-values in the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> of an <see cref="">ArcGISFeature</see>. Any z-values that differ by less than
        /// the vertical resolution will still be stored as the same z-value. The units of
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>  are the units of the vertical coordinate system
        /// (<see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</see>) defined when this builder was created, if one was set.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see> must be greater than zero for
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</see> to be true and <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ToSpatialReference">ArcGISSpatialReferenceBuilder.ToSpatialReference</see> to
        /// produce a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>. <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> must also be at least twice
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>.
        /// 
        /// A separate resolution, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see>, is defined for the distance between x,y
        /// coordinates.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalUnit">ArcGISSpatialReferenceBuilder.VerticalUnit</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</seealso>
        /// <since>2.1.0</since>
        public double VerticalResolution
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getVerticalResolution(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReferenceBuilder_setVerticalResolution(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The minimum distance that determines if two z-values are considered to be at the same location for
        /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations that compare z-values.
        /// </summary>
        /// <remarks>
        /// This value is used in relational and topological <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> operations when determining whether two
        /// z-values are close enough to be considered as the same value when calculating the result. This is currently
        /// only used by <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see> and when the input is a z-aware <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>; it is not
        /// used in other methods. The units of <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> are the units of the
        /// vertical coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalUnit">ArcGISSpatialReferenceBuilder.VerticalUnit</see>) defined when this builder was created,
        /// if one was set.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> must be greater than zero and at least twice
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see> for <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</see> to be true and
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.ToSpatialReference">ArcGISSpatialReferenceBuilder.ToSpatialReference</see> to produce a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// 
        /// A separate tolerance, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see>, is defined for the distance between x,y
        /// coordinates.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalUnit">ArcGISSpatialReferenceBuilder.VerticalUnit</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</seealso>
        /// <since>2.1.0</since>
        public double VerticalTolerance
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getVerticalTolerance(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReferenceBuilder_setVerticalTolerance(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The unit of measure for the vertical coordinate system of this spatial reference, or null if no vertical
        /// coordinate system is set for this builder.
        /// </summary>
        /// <remarks>
        /// Also the unit of measure for <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalTolerance">ArcGISSpatialReferenceBuilder.VerticalTolerance</see> and
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalResolution">ArcGISSpatialReferenceBuilder.VerticalResolution</see>.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Unit">ArcGISSpatialReferenceBuilder.Unit</seealso>
        /// <since>2.1.0</since>
        public ArcGISLinearUnit VerticalUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getVerticalUnit(Handle, errorHandler);
                
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
        /// The well-known ID for the vertical coordinate system (VCS), or 0 if this builder has no VCS or has a custom
        /// VCS.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.WKID">ArcGISSpatialReferenceBuilder.WKID</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalWKID">ArcGISSpatialReference.VerticalWKID</seealso>
        /// <since>2.1.0</since>
        public int VerticalWKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getVerticalWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known ID for the horizontal coordinate system, or 0 if this builder has a custom horizontal
        /// coordinate system.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.VerticalWKID">ArcGISSpatialReferenceBuilder.VerticalWKID</seealso>
        /// <since>2.1.0</since>
        public int WKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known text definition of the horizontal and vertical coordinate systems of this builder.
        /// </summary>
        /// <remarks>
        /// If this coordinate system can only be represented in WKT2 format this property returns an empty string.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.WKText">ArcGISSpatialReference.WKText</seealso>
        /// <since>2.1.0</since>
        public string WKText
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getWKText(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The well-known text for the horizontal and vertical coordinate systems of this builder as defined by the OGC standard.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.WKText">ArcGISSpatialReferenceBuilder.WKText</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.WKText2">ArcGISSpatialReference.WKText2</seealso>
        /// <since>2.3.0</since>
        public string WKText2
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReferenceBuilder_getWKText2(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Returns a new <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> based on the current values of this builder.
        /// </summary>
        /// <remarks>
        /// Use <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</see> after changing precision properties on this builder to check all the
        /// values are valid and consistent with each other - for example, <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Tolerance">ArcGISSpatialReferenceBuilder.Tolerance</see> must be
        /// at least twice <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.Resolution">ArcGISSpatialReferenceBuilder.Resolution</see> for this method to return a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </remarks>
        /// <returns>
        /// A new spatial reference with the current coordinate system and precision values of this builder.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder.IsValid">ArcGISSpatialReferenceBuilder.IsValid</seealso>
        /// <since>2.1.0</since>
        public ArcGISSpatialReference ToSpatialReference()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReferenceBuilder_toSpatialReference(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSpatialReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISSpatialReference(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISSpatialReferenceBuilder(IntPtr handle) => Handle = handle;
        
        ~ArcGISSpatialReferenceBuilder()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReferenceBuilder_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISSpatialReferenceBuilder other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_create(int WKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_createVerticalWKID(int WKID, int verticalWKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_createFromSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_createFromWKText([MarshalAs(UnmanagedType.LPStr)]string wkText, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReferenceBuilder_getIsValid(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReferenceBuilder_getResolution(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReferenceBuilder_setResolution(IntPtr handle, double resolution, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReferenceBuilder_getTolerance(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReferenceBuilder_setTolerance(IntPtr handle, double tolerance, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_getUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReferenceBuilder_getVerticalResolution(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReferenceBuilder_setVerticalResolution(IntPtr handle, double verticalResolution, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReferenceBuilder_getVerticalTolerance(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReferenceBuilder_setVerticalTolerance(IntPtr handle, double verticalTolerance, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_getVerticalUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_SpatialReferenceBuilder_getVerticalWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_SpatialReferenceBuilder_getWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_getWKText(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_getWKText2(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReferenceBuilder_toSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReferenceBuilder_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}