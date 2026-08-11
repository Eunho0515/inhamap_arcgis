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
    /// Represents a step in the process of transforming between datums.
    /// </summary>
    /// <remarks>
    /// Each geographic transformation step can be constructed from a well-known ID (WKID) that represents a geographic
    /// transformation. This API supports many transformation WKIDs, which are documented in the Coordinate Systems and
    /// Transformations Tables available in the <see cref="downloads area of developers.arcgis.com">https://developers.arcgis.com/downloads</see>,
    /// under the relevant SDK.
    /// 
    /// The list of supported WKIDs includes a transformation from every supported
    /// datum to WGS 1984. Additionally, there is more limited list of
    /// transformations directly between two non-WGS84 datums, such as
    /// 4461, which is NAD_1983_HARN_To_NAD_1983_NSRS2007_1.
    /// 
    /// Transformations with more than one step typically go via WGS84, with one
    /// forward and one inverse geographic transformation chained together to get the
    /// required geographic coordinates.
    /// 
    /// A geographic transformation step object is immutable.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeographicTransformationStep
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> instance from a well-known ID.
        /// </summary>
        /// <remarks>
        /// Occasionally, WKIDs may change, and older codes may be deprecated in favor of a new code. Both old
        /// (deprecated) and new (latest) WKIDs continue to work for instantiation, as long as they are supported
        /// by the Projection Engine. The <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.WKID">ArcGISGeographicTransformationStep.WKID</see> property returns the new (latest)
        /// WKID code.
        /// </remarks>
        /// <param name="WKID">The well-known ID of the geographic transformation step to create.</param>
        /// <since>1.0.0</since>
        public ArcGISGeographicTransformationStep(int WKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GeographicTransformationStep_createWithWKID(WKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> instance from a well-known text string.
        /// </summary>
        /// <param name="WKText">The well-known text of the geographic transformation step to create.</param>
        /// <since>1.0.0</since>
        public ArcGISGeographicTransformationStep(string WKText)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GeographicTransformationStep_createWithWKText(WKText, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// True if this geographic transformation step instance is an inverted transformation.
        /// </summary>
        /// <remarks>
        /// Transformations have a specific direction indicated by the <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.WKText">ArcGISGeographicTransformationStep.WKText</see> value. An inverted transformation transforms geometries in the opposite direction of the one indicated by the well-known text. <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> has <see cref="GameEngine.Geometry.ArcGISDatumTransformation.InputSpatialReference">ArcGISDatumTransformation.InputSpatialReference</see> and <see cref="GameEngine.Geometry.ArcGISDatumTransformation.OutputSpatialReference">ArcGISDatumTransformation.OutputSpatialReference</see> properties that respect the inverse value of the contained transformation(s). 
        /// 
        /// This API supports many transformation WKIDs, including transformations from every supported datum to the World Geodetic System 1984 (WGS84) datum. To transform coordinates between two non-WGS84 datums, one forward and one inverse <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> are usually added to an <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> to get the required inputs and outputs.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.GetInverse">ArcGISGeographicTransformationStep.GetInverse</seealso>
        /// <since>1.0.0</since>
        public bool IsInverse
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeographicTransformationStep_getIsInverse(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if any files needed by the Projection Engine for this transformation step are missing from the local
        /// file system. <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.ProjectionEngineFilenames">ArcGISGeographicTransformationStep.ProjectionEngineFilenames</see> indicates the required
        /// files.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsMissingProjectionEngineFiles
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeographicTransformationStep_getIsMissingProjectionEngineFiles(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// A list of the Projection Engine files required to support this geographic transformation step.
        /// </summary>
        /// <remarks>
        /// Datum transformations can be mathematically defined (equation-based transformations), or may rely on
        /// external supporting files (grid-based transformations). For a grid-based transformation step, this returns
        /// a list of the files required to use this transformation, prefixed by an expected subpath of the file(s)
        /// underneath <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</see>. For an equation-based transformation, this
        /// returns an empty list.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.IsMissingProjectionEngineFiles">ArcGISGeographicTransformationStep.IsMissingProjectionEngineFiles</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</seealso>
        /// <since>1.0.0</since>
        public Unity.ArcGISImmutableArray<string> ProjectionEngineFilenames
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeographicTransformationStep_getProjectionEngineFilenames(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableArray<string> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableArray<string>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The well-known ID, or 0 if the transformation in this step does not have a well-known ID.
        /// </summary>
        /// <remarks>
        /// Occasionally, WKIDs may change, and an older code may be deprecated in favor of a new code. This property
        /// returns the new (latest) WKID code.
        /// </remarks>
        /// <since>1.0.0</since>
        public int WKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeographicTransformationStep_getWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known text of this geographic transformation step instance.
        /// </summary>
        /// <remarks>
        /// If this transformation step can only be represented in WKT2 format this property returns an empty string.
        /// 
        /// This value does not respect the <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.IsInverse">ArcGISGeographicTransformationStep.IsInverse</see> property.
        /// </remarks>
        /// <since>1.0.0</since>
        public string WKText
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeographicTransformationStep_getWKText(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The well-known text of this geographic transformation step instance as defined by the OGC standard.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.WKText">ArcGISGeographicTransformationStep.WKText</seealso>
        /// <since>2.3.0</since>
        public string WKText2
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeographicTransformationStep_getWKText2(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Returns the inverse of this geographic transformation step or null if the transformation is not invertible.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep">ArcGISGeographicTransformationStep</see> representing the inverse or null if an inverse does not exist.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISGeographicTransformationStep GetInverse()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GeographicTransformationStep_getInverse(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeographicTransformationStep localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISGeographicTransformationStep(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISGeographicTransformationStep(IntPtr handle) => Handle = handle;
        
        ~ArcGISGeographicTransformationStep()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeographicTransformationStep_destroy(Handle, errorHandler);
                
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
            
            var rightGeographicTransformationStep = obj as ArcGISGeographicTransformationStep;
            
            if (rightGeographicTransformationStep == null)
            {
                return false;
            }
            
            var localRightGeographicTransformationStep = rightGeographicTransformationStep.Handle;
            
            if (Handle == localRightGeographicTransformationStep)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GeographicTransformationStep_equals(Handle, localRightGeographicTransformationStep, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public override int GetHashCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GeographicTransformationStep_getHash(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return (int)localResult.ToUInt64();
        }
        
        public static implicit operator bool(ArcGISGeographicTransformationStep other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeographicTransformationStep_createWithWKID(int WKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeographicTransformationStep_createWithWKText([MarshalAs(UnmanagedType.LPStr)]string WKText, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeographicTransformationStep_getIsInverse(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeographicTransformationStep_getIsMissingProjectionEngineFiles(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeographicTransformationStep_getProjectionEngineFilenames(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_GeographicTransformationStep_getWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeographicTransformationStep_getWKText(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeographicTransformationStep_getWKText2(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeographicTransformationStep_getInverse(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeographicTransformationStep_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeographicTransformationStep_equals(IntPtr handle, IntPtr rightGeographicTransformationStep, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GeographicTransformationStep_getHash(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}