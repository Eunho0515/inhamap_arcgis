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

namespace Esri.GameEngine.Layers.PointCloud
{
    /// <summary>
    /// Renderer for point clouds using class breaks.
    /// </summary>
    /// <remarks>
    /// Colors are assigned based on ranges of data. Each point is assigned a color based on the class break in which the value of the attribute falls.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudClassBreaksRenderer :
        ArcGISPointCloudRenderer
    {
        #region Constructors
        /// <summary>
        /// Creates a class breaks renderer with the specified attribute name and collection of class breaks.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to use for the renderer.</param>
        /// <param name="classBreaksCollection">A collection of class breaks that define a color to apply to each range of values.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudClassBreaksRenderer(string attributeName, Unity.ArcGISCollection<ArcGISPointCloudColorClassBreak> classBreaksCollection) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localClassBreaksCollection = classBreaksCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudClassBreaksRenderer_createWithClassBreaksCollection(attributeName, localClassBreaksCollection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A collection of class breaks that define a color to apply to each range of values.
        /// </summary>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<ArcGISPointCloudColorClassBreak> ClassBreaksCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudClassBreaksRenderer_getClassBreaksCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<ArcGISPointCloudColorClassBreak> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<ArcGISPointCloudColorClassBreak>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudClassBreaksRenderer_setClassBreaksCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A transform applied to the attribute value before evaluating the renderer.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttributeTransformType.None">ArcGISPointCloudAttributeTransformType.None</see>, which means that no transform is applied to the attribute value before evaluating the renderer.
        /// </remarks>
        /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttributeTransformType">ArcGISPointCloudAttributeTransformType</seealso>
        /// <since>2.4.0</since>
        public ArcGISPointCloudAttributeTransformType TransformType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudClassBreaksRenderer_getTransformType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudClassBreaksRenderer_setTransformType(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudClassBreaksRenderer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudClassBreaksRenderer_createWithClassBreaksCollection([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr classBreaksCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudClassBreaksRenderer_getClassBreaksCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudClassBreaksRenderer_setClassBreaksCollection(IntPtr handle, IntPtr classBreaksCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudAttributeTransformType RT_PointCloudClassBreaksRenderer_getTransformType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudClassBreaksRenderer_setTransformType(IntPtr handle, ArcGISPointCloudAttributeTransformType transformType, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}