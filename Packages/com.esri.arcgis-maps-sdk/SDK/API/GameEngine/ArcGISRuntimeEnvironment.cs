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

namespace Esri.GameEngine
{
    /// <summary>
    /// Provides environment settings, such as an API key for access to ArcGIS location services.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISRuntimeEnvironment
    {
        #region Properties
        /// <summary>
        /// The API key allows your app to access ArcGIS location services and private portal items.
        /// </summary>
        /// <remarks>
        /// An API key is a long-lived access token configured to authorize access to secure ArcGIS services and items. API keys are created, managed, and invalidated though an API key credentials item in an ArcGIS portal and are usually embedded directly into an application. To learn how to create and manage API keys, go to the <see cref="Create an API Key">https://developers.arcgis.com/documentation/security-and-authentication/api-key-authentication/tutorials/create-an-api-key/</see> tutorial. You must ensure that your API key has the correct privileges to access secure resources.
        /// </remarks>
        /// <since>1.0.0</since>
        public static string APIKey
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISRuntimeEnvironment_getAPIKey(errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISRuntimeEnvironment_setAPIKey(value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The object responsible for issuing authentication challenges and managing credential stores.
        /// </summary>
        /// <since>1.1.0</since>
        public static GameEngine.Authentication.ArcGISAuthenticationManager AuthenticationManager
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISRuntimeEnvironment_getAuthenticationManager(errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Authentication.ArcGISAuthenticationManager localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Authentication.ArcGISAuthenticationManager(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_ArcGISRuntimeEnvironment_setAuthenticationManager(localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Disables request chaining.
        /// </summary>
        /// <remarks>
        /// Request chaining is enabled by default. It should be disabled by clients that
        /// have switched to the global request handler.
        /// </remarks>
        internal static void DisableRequestChaining()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_disableRequestChaining(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Enables/disables breaking on exceptions.
        /// </summary>
        /// <param name="enable">true if the runtime should break on an exception, false otherwise.</param>
        internal static void EnableBreakOnException(bool enable)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_enableBreakOnException(enable, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Enables/disables memory leak detection.
        /// </summary>
        /// <remarks>
        /// Disabling will cause the runtime to dump all of the object instances that were currently being tracked and
        /// it will not track object instances from the point of disabling.  Enabling leak detection will make the
        /// runtime track all object instances allocated from the point of enabling.  By default, leak detection is turned off.
        /// </remarks>
        /// <param name="enable">true if the runtime should be tracking object allocations and deallocations, false otherwise.</param>
        internal static void EnableLeakDetection(bool enable)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_enableLeakDetection(enable, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Enables/disables raising assertion.
        /// </summary>
        /// <remarks>
        /// This is enabled by default in debug builds and disabled by default in release builds.
        /// If disable abort will not be called.
        /// </remarks>
        /// <param name="enable">true if the runtime should turn assertions on and abort, false otherwise.</param>
        internal static void EnableRaiseAssertion(bool enable)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_enableRaiseAssertion(enable, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Enables/disables the assert/abort dialog on Windows Desktop.
        /// </summary>
        /// <param name="enable">true the assert/abort dialog should appear with abort, break and continue options. enable false if all asserts and errors should go to the debug console.</param>
        internal static void EnableShowAssertDialog(bool enable)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_enableShowAssertDialog(enable, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Get the path of the directory containing the feature toggle file.
        /// </summary>
        /// <remarks>
        /// Returns the path of the directory used to find the feature toggle file 'arcgis_runtime_feature_set.txt'.
        /// This directory is set by calling <see cref="GameEngine.ArcGISRuntimeEnvironment.SetFeatureToggleDirectory">ArcGISRuntimeEnvironment.SetFeatureToggleDirectory</see>
        /// 
        /// Additionally when <see cref="GameEngine.ArcGISRuntimeEnvironment.SetInstallDirectory">ArcGISRuntimeEnvironment.SetInstallDirectory</see> is called, if the feature toggle directory has not already been set,
        /// then it is set to the install directory.
        /// 
        /// The feature toggle file is plain text. Each line contains the name of a feature (no spaces) and '=' a boolean value.
        /// For example:
        /// <code>
        /// enable_rendering_engine_mr3d=true
        /// </code>
        /// </remarks>
        /// <returns>
        /// The location of the directory containing the feature toggle file.
        /// </returns>
        internal static string GetFeatureToggleDirectory()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISRuntimeEnvironment_getFeatureToggleDirectory(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        
        /// <summary>
        /// Indicates if a specified feature is enabled in the feature toggle file.
        /// </summary>
        /// <remarks>
        /// If the feature toggle file contains the specified name then toggle value is returned.
        /// Otherwise false is returned where the file or toggle does not exist.
        /// See <see cref="GameEngine.ArcGISRuntimeEnvironment.SetFeatureToggleDirectory">ArcGISRuntimeEnvironment.SetFeatureToggleDirectory</see> and <see cref="GameEngine.ArcGISRuntimeEnvironment.GetFeatureToggleDirectory">ArcGISRuntimeEnvironment.GetFeatureToggleDirectory</see>.
        /// </remarks>
        /// <param name="featureToggle">The name of the feature in the feature toggle file.</param>
        /// <returns>
        /// The location of the directory containing the feature toggle file.
        /// </returns>
        internal static bool IsFeatureEnabled(string featureToggle)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISRuntimeEnvironment_isFeatureEnabled(featureToggle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// This will override the license watermark text with the beta text info.
        /// </summary>
        /// <remarks>
        /// * 'true' the water mark will always appear even if you set a license
        /// * 'false' (default) the water mark will not appear. The developer license will appear if a license is not set.
        /// </remarks>
        /// <param name="set">Set to true if you wish the beta watermark to appear, false if you wish the license level text to appear.</param>
        internal static void SetBetaWatermark(bool set)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setBetaWatermark(set, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the location of the directory containing the feature toggle file and reads the content of the file.
        /// </summary>
        /// <remarks>
        /// Sets the directory containing the feature toggle file 'arcgis_runtime_feature_set.txt'.
        /// If the file exists it will be read into memory. If the file is not formatted an error is returned, for example if a toggle is missing an '='.
        /// The feature toggles are used to enable or disable features that are tested <see cref="GameEngine.ArcGISRuntimeEnvironment.IsFeatureEnabled">ArcGISRuntimeEnvironment.IsFeatureEnabled</see>.
        /// 
        /// If the directory or file does not exist, no error is returned and tests for features will return false.
        /// This function should be called once at the start of runtime initialization.
        /// 
        /// Additionally when <see cref="GameEngine.ArcGISRuntimeEnvironment.SetInstallDirectory">ArcGISRuntimeEnvironment.SetInstallDirectory</see> is called, if the feature toggle directory has not already been set,
        /// then it is set to the install directory.
        /// 
        /// The feature toggle file is plain text. Each line contains the name of a feature (no spaces) and '=' a boolean value.
        /// For example:
        /// <code>
        /// enable_rendering_engine_mr3d=true
        /// </code>
        /// </remarks>
        /// <param name="featureToggleDirectory">The path to the directory containing the feature toggle file.</param>
        internal static void SetFeatureToggleDirectory(string featureToggleDirectory)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setFeatureToggleDirectory(featureToggleDirectory, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Set the location of the root folder for the deployment resources.
        /// </summary>
        /// <remarks>
        /// This is used for the default location to find file resources as follows:
        /// - DirectX shaders default location.
        /// - <i><b>install_path</b></i>/resources/shaders
        /// - military dictionary symbol style default location
        /// - <i><b>install_path</b></i>/resources/symbols/mil2525c
        /// - navigation localized resources
        /// - <i><b>install_path</b></i>/resources/navigation
        /// </remarks>
        /// <param name="installPath">The path to the root folder of the deployment.</param>
        internal static void SetInstallDirectory(string installPath)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setInstallDirectory(installPath, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Set the product name and version.
        /// </summary>
        /// <remarks>
        /// Sets the product information to be used to build the user-agent string.
        /// This should be called before the runtime environment is created. Calling it after may have no effect.
        /// The values are global to the process and defaults to an empty string.
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="version"></param>
        internal static void SetProductInfo(string name, string version)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setProductInfo(name, version, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the resources directory for the process.
        /// </summary>
        /// <remarks>
        /// If not set, it will default to "<install_directory>\resources\".
        /// </remarks>
        /// <param name="resourcesPath">Full pathname of the resources directory.</param>
        internal static void SetResourcesDirectory(string resourcesPath)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setResourcesDirectory(resourcesPath, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Set the runtime client type and version.
        /// </summary>
        /// <remarks>
        /// This is to support a specific use case. The Unity game engine requires a different thread pool implementation and
        /// this allows to identify the runtime client and instantiate the right thread pool implementation at startup.
        /// This should be called before the runtime environment is created. Calling it after that has no effect.
        /// The values are global to the process and default to <see cref="Standard.ArcGISRuntimeClient.None">ArcGISRuntimeClient.None</see> and an empty string respectively when not set.
        /// Both values are also used to build the user-agent string.
        /// </remarks>
        /// <param name="runtimeClient"></param>
        /// <param name="version"></param>
        internal static void SetRuntimeClient(Standard.ArcGISRuntimeClient runtimeClient, string version)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setRuntimeClient(runtimeClient, version, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the temp directory for the process.
        /// </summary>
        /// <param name="tempPath">Full pathname of the temporary file.</param>
        internal static void SetTempDirectory(string tempPath)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISRuntimeEnvironment_setTempDirectory(tempPath, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Events
        /// <summary>
        /// Set a global error handler.
        /// </summary>
        /// <remarks>
        /// The global error handler can be overridden by a function error handler. At least one must be set.
        /// An exception will be thrown if which will cause a crash if the error handler has not been
        /// set globally or per function call.
        /// </remarks>
        /// <seealso cref="Unity.ArcGISErrorHandler">ArcGISErrorHandler</seealso>
        internal static ArcGISRuntimeEnvironmentErrorEvent Error
        {
            get
            {
                return _errorHandler.Delegate;
            }
            set
            {
                if (_errorHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _errorHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISRuntimeEnvironment_setErrorCallback(ArcGISRuntimeEnvironmentErrorEventHandler.HandlerFunction, _errorHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISRuntimeEnvironment_setErrorCallback(null, _errorHandler.UserData, errorHandler);
                    
                    _errorHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal static ArcGISRuntimeEnvironmentErrorEventHandler _errorHandler = new ArcGISRuntimeEnvironmentErrorEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISRuntimeEnvironment_getAPIKey(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setAPIKey([MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISRuntimeEnvironment_getAuthenticationManager(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setAuthenticationManager(IntPtr authenticationManager, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_disableRequestChaining(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_enableBreakOnException([MarshalAs(UnmanagedType.I1)]bool enable, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_enableLeakDetection([MarshalAs(UnmanagedType.I1)]bool enable, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_enableRaiseAssertion([MarshalAs(UnmanagedType.I1)]bool enable, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_enableShowAssertDialog([MarshalAs(UnmanagedType.I1)]bool enable, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISRuntimeEnvironment_getFeatureToggleDirectory(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ArcGISRuntimeEnvironment_isFeatureEnabled([MarshalAs(UnmanagedType.LPStr)]string featureToggle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setBetaWatermark([MarshalAs(UnmanagedType.I1)]bool set, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setFeatureToggleDirectory([MarshalAs(UnmanagedType.LPStr)]string featureToggleDirectory, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setInstallDirectory([MarshalAs(UnmanagedType.LPStr)]string installPath, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setProductInfo([MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string version, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setResourcesDirectory([MarshalAs(UnmanagedType.LPStr)]string resourcesPath, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setRuntimeClient(Standard.ArcGISRuntimeClient runtimeClient, [MarshalAs(UnmanagedType.LPStr)]string version, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setTempDirectory([MarshalAs(UnmanagedType.LPStr)]string tempPath, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISRuntimeEnvironment_setErrorCallback(ArcGISRuntimeEnvironmentErrorEventInternal error, IntPtr userData, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}