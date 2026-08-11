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

namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// An ordered collection of points that can be managed as a single geometry.
    /// </summary>
    /// <remarks>
    /// <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> geometries represent an ordered collection of points. They can be used as
    /// the geometry of features and graphics, or as input or output for spatial operations. For
    /// features that consist of a very large number of points that share the same set of attribute
    /// values, multipoints may be more efficient to store and analyze in a geodatabase compared to
    /// using multiple point features.
    /// 
    /// A <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> is composed of a single read-only collection of <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>. Use <see cref="GameEngine.Geometry.ArcGISMultipointBuilder">ArcGISMultipointBuilder</see>
    /// to build a multipoint one point at a time or to modify an existing <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMultipoint :
        ArcGISGeometry
    {
        #region Properties
        /// <summary>
        /// The read-only collection of points for the multipoint.
        /// </summary>
        /// <remarks>
        /// Use <see cref="GameEngine.Geometry.ArcGISMultipointBuilder">ArcGISMultipointBuilder</see> to build a multipoint one point at a time or to modify
        /// the points that compose an existing <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISImmutablePointCollection Points
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Multipoint_getPoints(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISImmutablePointCollection localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISImmutablePointCollection(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISMultipoint(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Multipoint_getPoints(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}