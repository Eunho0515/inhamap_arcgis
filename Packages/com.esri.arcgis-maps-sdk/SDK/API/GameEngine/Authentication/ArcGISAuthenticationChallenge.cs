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

namespace Esri.GameEngine.Authentication
{
    /// <summary>
    /// An ArcGIS authentication challenge.
    /// </summary>
    /// <remarks>
    /// An ArcGIS authentication challenge is raised by the <see cref="GameEngine.Authentication.ArcGISAuthenticationChallengeHandler">ArcGISAuthenticationChallengeHandler</see> if an ArcGIS secured resource
    /// requires OAuth or ArcGIS Token authentication.
    /// </remarks>
    /// <since>1.1.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISAuthenticationChallenge
    {
        #region Properties
        /// <summary>
        /// The underlying error that led to this authentication challenge.
        /// </summary>
        /// <since>1.1.0</since>
        public Exception Error
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISAuthenticationChallenge_getError(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISError(new Standard.ArcGISError(localResult));
            }
        }
        
        /// <summary>
        /// The number of failed authentication attempts that occurred previously for this authentication challenge.
        /// </summary>
        /// <remarks>
        /// The maximum number of allowed authentication attempts is `5`. After reaching this limit,
        /// the request that led to this authentication challenge may fail with an error.
        /// </remarks>
        /// <since>2.1.0</since>
        public byte PreviousFailureCount
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISAuthenticationChallenge_getPreviousFailureCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The URL of the request that led to this authentication challenge.
        /// </summary>
        /// <since>1.1.0</since>
        public string RequestURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISAuthenticationChallenge_getRequestURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The type of the challenge, indicating which type of <see cref="GameEngine.Authentication.ArcGISCredential">ArcGISCredential</see> should be created to handle it.
        /// </summary>
        /// <since>2.1.0</since>
        public ArcGISAuthenticationChallengeType Type
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISAuthenticationChallenge_getType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Cancels the request that initiated the challenge.
        /// </summary>
        /// <since>1.1.0</since>
        public void Cancel()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISAuthenticationChallenge_cancel(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Handles the challenge without a credential, causing it to fail with the original authentication error.
        /// </summary>
        /// <since>1.1.0</since>
        public void ContinueAndFail()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISAuthenticationChallenge_continueAndFail(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Handles the challenge with the specified credential.
        /// </summary>
        /// <param name="credential">The credential to use when retrying the request.</param>
        /// <since>1.1.0</since>
        public void ContinueWithCredential(ArcGISCredential credential)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCredential = credential.Handle;
            
            PInvoke.RT_ArcGISAuthenticationChallenge_continueWithCredential(Handle, localCredential, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Handles the challenge with an error that occurred at the platform API level while trying to
        /// generate a credential. If the error is not an authentication error, the request that issued the
        /// authentication challenge fails with the given error. If the error is an authentication error,
        /// the request's retry count is incremented and a new challenge is issued.
        /// </summary>
        /// <param name="platformAPIError">The platform API error that was encountered during the challenge.</param>
        /// <param name="isArcGISAuthenticationError">Indicates whether the error is an ArcGIS authentication error.</param>
        internal void ContinueWithPlatformAPIError(Standard.ArcGISClientReference platformAPIError, bool isArcGISAuthenticationError)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPlatformAPIError = platformAPIError.Handle;
            
            PInvoke.RT_ArcGISAuthenticationChallenge_continueWithPlatformAPIError(Handle, localPlatformAPIError, isArcGISAuthenticationError, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Returns a boolean indicating if this challenge is a match for another challenge for the
        /// purpose of sharing the result returned from a challenge handler.
        /// </summary>
        /// <param name="other">The other <see cref="GameEngine.Authentication.ArcGISAuthenticationChallenge">ArcGISAuthenticationChallenge</see> to test for a match.</param>
        /// <returns>
        /// True if the challenge is a match, false otherwise.
        /// </returns>
        internal bool IsMatch(ArcGISAuthenticationChallenge other)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localOther = other.Handle;
            
            var localResult = PInvoke.RT_ArcGISAuthenticationChallenge_isMatch(Handle, localOther, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISAuthenticationChallenge(IntPtr handle) => Handle = handle;
        
        ~ArcGISAuthenticationChallenge()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISAuthenticationChallenge_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISAuthenticationChallenge other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISAuthenticationChallenge_getError(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern byte RT_ArcGISAuthenticationChallenge_getPreviousFailureCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISAuthenticationChallenge_getRequestURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISAuthenticationChallengeType RT_ArcGISAuthenticationChallenge_getType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISAuthenticationChallenge_cancel(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISAuthenticationChallenge_continueAndFail(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISAuthenticationChallenge_continueWithCredential(IntPtr handle, IntPtr credential, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISAuthenticationChallenge_continueWithPlatformAPIError(IntPtr handle, IntPtr platformAPIError, [MarshalAs(UnmanagedType.I1)]bool isArcGISAuthenticationError, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ArcGISAuthenticationChallenge_isMatch(IntPtr handle, IntPtr other, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISAuthenticationChallenge_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}