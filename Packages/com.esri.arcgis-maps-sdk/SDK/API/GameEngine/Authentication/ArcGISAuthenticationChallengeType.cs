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
namespace Esri.GameEngine.Authentication
{
    /// <summary>
    /// The types of ArcGIS authentication challenges.
    /// </summary>
    /// <since>2.1.0</since>
    public enum ArcGISAuthenticationChallengeType
    {
        /// <summary>
        /// The type of challenge issued while accessing token secured ArcGIS resources.
        /// </summary>
        /// <remarks>
        /// This type of challenge can be handled using <see cref="">TokenCredential</see> or
        /// <see cref="">PregeneratedTokenCredential</see>.
        /// </remarks>
        /// <since>2.1.0</since>
        Token = 0,
        
        /// <summary>
        /// The type of challenge issued while accessing OAuth secured ArcGIS resources.
        /// </summary>
        /// <remarks>
        /// This type of challenge can be handled using <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>
        /// or <see cref="">OAuthApplicationCredential</see>. If the OAuth information is not
        /// available, this challenge can also be handled using <see cref="">TokenCredential</see>
        /// or <see cref="">PregeneratedTokenCredential</see>.
        /// </remarks>
        /// <since>2.1.0</since>
        OAuthOrToken = 1,
        
        /// <summary>
        /// The type of challenge issued while accessing ArcGIS resources secured behind Identity-Aware Proxy (IAP).
        /// </summary>
        /// <remarks>
        /// This type of challenge can be handled using <see cref="">IAPCredential</see>.
        /// </remarks>
        /// <since>2.1.0</since>
        IAP = 2
    };
}