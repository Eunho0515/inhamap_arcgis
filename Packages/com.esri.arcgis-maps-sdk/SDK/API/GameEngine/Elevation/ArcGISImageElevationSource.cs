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

namespace Esri.GameEngine.Elevation
{
    /// <summary>
    /// Public class that will contain a tiled image elevation source.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISImageElevationSource :
        GameEngine.Elevation.Base.ArcGISElevationSource
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates a new ElevationSource.
        /// </summary>
        /// <remarks>
        /// Creates a new ElevationSource.
        /// </remarks>
        /// <param name="source">ElevationSource source.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>1.0.0</since>
        public ArcGISImageElevationSource(string source, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEImageElevationSource_create(source, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new ElevationSource.
        /// </summary>
        /// <remarks>
        /// Creates a new ElevationSource.
        /// </remarks>
        /// <param name="source">ElevationSource source.</param>
        /// <param name="name">ElevationSource name.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>1.0.0</since>
        public ArcGISImageElevationSource(string source, string name, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEImageElevationSource_createWithName(source, name, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Internal Members
        internal ArcGISImageElevationSource(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEImageElevationSource_create([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEImageElevationSource_createWithName([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}