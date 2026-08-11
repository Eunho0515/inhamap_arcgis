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
namespace Esri.GameEngine.Map.Symbology
{
    /// <summary>
    /// The list of possible size units for symbols.
    /// </summary>
    /// <remarks>
    /// It describes the size units that can be applied to the symbols.
    /// For instance using DIPs for <see cref="">ModelSceneSymbol</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    public enum ArcGISSymbolSizeUnits
    {
        /// <summary>
        /// Render the affected symbol by interpreting the size values as DIPs. Symbols in this mode remain the same screen space size no matter the camera's distance from the symbol itself.
        /// </summary>
        /// <since>1.0.0</since>
        DIPs = 0,
        
        /// <summary>
        /// Render the affected symbol by interpreting the size values as meters. Symbols in this mode remain the same world space size no matter the camera's distance from the symbol itself.
        /// </summary>
        /// <since>1.0.0</since>
        Meters = 1
    };
}