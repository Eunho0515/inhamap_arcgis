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

namespace Esri.GameEngine.RCQ
{
    /// <summary>
    /// Stream render commands to the game engine.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISRenderCommandServer
    {
        #region Methods
        /// <summary>
        /// Get an object with the render command cache since the last time the method was called.
        /// </summary>
        /// <remarks>
        /// The DataBuffer contains the render commands serialized as a consecutive array of [type_of_command, render_command_parameters].
        /// Since the length of each render command is different, the de-serialization has to be done dynamically, reading the commands one by one
        /// and dynamically advancing the pointer to the DataBuffer.
        /// </remarks>
        /// <returns>
        /// An object with the render command cache since the last time the method was called.
        /// </returns>
        internal Unity.ArcGISDataBuffer<byte> GetRenderCommands()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_RenderCommandServer_getRenderCommands(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISDataBuffer<byte> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISDataBuffer<byte>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// The game engine must call this method to notify the runtimecore when created Renderables are drawable (i.e. their GPU resources have been created).
        /// The runtimecore uses this information when determining what Renderables to make visible.
        /// </summary>
        /// <param name="callbackTokens">List of Renderables that have GPU resources, identified by their uint64 callbackToken values.</param>
        internal void NotifyRenderableHasGPUResources(global::Unity.Collections.NativeArray<byte> callbackTokens)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCallbackTokens = Unity.Convert.ToArcGISByteArrayStruct(callbackTokens);
            
            PInvoke.RT_RenderCommandServer_notifyRenderableHasGPUResources(Handle, localCallbackTokens, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// The game engine must call this method to notify the runtimecore when a render command group has been processed.
        /// The runtimecore uses this information to determine if there is still commands to be processed.
        /// </summary>
        internal void NotifyRenderCommandsProcessed()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_RenderCommandServer_notifyRenderCommandsProcessed(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISRenderCommandServer(IntPtr handle) => Handle = handle;
        
        ~ArcGISRenderCommandServer()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_RenderCommandServer_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISRenderCommandServer other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_RenderCommandServer_getRenderCommands(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_RenderCommandServer_notifyRenderableHasGPUResources(IntPtr handle, Standard.ArcGISIntermediateByteArrayStruct callbackTokens, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_RenderCommandServer_notifyRenderCommandsProcessed(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_RenderCommandServer_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}