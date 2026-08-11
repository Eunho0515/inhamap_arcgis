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
    /// The envelope builder allows you to create and modify envelope geometries incrementally.
    /// </summary>
    /// <remarks>
    /// Envelope geometries are immutable and cannot be changed directly once created. Use the constructors to define
    /// minimum and maximum x,y coordinates, z and m values, and a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>. Once set, the
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> cannot be changed. Use <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see> to return the new <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> from
    /// the builder.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISEnvelopeBuilder :
        ArcGISGeometryBuilder
    {
        #region Constructors
        /// <summary>
        /// Creates an envelope builder from the specified <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the new envelope builder will match that of the given <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>. Use this
        /// constructor in workflows that create a modified version of an existing geometry.
        /// </remarks>
        /// <param name="envelope">The envelope.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelopeBuilder(ArcGISEnvelope envelope) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localEnvelope = envelope == null ? System.IntPtr.Zero : envelope.Handle;
            
            Handle = PInvoke.RT_EnvelopeBuilder_createFromEnvelope(localEnvelope, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope builder from a center point and a width and height.
        /// </summary>
        /// <param name="center">The center point of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.</param>
        /// <param name="width">The width of the envelope around the center point.</param>
        /// <param name="height">The height of the envelope around the center point.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelopeBuilder(ArcGISPoint center, double width, double height) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center.Handle;
            
            Handle = PInvoke.RT_EnvelopeBuilder_createFromCenterPoint(localCenter, width, height, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope builder from a center point and a width, height, and depth.
        /// </summary>
        /// <param name="center">The center point of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.</param>
        /// <param name="width">The width of the envelope around the center point.</param>
        /// <param name="height">The height of the envelope around the center point.</param>
        /// <param name="depth">The depth of the envelope around the center point.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelopeBuilder(ArcGISPoint center, double width, double height, double depth) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center.Handle;
            
            Handle = PInvoke.RT_EnvelopeBuilder_createFromCenterPointAndDepth(localCenter, width, height, depth, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope builder.
        /// </summary>
        /// <param name="spatialReference">The builder's spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelopeBuilder(ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_EnvelopeBuilder_createFromSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The center point of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint Center
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getCenter(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The depth of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// This is the difference between the minimum and maximum z values. A 2D envelope has zero depth.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Depth
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getDepth(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The height of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// This is the difference between the minimum and maximum y coordinates.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Height
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getHeight(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The m maximum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double MMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getMMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setMMax(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The m minimum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double MMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getMMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setMMin(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The width of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// This is the difference between the minimum and maximum x coordinates.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Width
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getWidth(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The x maximum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double XMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getXMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setXMax(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The x minimum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double XMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getXMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setXMin(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The y maximum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double YMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getYMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setYMax(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The y minimum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double YMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getYMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setYMin(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The z maximum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double ZMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getZMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setZMax(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The z minimum value of the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> being constructed.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double ZMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EnvelopeBuilder_getZMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_EnvelopeBuilder_setZMin(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Adjusts the envelope to be within the bounds of its spatial reference's extent.
        /// </summary>
        /// <returns>
        /// An envelope object adjusted for wraparound or null if an error occurs.
        /// </returns>
        internal ArcGISEnvelope AdjustForWrapAround()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_EnvelopeBuilder_adjustForWrapAround(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Centers the envelope at the specified point.
        /// </summary>
        /// <remarks>
        /// The point's spatial reference must be the same as the envelope builder's.
        /// </remarks>
        /// <param name="point">The point to center on.</param>
        /// <since>1.0.0</since>
        public void CenterAt(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            PInvoke.RT_EnvelopeBuilder_centerAt(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Adjust the envelope's aspect ratio to match the ratio of the given width and height.
        /// </summary>
        /// <remarks>
        /// The operation preserves the center of the envelope and only increases either height or width, not both.
        /// If the new width would be greater than the old, the width is changed and the height remains the same.
        /// If the new width would be less than or equal to the old, the height is changed and the width remains the same.
        /// </remarks>
        /// <param name="width">A width.</param>
        /// <param name="height">A height.</param>
        /// <since>1.0.0</since>
        public void ChangeAspectRatio(double width, double height)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_changeAspectRatio(Handle, width, height, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Expands the envelope by the given scale factor.
        /// </summary>
        /// <remarks>
        /// A factor of less than 1.0 shrinks the envelope and greater than 1.0 expands it. The envelope's center
        /// remains unchanged, but its corners change.
        /// </remarks>
        /// <param name="factor">The factor to scale the envelope by.</param>
        /// <since>1.0.0</since>
        public void Expand(double factor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_expand(Handle, factor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Expands the envelope at the anchor point by the given factor.
        /// </summary>
        /// <remarks>
        /// A factor of less than 1.0 shrinks the envelope and greater than 1.0 expands it.
        /// </remarks>
        /// <param name="anchor">The point to anchor at.</param>
        /// <param name="factor">The factor to scale the envelope by.</param>
        /// <since>1.0.0</since>
        public void Expand(ArcGISPoint anchor, double factor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localAnchor = anchor.Handle;
            
            PInvoke.RT_EnvelopeBuilder_expandAtAnchor(Handle, localAnchor, factor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Adjusts the envelope's x-values to be within the bounds of the assigned spatial reference.
        /// </summary>
        internal void Normalize()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_normalize(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Normalizes the envelope to the passed in envelope.
        /// </summary>
        /// <param name="envelope">The envelope to normalize against.</param>
        internal void Normalize(ArcGISEnvelope envelope)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localEnvelope = envelope.Handle;
            
            PInvoke.RT_EnvelopeBuilder_normalizeToEnvelope(Handle, localEnvelope, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Moves the builder's envelope geometry by the given offsets in the x and y dimension.
        /// </summary>
        /// <param name="offsetX">The number of units to move the envelope on the x axis.</param>
        /// <param name="offsetY">The number of units to move the envelope on the y axis.</param>
        /// <since>1.0.0</since>
        public void OffsetBy(double offsetX, double offsetY)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_offsetBy(Handle, offsetX, offsetY, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Set the m-values for the envelope.
        /// </summary>
        /// <param name="mMin">The m minimum value for the envelope.</param>
        /// <param name="mMax">The m maximum value for the envelope.</param>
        /// <since>1.0.0</since>
        public void SetM(double mMin, double mMax)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_setM(Handle, mMin, mMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Set the x,y coordinates for the envelope.
        /// </summary>
        /// <param name="xMin">The x minimum value for the envelope.</param>
        /// <param name="yMin">The y minimum value for the envelope.</param>
        /// <param name="xMax">The x maximum value for the envelope.</param>
        /// <param name="yMax">The y maximum value for the envelope.</param>
        /// <since>1.0.0</since>
        public void SetXY(double xMin, double yMin, double xMax, double yMax)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_setXY(Handle, xMin, yMin, xMax, yMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Set the z-values for the envelope.
        /// </summary>
        /// <param name="zMin">The z minimum value for the envelope.</param>
        /// <param name="zMax">The z maximum value for the envelope.</param>
        /// <since>1.0.0</since>
        public void SetZ(double zMin, double zMax)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_EnvelopeBuilder_setZ(Handle, zMin, zMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Finds the union of the builder's envelope and the specified envelope and updates the envelope builder with the result.
        /// </summary>
        /// <param name="envelope">Another envelope to union with.</param>
        /// <since>1.0.0</since>
        public void Union(ArcGISEnvelope envelope)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localEnvelope = envelope.Handle;
            
            PInvoke.RT_EnvelopeBuilder_unionWithEnvelope(Handle, localEnvelope, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Finds the union of the builder's envelope and the specified point and updates the envelope builder with the result.
        /// </summary>
        /// <param name="point">A point to union with.</param>
        /// <since>1.0.0</since>
        public void Union(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            PInvoke.RT_EnvelopeBuilder_unionWithPoint(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISEnvelopeBuilder(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EnvelopeBuilder_createFromEnvelope(IntPtr envelope, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EnvelopeBuilder_createFromCenterPoint(IntPtr center, double width, double height, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EnvelopeBuilder_createFromCenterPointAndDepth(IntPtr center, double width, double height, double depth, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EnvelopeBuilder_createFromSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EnvelopeBuilder_getCenter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getDepth(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getHeight(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getMMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setMMax(IntPtr handle, double mMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getMMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setMMin(IntPtr handle, double mMin, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getWidth(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getXMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setXMax(IntPtr handle, double xMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getXMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setXMin(IntPtr handle, double xMin, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getYMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setYMax(IntPtr handle, double yMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getYMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setYMin(IntPtr handle, double yMin, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getZMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setZMax(IntPtr handle, double zMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EnvelopeBuilder_getZMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setZMin(IntPtr handle, double zMin, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EnvelopeBuilder_adjustForWrapAround(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_centerAt(IntPtr handle, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_changeAspectRatio(IntPtr handle, double width, double height, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_expand(IntPtr handle, double factor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_expandAtAnchor(IntPtr handle, IntPtr anchor, double factor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_normalize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_normalizeToEnvelope(IntPtr handle, IntPtr envelope, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_offsetBy(IntPtr handle, double offsetX, double offsetY, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setM(IntPtr handle, double mMin, double mMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setXY(IntPtr handle, double xMin, double yMin, double xMax, double yMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_setZ(IntPtr handle, double zMin, double zMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_unionWithEnvelope(IntPtr handle, IntPtr envelope, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_EnvelopeBuilder_unionWithPoint(IntPtr handle, IntPtr point, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}