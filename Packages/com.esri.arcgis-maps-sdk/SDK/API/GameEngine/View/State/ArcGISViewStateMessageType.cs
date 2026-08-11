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
namespace Esri.GameEngine.View.State
{
    /// <summary>
    /// Specifies the type of view message.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISViewStateMessageType
    {
        /// <summary>
        /// Message contains error information.
        /// </summary>
        /// <since>1.0.0</since>
        Error = 1,
        
        /// <summary>
        /// Message contains warning information.
        /// </summary>
        /// <since>1.0.0</since>
        Warning = 2
    };
}