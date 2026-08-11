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
    /// Provides access to the <see cref="GameEngine.Attributes.ArcGISAttributeProcessorEvent">ArcGISAttributeProcessorEvent</see>.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISAttributeProcessor
    {
        #region Constructors
        /// <summary>
        /// Creates an attribute processor object.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISAttributeProcessor()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEAttributeProcessor_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Events
        /// <summary>
        /// Set an <see cref="GameEngine.Attributes.ArcGISAttributeProcessorEvent">ArcGISAttributeProcessorEvent</see> which is invoked when the requested attributes are available to
        ///  be processed.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISAttributeProcessorEvent ProcessEvent
        {
            get
            {
                return _processEventHandler.Delegate;
            }
            set
            {
                if (_processEventHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _processEventHandler.Delegate = value;
                    
                    PInvoke.RT_GEAttributeProcessor_setProcessEventCallback(Handle, ArcGISAttributeProcessorEventHandler.HandlerFunction, _processEventHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEAttributeProcessor_setProcessEventCallback(Handle, null, _processEventHandler.UserData, errorHandler);
                    
                    _processEventHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal ArcGISAttributeProcessor(IntPtr handle) => Handle = handle;
        
        ~ArcGISAttributeProcessor()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_processEventHandler.Delegate != null)
                {
                    PInvoke.RT_GEAttributeProcessor_setProcessEventCallback(Handle, null, _processEventHandler.UserData, IntPtr.Zero);
                    
                    _processEventHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEAttributeProcessor_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISAttributeProcessor other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        
        internal ArcGISAttributeProcessorEventHandler _processEventHandler = new ArcGISAttributeProcessorEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEAttributeProcessor_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEAttributeProcessor_setProcessEventCallback(IntPtr handle, ArcGISAttributeProcessorEventInternal processEvent, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEAttributeProcessor_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}