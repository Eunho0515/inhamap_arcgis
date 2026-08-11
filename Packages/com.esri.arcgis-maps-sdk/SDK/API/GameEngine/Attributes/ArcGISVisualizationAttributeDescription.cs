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

namespace Esri.GameEngine.Attributes
{
    /// <summary>
    /// Describes how the visualization attributes are provided to <see cref="GameEngine.Attributes.ArcGISAttributeProcessorEvent">ArcGISAttributeProcessorEvent</see>.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISVisualizationAttributeDescription
    {
        #region Constructors
        /// <summary>
        /// Creates a VisualizationAttributeDescription object.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="visualizationAttributeType">The type of the visualization attribute.</param>
        /// <since>1.0.0</since>
        public ArcGISVisualizationAttributeDescription(string name, ArcGISVisualizationAttributeType visualizationAttributeType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEVisualizationAttributeDescription_create(name, visualizationAttributeType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The attribute name.
        /// </summary>
        /// <since>1.0.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEVisualizationAttributeDescription_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The type of the visualization attribute.
        /// </summary>
        /// <seealso cref="GameEngine.Attributes.ArcGISVisualizationAttributeType">ArcGISVisualizationAttributeType</seealso>
        /// <since>1.0.0</since>
        public ArcGISVisualizationAttributeType VisualizationAttributeType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEVisualizationAttributeDescription_getVisualizationAttributeType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISVisualizationAttributeDescription(IntPtr handle) => Handle = handle;
        
        ~ArcGISVisualizationAttributeDescription()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEVisualizationAttributeDescription_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISVisualizationAttributeDescription other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEVisualizationAttributeDescription_create([MarshalAs(UnmanagedType.LPStr)]string name, ArcGISVisualizationAttributeType visualizationAttributeType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEVisualizationAttributeDescription_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISVisualizationAttributeType RT_GEVisualizationAttributeDescription_getVisualizationAttributeType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEVisualizationAttributeDescription_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}