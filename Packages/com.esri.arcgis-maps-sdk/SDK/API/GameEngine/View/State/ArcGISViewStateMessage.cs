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

namespace Esri.GameEngine.View.State
{
    /// <summary>
    /// A view message.
    /// </summary>
    /// <remarks>
    /// This object contains the error or warning message that comes from the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISViewStateMessage
    {
        #region Methods
        /// <summary>
        /// Retrieve the additional information if it exists.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.View.State.ArcGISViewStateMessage">ArcGISViewStateMessage</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public Unity.ArcGISDictionary<string, string> GetAdditionalInformation()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEViewStateMessage_getAdditionalInformation(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISDictionary<string, string> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISDictionary<string, string>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieve warning or error code.
        /// </summary>
        /// <returns>
        /// The unique code identifies an error or warning.
        /// </returns>
        /// <since>1.0.0</since>
        public uint GetCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEViewStateMessage_getCode(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Retrieve error or warning message if it exists.
        /// </summary>
        /// <returns>
        /// This message contains the error or warning description.
        /// </returns>
        /// <since>1.0.0</since>
        public string GetMessage()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEViewStateMessage_getMessage(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        
        /// <summary>
        /// Retrieve the message type.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.View.State.ArcGISViewStateMessageType">ArcGISViewStateMessageType</see>
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISViewStateMessageType GetMessageType()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEViewStateMessage_getMessageType(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISViewStateMessage(IntPtr handle) => Handle = handle;
        
        ~ArcGISViewStateMessage()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEViewStateMessage_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISViewStateMessage other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEViewStateMessage_getAdditionalInformation(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern uint RT_GEViewStateMessage_getCode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEViewStateMessage_getMessage(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISViewStateMessageType RT_GEViewStateMessage_getMessageType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEViewStateMessage_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}