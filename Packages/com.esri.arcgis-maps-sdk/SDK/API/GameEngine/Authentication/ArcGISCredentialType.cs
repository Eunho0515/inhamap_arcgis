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
    /// The list of credential types.
    /// </summary>
    /// <remarks>
    /// This is used to determine the credential type.
    /// </remarks>
    /// <seealso cref="GameEngine.Authentication.ArcGISCredential.ObjectType">ArcGISCredential.ObjectType</seealso>
    public enum ArcGISCredentialType
    {
        /// <summary>
        /// A token credential.
        /// </summary>
        /// <seealso cref="">TokenCredential</seealso>
        TokenCredential = 1,
        
        /// <summary>
        /// A pregenerated token credential.
        /// </summary>
        /// <seealso cref="">PregeneratedTokenCredential</seealso>
        PregeneratedTokenCredential = 2,
        
        /// <summary>
        /// An OAuth user credential.
        /// </summary>
        /// <seealso cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</seealso>
        OAuthUserCredential = 3,
        
        /// <summary>
        /// An OAuth application credential.
        /// </summary>
        /// <seealso cref="">OAuthApplicationCredential</seealso>
        OAuthApplicationCredential = 4,
        
        /// <summary>
        /// An IAP credential.
        /// </summary>
        /// <seealso cref="">IAPCredential</seealso>
        IAPCredential = 5
    };
}