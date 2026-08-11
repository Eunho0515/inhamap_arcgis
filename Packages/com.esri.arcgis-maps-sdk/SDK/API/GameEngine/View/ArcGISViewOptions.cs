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

namespace Esri.GameEngine.View
{
    /// <summary>
    /// A struct for configuration options for the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
    /// </summary>
    /// <remarks>
    /// Use this to set up the configuration of the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public struct ArcGISViewOptions
    {
        /// <summary>
        /// Forces data to be loaded for invisible layers.
        /// </summary>
        /// <remarks>
        /// Default value is false.
        /// </remarks>
        [MarshalAs(UnmanagedType.I1)]
        public bool LoadDataFromInvisibleLayers;
    }
}