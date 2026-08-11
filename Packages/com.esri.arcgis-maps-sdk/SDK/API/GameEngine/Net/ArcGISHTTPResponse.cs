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
    /// An HTTP response for a HTTP request.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISHTTPResponse
    {
        #region Constructors
        /// <summary>
        /// Creates a new HTTP response.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="body">The body of the response.</param>
        internal ArcGISHTTPResponse(ushort statusCode, global::Unity.Collections.NativeArray<byte> body)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localBody = Unity.Convert.ToArcGISByteArrayStruct(body);
            
            Handle = PInvoke.RT_GEHTTPResponse_create(statusCode, localBody, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// This body of the request.
        /// </summary>
        internal global::Unity.Collections.NativeArray<byte> Body
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPResponse_getBody(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISByteArrayStruct(localResult);
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
                
                var localResult = PInvoke.RT_GEHTTPResponse_getHeaders(Handle, errorHandler);
                
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
        /// The status code of the response.
        /// </summary>
        internal ushort StatusCode
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEHTTPResponse_getStatusCode(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISHTTPResponse(IntPtr handle) => Handle = handle;
        
        ~ArcGISHTTPResponse()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEHTTPResponse_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISHTTPResponse other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEHTTPResponse_create(ushort statusCode, Standard.ArcGISIntermediateByteArrayStruct body, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern Standard.ArcGISIntermediateByteArrayStruct RT_GEHTTPResponse_getBody(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEHTTPResponse_getHeaders(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ushort RT_GEHTTPResponse_getStatusCode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEHTTPResponse_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}