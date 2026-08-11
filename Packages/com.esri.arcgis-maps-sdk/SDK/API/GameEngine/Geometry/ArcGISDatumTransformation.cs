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
    /// Represents a function to convert between two coordinate systems.
    /// </summary>
    /// <remarks>
    /// This is the base class for classes used to transform coordinates between spatial
    /// references that have different datums. The inverse of the datum transformation,
    /// used to transform in the opposite direction, may be accessed using a member
    /// function.
    /// 
    /// A datum transformation has a name property intended to be suitable for display,
    /// such as when displaying a list of available transformations to an end user.
    /// 
    /// You can get a list of suitable transformations for a given input and output spatial
    /// reference using methods of the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog">ArcGISTransformationCatalog</see> class. Some transformations
    /// require that certain Projection Engine data files be present on the local file system.
    /// The property <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> indicates whether
    /// any of the files are missing. The complete list of necessary files is available using
    /// the <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.ProjectionEngineFilenames">ArcGISGeographicTransformationStep.ProjectionEngineFilenames</see> or
    /// <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames">ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames</see> properties.
    /// 
    /// In order for any Projection Engine files to be found, the data location must be set
    /// first using the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</see> property.
    /// 
    /// A datum transformation object is immutable.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISDatumTransformation
    {
        #region Properties
        /// <summary>
        /// The input <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </summary>
        /// <remarks>
        /// If transforming from a projected coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReference.IsProjected">ArcGISSpatialReference.IsProjected</see> is true), this input
        /// spatial reference should equal the <see cref="GameEngine.Geometry.ArcGISSpatialReference.BaseGeographic">ArcGISSpatialReference.BaseGeographic</see> of the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the
        /// <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> passed to <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference InputSpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_DatumTransformation_getInputSpatialReference(Handle, errorHandler);
                
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
        /// True if any files needed by the Projection Engine for this transformation step are missing from the local
        /// file system.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.ProjectionEngineFilenames">ArcGISGeographicTransformationStep.ProjectionEngineFilenames</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames">ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames</seealso>
        /// <since>1.0.0</since>
        public bool IsMissingProjectionEngineFiles
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_DatumTransformation_getIsMissingProjectionEngineFiles(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The name of the datum transformation.
        /// </summary>
        /// <remarks>
        /// For multi-step transformations, the name contains the concatenated names of
        /// each step's transformation, separated by a plus sign '+'. If the transformation is
        /// inverted, the name starts with a tilde (~).
        /// </remarks>
        /// <since>1.0.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_DatumTransformation_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The object type of this object.
        /// </summary>
        internal ArcGISDatumTransformationType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_DatumTransformation_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The output <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </summary>
        /// <remarks>
        /// If transforming to a projected coordinate system (<see cref="GameEngine.Geometry.ArcGISSpatialReference.IsProjected">ArcGISSpatialReference.IsProjected</see> is true), this output
        /// spatial reference should equal the <see cref="GameEngine.Geometry.ArcGISSpatialReference.BaseGeographic">ArcGISSpatialReference.BaseGeographic</see> of the outputSpatialReference
        /// parameter passed to <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference OutputSpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_DatumTransformation_getOutputSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Returns the inverse of this datum transformation.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> representing the inverse, or null if an inverse for this data transformation does not exist.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISDatumTransformation GetInverse()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_DatumTransformation_getInverse(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISDatumTransformation localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_DatumTransformation_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISDatumTransformationType.GeographicTransformation:
                        localLocalResult = new ArcGISGeographicTransformation(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISDatumTransformationType.HorizontalVerticalTransformation:
                        localLocalResult = new ArcGISHorizontalVerticalTransformation(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISDatumTransformation(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISDatumTransformation(IntPtr handle) => Handle = handle;
        
        ~ArcGISDatumTransformation()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_DatumTransformation_destroy(Handle, errorHandler);
                
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
            
            var rightDatumTransformation = obj as ArcGISDatumTransformation;
            
            if (rightDatumTransformation == null)
            {
                return false;
            }
            
            var localRightDatumTransformation = rightDatumTransformation.Handle;
            
            if (Handle == localRightDatumTransformation)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_DatumTransformation_equals(Handle, localRightDatumTransformation, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public override int GetHashCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_DatumTransformation_getHash(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return (int)localResult.ToUInt64();
        }
        
        public static implicit operator bool(ArcGISDatumTransformation other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_DatumTransformation_getInputSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_DatumTransformation_getIsMissingProjectionEngineFiles(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_DatumTransformation_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISDatumTransformationType RT_DatumTransformation_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_DatumTransformation_getOutputSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_DatumTransformation_getInverse(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_DatumTransformation_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_DatumTransformation_equals(IntPtr handle, IntPtr rightDatumTransformation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_DatumTransformation_getHash(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}