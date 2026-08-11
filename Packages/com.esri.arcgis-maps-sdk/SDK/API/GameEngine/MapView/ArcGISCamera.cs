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
    /// A camera represents an observer's location and their perspective of an <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>. It is used as a data loading point.
    /// </summary>
    /// <remarks>
    /// A <see cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</see> object can be thought of as a camera that you look through to see a viewable area of a scene. What
    /// you see depends on how you orientate the camera and how far it is above the <see cref="">Surface</see>. A camera has four main
    /// configurable properties:
    /// 
    /// The camera orientation uses a local right-handed XYZ coordinate system aligned to East, North, Up (ENU):
    /// X = East, Y = North, Z = Up.
    /// 
    /// * Location - The 3D point in space where the camera is located. The camera will adopt the spatial reference of
    ///   the point at the time of creation. If a spatial reference is not supplied, a spatial reference having a WGS84
    ///   horizontal coordinate system is assumed. When no vertical coordinate system is provided, a point's z value is
    ///   inferred based on the horizontal coordinate system's unit, interpreted as a height-based value relative to
    ///   the horizontal coordinate system's surface:
    ///     * Projected coordinate systems use the unit of the horizontal coordinate system.
    ///     * Geographic coordinate systems use meters.
    /// * Heading - The angle around the z-axis at which the camera is rotated. The angle is clockwise from north in the East,
    ///   North, Up (ENU) ground reference frame. The value is between 0 and 360, where 0 is looking North and 90 is looking
    ///   East.
    /// * Pitch - The angle around the x-axis at which the camera is rotated. The value is between 0 and 180, where 0 is
    ///   looking straight down, 90 is looking towards the horizon, and 180 is looking straight up.
    /// * Roll - The angle around the y-axis (the direction of sight) at which the camera is rotated. Positive values rotate
    ///   counterclockwise in view space (for example, when looking north, 90 means the top of the screen points west).
    ///   The value is between 0 and 360, where 0 is horizontal and 180 is upside down.
    /// 
    /// You can construct a <see cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</see> based on these values or you can obtain it from the scene view's current
    /// viewpoint using <see cref="">SceneView.getCurrentViewpointCamera()</see>. The <see cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</see> is immutable which means that
    /// once it is created it you cannot modify it.
    /// 
    /// You can define the user's camera interactions using the scene view's camera controller. The default camera
    /// controller (<see cref="">GlobeCameraController</see>) allows users to freely move and focus the camera anywhere in the scene.
    /// Other camera controllers provide specialized behavior, such as:
    /// 
    /// * The <see cref="">OrbitGeoElementCameraController</see> locks the camera to maintain focus on a (possibly moving) <see cref="GameEngine.Map.ArcGISGeoElement">ArcGISGeoElement</see>.
    /// * The <see cref="">OrbitLocationCameraController</see> locks the camera to orbit and to maintain focus on a fixed location.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0659, CS0661
    public partial class ArcGISCamera
    #pragma warning restore CS0661, CS0659
    {
        #region Constructors
        /// <summary>
        /// Creates a camera with the specified location, heading, pitch, and roll. The camera's spatial reference is derived from the locationPoint.
        /// </summary>
        /// <param name="locationPoint">A point geometry defining the 3D location at which to place the camera. If the point has a spatial reference, the camera will adopt it. Otherwise, a spatial reference of WGS84 is assumed. If the point's z value is below the <see cref="">Scene.baseSurface</see> and the <see cref="">Surface.navigationConstraint</see> is set to <see cref="">NavigationConstraint.stayAbove</see>, the camera will be positioned at the <see cref="">Surface</see>. Note that the default is <see cref="">NavigationConstraint.stayAbove</see>.</param>
        /// <param name="heading">The angle around the z-axis at which the camera is rotated. The angle is clockwise from north in the East, North, Up (ENU) ground reference frame. The value is between 0 and 360, where 0 is looking North and 90 is looking East. Values are wrapped modulo 360 (for example, 370 becomes 10).</param>
        /// <param name="pitch">The angle around the x-axis at which the camera is rotated in the East, North, Up (ENU) ground reference frame. The value is between 0 and 180, where 0 is looking straight down, 90 is looking towards the horizon, and 180 is looking straight up. Pitch values are clamped to this range: a negative value defaults to 0 and a value greater than 180 defaults to 180. If the behavior of a negative pitch is required, then the corresponding transformation with positive pitch can be set instead. For example, if heading:0 pitch:-20 roll:0 is required then heading:180 pitch:20 roll:180 can be used instead.</param>
        /// <param name="roll">The angle around the y-axis (the direction of sight) at which the camera is rotated in the East, North, Up (ENU) ground reference frame. Positive values rotate counterclockwise in view space (for example, when looking north, 90 means the top of the screen points west). The value is between 0 and 360, where 0 is horizontal and 180 is upside down. Values are wrapped modulo 360 (for example, 370 becomes 10).</param>
        /// <since>1.0.0</since>
        public ArcGISCamera(GameEngine.Geometry.ArcGISPoint locationPoint, double heading, double pitch, double roll)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLocationPoint = locationPoint.Handle;
            
            Handle = PInvoke.RT_Camera_createWithLocationHeadingPitchRoll(localLocationPoint, heading, pitch, roll, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The heading of the camera.
        /// </summary>
        /// <remarks>
        /// The angle around the z-axis at which the camera is rotated. The angle is clockwise from north in the East,
        /// North, Up (ENU) ground reference frame. The value is between 0 and 360, where 0 is looking North and 90 is
        /// looking East.
        /// </remarks>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public double Heading
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Camera_getHeading(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The point geometry containing the location and altitude of the camera.
        /// </summary>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISPoint Location
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Camera_getLocation(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The pitch of the camera.
        /// </summary>
        /// <remarks>
        /// The angle around the x-axis at which the camera is rotated in the East, North, Up (ENU) ground reference frame.
        /// The value is between 0 and 180, where 0 is looking straight down, 90 is looking towards the horizon, and 180 is
        /// looking straight up.
        /// </remarks>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public double Pitch
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Camera_getPitch(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The roll of the camera.
        /// </summary>
        /// <remarks>
        /// The angle around the y-axis (the direction of sight) at which the camera is rotated in the East, North, Up (ENU)
        /// ground reference frame. Positive values rotate counterclockwise in view space (for example, when looking
        /// north, 90 means the top of the screen points west). The value is between 0 and 360, where 0 is horizontal and
        /// 180 is upside down.
        /// </remarks>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public double Roll
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Camera_getRoll(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates a copy of the camera with the specified change in altitude applied.
        /// </summary>
        /// <param name="deltaAltitude">The altitude delta in meters to apply to the output camera.</param>
        /// <returns>
        /// A copy of the camera with an elevation delta adjusted by the parameter delta_altitude.
        /// </returns>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public ArcGISCamera Elevate(double deltaAltitude)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Camera_elevate(Handle, deltaAltitude, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISCamera localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISCamera(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a copy of the camera with a new location.
        /// </summary>
        /// <param name="location">The location to move the output camera to. The returned camera is in the same spatial reference as the source camera. If the spatial reference of the point is not specified, it is assumed to be the spatial reference of the camera.</param>
        /// <returns>
        /// A copy of the camera with the location changed.
        /// </returns>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public ArcGISCamera MoveTo(GameEngine.Geometry.ArcGISPoint location)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLocation = location.Handle;
            
            var localResult = PInvoke.RT_Camera_moveTo(Handle, localLocation, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISCamera localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISCamera(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a copy of the camera with the specified heading, pitch and roll values.
        /// </summary>
        /// <param name="heading">The angle around the z-axis at which the new camera is rotated. The angle is clockwise from north in the East, North, Up (ENU) ground reference frame. The value is between 0 and 360, where 0 is looking North and 90 is looking East. Values are wrapped modulo 360 (for example, 370 becomes 10).</param>
        /// <param name="pitch">The angle around the x-axis at which the new camera is rotated in the East, North, Up (ENU) ground reference frame. The value is between 0 and 180, where 0 is looking straight down, 90 is looking towards the horizon, and 180 is looking straight up. Pitch values are clamped to this range: a negative value defaults to 0 and a value greater than 180 defaults to 180. If the behavior of a negative pitch is required, then the corresponding transformation with positive pitch can be set instead. For example, if heading:0 pitch:-20 roll:0 is required then heading:180 pitch:20 roll:180 can be used instead.</param>
        /// <param name="roll">The angle around the y-axis (the direction of sight) at which the new camera is rotated in the East, North, Up (ENU) ground reference frame. Positive values rotate counterclockwise in view space (for example, when looking north, 90 means the top of the screen points west). The value is between 0 and 360, where 0 is horizontal and 180 is upside down. Values are wrapped modulo 360 (for example, 370 becomes 10).</param>
        /// <returns>
        /// A copy of the camera with the position moved
        /// </returns>
        /// <seealso cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</seealso>
        /// <since>1.0.0</since>
        public ArcGISCamera RotateTo(double heading, double pitch, double roll)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Camera_rotateTo(Handle, heading, pitch, roll, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISCamera localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISCamera(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISCamera(IntPtr handle) => Handle = handle;
        
        ~ArcGISCamera()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Camera_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            var otherCamera = obj as ArcGISCamera;
            
            if (otherCamera == null)
            {
                return false;
            }
            
            var localOtherCamera = otherCamera.Handle;
            
            if (Handle == localOtherCamera)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Camera_equals(Handle, localOtherCamera, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public static implicit operator bool(ArcGISCamera other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Camera_createWithLocationHeadingPitchRoll(IntPtr locationPoint, double heading, double pitch, double roll, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Camera_getHeading(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Camera_getLocation(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Camera_getPitch(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Camera_getRoll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Camera_elevate(IntPtr handle, double deltaAltitude, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Camera_moveTo(IntPtr handle, IntPtr location, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Camera_rotateTo(IntPtr handle, double heading, double pitch, double roll, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Camera_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Camera_equals(IntPtr handle, IntPtr otherCamera, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}