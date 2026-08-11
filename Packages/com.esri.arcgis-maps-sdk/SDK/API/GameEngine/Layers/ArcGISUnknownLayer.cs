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

namespace Esri.GameEngine.Layers
{
    /// <summary>
    /// An instance of this class represents a layer whose type could not be determined.
    /// </summary>
    /// <remarks>
    /// The layer will not be persisted in the string returned by <see cref="">Map.toJSON</see>,
    /// and will not be drawn by the  view.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISUnknownLayer :
        GameEngine.Layers.Base.ArcGISLayer
    #pragma warning restore CS0661, CS0660
    {
        #region Internal Members
        internal ArcGISUnknownLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        #endregion // P-Invoke Declarations
    }
}