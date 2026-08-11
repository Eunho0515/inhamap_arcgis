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

namespace Esri.Standard
{
    /// <summary>
    /// The client reference object.
    /// </summary>
    /// <remarks>
    /// A reference to data owned by the client SDK or extension, but whose lifetime
    /// is managed by RTC.
    /// 
    /// Prior to constructing an instance of <see cref="Standard.ArcGISClientReference">ArcGISClientReference</see>, the client SDK or
    /// extension must use <see cref="">ArcGISRuntimeEnvironment.clientReferenceRelease</see> or
    /// ArcGISRuntimeEnvironment.registerClientReferenceReleaseHandler(ClientReferenceReleaseHandler, cpointer<void>)
    /// (respectively) to register a callback which RTC invokes to indicate the
    /// client SDK or extension can release data referenced by a <see cref="Standard.ArcGISClientReference">ArcGISClientReference</see>
    /// it created.
    /// </remarks>
    /// <seealso cref="">ArcGISRuntimeEnvironment.clientReferenceRelease</seealso>
    /// <seealso cref="">ArcGISRuntimeEnvironment.registerClientReferenceReleaseHandler(ClientReferenceReleaseHandler
    /// cpointer<void>)</seealso>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISClientReference
    {
        #region Properties
        /// <summary>
        /// Reference to data owned by the client SDK or extension.
        /// </summary>
        internal IntPtr Data
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ClientReference_getData(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Create a <see cref="Standard.ArcGISClientReference">ArcGISClientReference</see> that references data owned by the client SDK.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling
        /// <see cref="">ClientReference.fromClientDataAndId(cpointer<void>, cpointer<void>)</see>
        /// with a client ID of null.
        /// </remarks>
        /// <param name="clientData">Reference to data owned by the client SDK.</param>
        /// <returns>
        /// A <see cref="Standard.ArcGISClientReference">ArcGISClientReference</see>.
        /// </returns>
        /// <seealso cref="">ArcGISRuntimeEnvironment.clientReferenceRelease</seealso>
        /// <seealso cref="">ClientReference.fromClientDataAndId(cpointer<void>
        /// cpointer<void>)</seealso>
        internal static ArcGISClientReference FromClientData(IntPtr clientData)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ClientReference_fromClientData(clientData, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISClientReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISClientReference(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISClientReference(IntPtr handle) => Handle = handle;
        
        ~ArcGISClientReference()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ClientReference_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISClientReference other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ClientReference_getData(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ClientReference_fromClientData(IntPtr clientData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ClientReference_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}