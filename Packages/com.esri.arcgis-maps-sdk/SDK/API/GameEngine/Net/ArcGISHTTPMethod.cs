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
namespace Esri.GameEngine.Net
{
    /// <summary>
    /// The list of available HTTP methods.
    /// </summary>
    public enum ArcGISHTTPMethod
    {
        /// <summary>
        /// The HTTP DELETE method.
        /// </summary>
        Delete = 0,
        
        /// <summary>
        /// The HTTP GET method.
        /// </summary>
        Get = 1,
        
        /// <summary>
        /// The HTTP HEAD method.
        /// </summary>
        Head = 2,
        
        /// <summary>
        /// The HTTP OPTIONS method.
        /// </summary>
        Options = 3,
        
        /// <summary>
        /// The HTTP PATCH method.
        /// </summary>
        Patch = 4,
        
        /// <summary>
        /// The HTTP POST method.
        /// </summary>
        Post = 5,
        
        /// <summary>
        /// The HTTP PUT method.
        /// </summary>
        Put = 6,
        
        /// <summary>
        /// Unknown HTTP method.
        /// </summary>
        Unknown = -1
    };
}