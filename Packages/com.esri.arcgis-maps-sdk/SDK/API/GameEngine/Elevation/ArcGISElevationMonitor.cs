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
    /// An <see cref="GameEngine.Elevation.ArcGISElevationMonitor">ArcGISElevationMonitor</see> monitors elevation changes at a position.
    /// </summary>
    /// <remarks>
    /// While registered in the <see cref="GameEngine.Elevation.ArcGISElevationProvider">ArcGISElevationProvider</see>, position updates are received when the elevation
    /// at the position changes.
    /// </remarks>
    /// <since>1.6.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISElevationMonitor
    {
        #region Constructors
        /// <summary>
        /// Creates an elevation monitor for a specific position.
        /// </summary>
        /// <param name="position">The point to get elevation updates. Only the x and y coordinates of the position are taken into account.</param>
        /// <param name="mode">The elevation mode for the point.</param>
        /// <param name="offset">The relative offset to be added.</param>
        /// <since>1.6.0</since>
        public ArcGISElevationMonitor(GameEngine.Geometry.ArcGISPoint position, ArcGISElevationMode mode, double offset)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPosition = position.Handle;
            
            Handle = PInvoke.RT_GEElevationMonitor_create(localPosition, mode, offset, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The elevation mode for the point.
        /// </summary>
        /// <since>1.6.0</since>
        public ArcGISElevationMode Mode
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEElevationMonitor_getMode(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// A relative offset that's added to the point's elevation.
        /// </summary>
        /// <since>1.6.0</since>
        public double Offset
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEElevationMonitor_getOffset(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The updated position.
        /// </summary>
        /// <since>1.6.0</since>
        public GameEngine.Geometry.ArcGISPoint Position
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEElevationMonitor_getPosition(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Events
        /// <summary>
        /// An <see cref="GameEngine.Elevation.ArcGISElevationMonitorPositionChangedEvent">ArcGISElevationMonitorPositionChangedEvent</see> that is invoked when the elevation changes.
        /// </summary>
        /// <since>1.6.0</since>
        public ArcGISElevationMonitorPositionChangedEvent PositionChanged
        {
            get
            {
                return _positionChangedHandler.Delegate;
            }
            set
            {
                if (_positionChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _positionChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEElevationMonitor_setPositionChangedCallback(Handle, ArcGISElevationMonitorPositionChangedEventHandler.HandlerFunction, _positionChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEElevationMonitor_setPositionChangedCallback(Handle, null, _positionChangedHandler.UserData, errorHandler);
                    
                    _positionChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal ArcGISElevationMonitor(IntPtr handle) => Handle = handle;
        
        ~ArcGISElevationMonitor()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_positionChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEElevationMonitor_setPositionChangedCallback(Handle, null, _positionChangedHandler.UserData, IntPtr.Zero);
                    
                    _positionChangedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEElevationMonitor_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISElevationMonitor other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        
        internal ArcGISElevationMonitorPositionChangedEventHandler _positionChangedHandler = new ArcGISElevationMonitorPositionChangedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEElevationMonitor_create(IntPtr position, ArcGISElevationMode mode, double offset, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISElevationMode RT_GEElevationMonitor_getMode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GEElevationMonitor_getOffset(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEElevationMonitor_getPosition(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationMonitor_setPositionChangedCallback(IntPtr handle, ArcGISElevationMonitorPositionChangedEventInternal positionChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationMonitor_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}