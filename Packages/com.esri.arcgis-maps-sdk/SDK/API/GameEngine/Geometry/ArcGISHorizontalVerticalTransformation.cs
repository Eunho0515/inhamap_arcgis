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
    /// Used to transform coordinates of z-aware geometries between spatial references that have different
    /// geographic and/or vertical coordinate systems.
    /// </summary>
    /// <remarks>
    /// A HorizontalVerticalTransformation is an ordered list of <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> objects.
    /// Each HorizontalVerticalTransformation has an input and an output <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>, and this
    /// HorizontalVerticalTransformation object can be used to convert coordinates between the horizontal (geographic)
    /// and vertical datums of these spatial references using the series of steps it contains. Use the
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see> method to transform the coordinates
    /// of a specific <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>.
    /// 
    /// A horizontal transformation step is not needed when the input and output spatial references have the same
    /// underlying geographic coordinate systems. A vertical transformation is not needed if both datums (for
    /// ellipsoidal heights) or vertical datums (for gravity-related heights) are the same. To transform coordinates
    /// only between different horizontal (geographic) coordinate systems, you can use a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see>
    /// instead.
    /// 
    /// The inverse of this transformation, used to transform in the opposite direction, is returned from the
    /// <see cref="GameEngine.Geometry.ArcGISDatumTransformation.GetInverse">ArcGISDatumTransformation.GetInverse</see> method.
    /// 
    /// A HorizontalVerticalTransformation can be constructed from a single <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see>
    /// object, or from a number of transformation step objects that are chained together.
    /// 
    /// You can get a list of suitable transformations for a given input and output spatial reference using one of the
    /// methods on the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog">ArcGISTransformationCatalog</see> class.
    /// 
    /// Some transformations require that certain Projection Engine data files be present on the local file system,
    /// and vertical transformation steps are especially likely to use such files, which can be very large in size.
    /// The <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> property indicates whether any of the files are
    /// missing. The complete list of necessary files for each specific step is available using the
    /// <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames">ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames</see> property.
    /// 
    /// A HorizontalVerticalTransformation object is immutable.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISHorizontalVerticalTransformation :
        ArcGISDatumTransformation
    {
        #region Constructors
        /// <summary>
        /// Create a multi-step transformation from one or more <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> instances.
        /// </summary>
        /// <remarks>
        /// Use this constructor to create a horizontal-vertical transformation that has multiple steps.
        /// The output <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of each step should match the input <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the
        /// following step.
        /// </remarks>
        /// <param name="steps">An array containing <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> instances.</param>
        /// <since>1.0.0</since>
        public ArcGISHorizontalVerticalTransformation(Unity.ArcGISImmutableArray<ArcGISHorizontalVerticalTransformationStep> steps) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSteps = steps.Handle;
            
            Handle = PInvoke.RT_HorizontalVerticalTransformation_createWithSteps(localSteps, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Create a single step transformation.
        /// </summary>
        /// <param name="step">An <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep">ArcGISHorizontalVerticalTransformationStep</see> instance.</param>
        /// <since>1.0.0</since>
        public ArcGISHorizontalVerticalTransformation(ArcGISHorizontalVerticalTransformationStep step) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStep = step.Handle;
            
            Handle = PInvoke.RT_HorizontalVerticalTransformation_createWithStep(localStep, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The array of transformation steps that define this transformation.
        /// </summary>
        /// <since>1.0.0</since>
        public Unity.ArcGISImmutableArray<ArcGISHorizontalVerticalTransformationStep> Steps
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_HorizontalVerticalTransformation_getSteps(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableArray<ArcGISHorizontalVerticalTransformationStep> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableArray<ArcGISHorizontalVerticalTransformationStep>(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISHorizontalVerticalTransformation(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_HorizontalVerticalTransformation_createWithSteps(IntPtr steps, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_HorizontalVerticalTransformation_createWithStep(IntPtr step, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_HorizontalVerticalTransformation_getSteps(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}