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
    /// Manages the registration of positions that will get elevation updates.
    /// </summary>
    /// <since>1.6.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISElevationProvider
    {
        #region Methods
        /// <summary>
        /// Dispatch events for all updated positions.
        /// </summary>
        /// <remarks>
        /// This method should be called once per frame.
        /// </remarks>
        internal void DispatchEvents()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEElevationProvider_dispatchEvents(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Registers a position to start receiving elevation updates.
        /// </summary>
        /// <param name="elevationMonitor">The position to register.</param>
        /// <since>1.6.0</since>
        public void RegisterMonitor(ArcGISElevationMonitor elevationMonitor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localElevationMonitor = elevationMonitor.Handle;
            
            PInvoke.RT_GEElevationProvider_registerMonitor(Handle, localElevationMonitor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Unregisters a position to stop receiving elevation updates.
        /// </summary>
        /// <param name="elevationMonitor">The position to unregister.</param>
        /// <since>1.6.0</since>
        public void UnregisterMonitor(ArcGISElevationMonitor elevationMonitor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localElevationMonitor = elevationMonitor.Handle;
            
            PInvoke.RT_GEElevationProvider_unregisterMonitor(Handle, localElevationMonitor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISElevationProvider(IntPtr handle) => Handle = handle;
        
        ~ArcGISElevationProvider()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEElevationProvider_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISElevationProvider other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationProvider_dispatchEvents(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationProvider_registerMonitor(IntPtr handle, IntPtr elevationMonitor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationProvider_unregisterMonitor(IntPtr handle, IntPtr elevationMonitor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationProvider_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}