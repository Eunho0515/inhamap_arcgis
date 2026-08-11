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

namespace Esri.GameEngine.Map
{
    /// <summary>
    /// Utilities for parsing information from URL objects.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISURLUtils
    {
        #region Methods
        /// <summary>
        /// Converts the URL's scheme to HTTPS.
        /// </summary>
        /// <param name="URL">The URL to convert.</param>
        /// <returns>
        /// The URL with a scheme of HTTPS.
        /// </returns>
        internal static string ConvertToHTTPS(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_URLUtils_convertToHTTPS(URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        
        /// <summary>
        /// Gets the path by which the root of a server is accessed.
        /// </summary>
        /// <remarks>
        /// This is the base URL against which rest endpoints are resolved. For example,
        /// "https://sampleserver3.arcgisonline.com/ArcGIS/rest/services/SanFrancisco/311Incidents/FeatureServer/0"
        /// would have a server context of "https://sampleserver3.arcgisonline.com/ArcGIS", on which we could add
        /// "/rest/info" or "/rest" to fetch the server information.
        /// </remarks>
        /// <param name="URL">The URL from which to get the server context.</param>
        /// <returns>
        /// The server's root access URL.
        /// </returns>
        internal static string GetServerContext(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_URLUtils_getServerContext(URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        
        /// <summary>
        /// Gets the server info rest endpoint of the given URL.
        /// </summary>
        /// <remarks>
        /// For example, "https://sampleserver3.arcgisonline.com/ArcGIS/rest/services/SanFrancisco/311Incidents/FeatureServer/0" 
        /// would have a server info URL of "https://sampleserver3.arcgisonline.com/ArcGIS/rest/info".
        /// </remarks>
        /// <param name="URL">The URL from which to get the server info URL.</param>
        /// <returns>
        /// The server info rest endpoint URL.
        /// </returns>
        internal static string GetServerInfoURL(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_URLUtils_getServerInfoURL(URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        
        /// <summary>
        /// Checks if the given URL is using the HTTP scheme.
        /// </summary>
        /// <param name="URL">The URL to check.</param>
        /// <returns>
        /// True if the URL is using the HTTP scheme, otherwise false.
        /// </returns>
        internal static bool IsHTTP(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_URLUtils_isHTTP(URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Checks if the given URL is using the HTTPS scheme.
        /// </summary>
        /// <param name="URL">The URL to check.</param>
        /// <returns>
        /// True if the URL is using the HTTPS scheme, otherwise false.
        /// </returns>
        internal static bool IsHTTPS(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_URLUtils_isHTTPS(URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Normalizes the URL by lowercasing it, removing query parameters, and converting the scheme to https.
        /// </summary>
        /// <param name="URL">The URL to normalize.</param>
        /// <returns>
        /// The normalized URL.
        /// </returns>
        internal static string Normalize(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_URLUtils_normalize(URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        #endregion // Methods
        
        #region Internal Members
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_URLUtils_convertToHTTPS([MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_URLUtils_getServerContext([MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_URLUtils_getServerInfoURL([MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_URLUtils_isHTTP([MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_URLUtils_isHTTPS([MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_URLUtils_normalize([MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}