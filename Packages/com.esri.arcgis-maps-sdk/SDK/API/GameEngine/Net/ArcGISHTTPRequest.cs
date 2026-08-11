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

namespace Esri.GameEngine.Net
{
    /// <summary>
    /// An HTTP request that needs to be processed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISHTTPRequest
    {
        #region Properties
        /// <summary>
        /// This body of the request.
        /// </summary>
        internal global::Unity.Collections.NativeArray<byte> Body
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPRequest_getBody(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISByteArrayStruct(localResult);
            }
        }
        
        /// <summary>
        /// Indicates whether the request has been canceled.
        /// </summary>
        internal bool Canceled
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPRequest_getCanceled(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The headers of the request.
        /// </summary>
        internal Unity.ArcGISDictionary<string, string> Headers
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPRequest_getHeaders(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISDictionary<string, string> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISDictionary<string, string>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The HTTP method of the request.
        /// </summary>
        internal ArcGISHTTPMethod Method
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPRequest_getMethod(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The URI of the request.
        /// </summary>
        internal string URI
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPRequest_getURI(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Notifies an error occurred while processing the request.
        /// </summary>
        internal void HandleError()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEHTTPRequest_handleError(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Handles the response for the current request.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        internal void HandleResponse(ArcGISHTTPResponse response)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResponse = response.Handle;
            
            PInvoke.RT_GEHTTPRequest_handleResponse(Handle, localResponse, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISHTTPRequest(IntPtr handle) => Handle = handle;
        
        ~ArcGISHTTPRequest()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEHTTPRequest_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISHTTPRequest other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern Standard.ArcGISIntermediateByteArrayStruct RT_GEHTTPRequest_getBody(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GEHTTPRequest_getCanceled(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEHTTPRequest_getHeaders(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISHTTPMethod RT_GEHTTPRequest_getMethod(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEHTTPRequest_getURI(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEHTTPRequest_handleError(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEHTTPRequest_handleResponse(IntPtr handle, IntPtr response, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEHTTPRequest_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}