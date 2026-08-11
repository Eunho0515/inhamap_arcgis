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

namespace Esri.GameEngine.MapView
{
    /// <summary>
    /// The identify layer result contains the identified geoelements of a layer.
    /// </summary>
    /// <remarks>
    /// Operations that identify the geoelements in a layer, such as <see cref="">GeoView.identifyLayerAsync(Layer, Coordinate, double, bool)</see>
    /// or <see cref="">GeoView.identifyLayersAsync(Coordinate, double, bool)</see>, return the resulting geoelements in a
    /// <see cref="">IdentifyLayerResult.geoElements</see> collection for each layer.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISIdentifyLayerResult
    {
        #region Properties
        /// <summary>
        /// The error that occurred during the identify operation, if there is one.
        /// </summary>
        /// <since>1.0.0</since>
        public Exception Error
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_IdentifyLayerResult_getError(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISError(new Standard.ArcGISError(localResult));
            }
        }
        
        /// <summary>
        /// The collection of identified geoelements.
        /// </summary>
        /// <remarks>
        /// If there are no geoelement results at the layer level (for layers where geoelement results are exposed in
        /// sublayer results), an empty collection is returned. The function will always return an array containing
        /// objects that implement <see cref="GameEngine.Map.ArcGISGeoElement">ArcGISGeoElement</see>. The specific type of geoelement in the collection depends on the type
        /// of objects contained in the layer.
        /// </remarks>
        /// <since>2.2.0</since>
        public Unity.ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> GeoElements
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_IdentifyLayerResult_getGeoElementsImmutableCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<GameEngine.Map.ArcGISGeoElement>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The identify layer result's collection of sub results.
        /// </summary>
        /// <remarks>
        /// For layers that do not contain sublayers, this array will be empty.
        /// </remarks>
        /// <since>2.2.0</since>
        public Unity.ArcGISImmutableCollection<ArcGISIdentifyLayerResult> SublayerResults
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_IdentifyLayerResult_getSublayerResultsImmutableCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<ArcGISIdentifyLayerResult> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<ArcGISIdentifyLayerResult>(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISIdentifyLayerResult(IntPtr handle) => Handle = handle;
        
        ~ArcGISIdentifyLayerResult()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_IdentifyLayerResult_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISIdentifyLayerResult other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_IdentifyLayerResult_getError(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_IdentifyLayerResult_getGeoElementsImmutableCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_IdentifyLayerResult_getSublayerResultsImmutableCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_IdentifyLayerResult_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}