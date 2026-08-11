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
    /// Defines how points in a <see cref="">PointCloudLayer</see> are rendered.
    /// </summary>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudRenderer
    {
        #region Properties
        /// <summary>
        /// The name of the attribute upon which rendering is based.
        /// </summary>
        /// <since>2.4.0</since>
        public string AttributeName
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudRenderer_getAttributeName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudRenderer_setAttributeName(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Reduces the brightness of the point's color based on the value of another attribute (usually intensity).
        /// </summary>
        /// <since>2.4.0</since>
        public ArcGISPointCloudColorModulation ColorModulation
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudRenderer_getColorModulation(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPointCloudColorModulation localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPointCloudColorModulation(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_PointCloudRenderer_setColorModulation(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The type of point cloud renderer.
        /// </summary>
        internal ArcGISPointCloudRendererType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudRenderer_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The number of points to draw per display inch. The default value is 10.
        /// </summary>
        /// <since>2.4.0</since>
        public double PointsPerInch
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudRenderer_getPointsPerInch(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudRenderer_setPointsPerInch(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// How the size of the points in the point cloud are computed for rendering.
        /// </summary>
        /// <remarks>
        /// When this property is null, a default size algorithm is used internally. The default value used is <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudSplatAlgorithm">ArcGISPointCloudSplatAlgorithm</see>.
        /// </remarks>
        /// <since>2.4.0</since>
        public ArcGISPointCloudSizeAlgorithm SizeAlgorithm
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudRenderer_getSizeAlgorithm(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPointCloudSizeAlgorithm localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    var objectType = GameEngine.Layers.PointCloud.PInvoke.RT_PointCloudSizeAlgorithm_getObjectType(localResult, IntPtr.Zero);
                    
                    switch (objectType)
                    {
                        case GameEngine.Layers.PointCloud.ArcGISPointCloudSizeAlgorithmType.PointCloudFixedSizeAlgorithm:
                            localLocalResult = new ArcGISPointCloudFixedSizeAlgorithm(localResult);
                            break;
                        case GameEngine.Layers.PointCloud.ArcGISPointCloudSizeAlgorithmType.PointCloudSplatAlgorithm:
                            localLocalResult = new ArcGISPointCloudSplatAlgorithm(localResult);
                            break;
                        default:
                            localLocalResult = new ArcGISPointCloudSizeAlgorithm(localResult);
                            break;
                    }
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_PointCloudRenderer_setSizeAlgorithm(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudRenderer(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudRenderer()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudRenderer_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISPointCloudRenderer other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudRenderer_getAttributeName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudRenderer_setAttributeName(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudRenderer_getColorModulation(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudRenderer_setColorModulation(IntPtr handle, IntPtr colorModulation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudRendererType RT_PointCloudRenderer_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudRenderer_getPointsPerInch(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudRenderer_setPointsPerInch(IntPtr handle, double pointsPerInch, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudRenderer_getSizeAlgorithm(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudRenderer_setSizeAlgorithm(IntPtr handle, IntPtr sizeAlgorithm, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudRenderer_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}