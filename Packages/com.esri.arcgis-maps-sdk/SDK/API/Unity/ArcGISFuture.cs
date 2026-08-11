// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
using System;
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	/// <summary>
	/// A <see cref="ArcGISFuture">ArcGISFuture</see> represents work that can be completed asynchronously and concurrently with other work.
	/// </summary>
	/// <remarks>
	/// A <see cref="ArcGISFuture">ArcGISFuture</see> can be either executing or complete as indicated by <see cref="IsDone">ArcGISFuture.IsDone</see>.
	/// Notification of completion is available through the callback <see cref="Standard.ArcGISFutureCompletedEvent">ArcGISFutureCompletedEvent</see>.
	///
	/// When complete, the <see cref="ArcGISFuture">ArcGISFuture</see> will have either completed successfully or failed with an error
	/// during its execution. A successfully completed <see cref="ArcGISFuture">ArcGISFuture</see> returns null from <see cref="GetError">ArcGISFuture.GetError</see>
	/// and the result can be obtained from <see cref="Get">ArcGISFuture.Get</see>. Whereas a failed <see cref="ArcGISFuture">ArcGISFuture</see> returns an <see cref="Standard.ArcGISError">ArcGISError</see>,
	/// including if the <see cref="ArcGISFuture">ArcGISFuture</see> was canceled.
	///
	/// Successfully completed <see cref="ArcGISFuture">ArcGISFuture</see> instances may return a result to the caller. The result is accessed by
	/// calling <see cref="Get">ArcGISFuture.Get</see>. If there is no result an empty <see cref="">Element</see> is returned.
	///
	/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is executing and <see cref="Get">ArcGISFuture.Get</see> or <see cref="Wait">ArcGISFuture.Wait</see> is called the
	/// thread will be blocked until the completion of the <see cref="ArcGISFuture">ArcGISFuture</see>. Once the <see cref="ArcGISFuture">ArcGISFuture</see> is complete
	/// subsequent calls will no longer block. If the <see cref="ArcGISFuture">ArcGISFuture</see> fails, but not canceled, both
	/// <see cref="Get">ArcGISFuture.Get</see> or <see cref="Wait">ArcGISFuture.Wait</see> will result in an error. A cancelled <see cref="ArcGISFuture">ArcGISFuture</see> will return a null
	/// from <see cref="Get">ArcGISFuture.Get</see> and <see cref="ArcGISFutureStatus.Canceled">ArcGISFutureStatus.Canceled</see> from <see cref="Wait">ArcGISFuture.Wait</see>.
	///
	/// To avoid blocking calling threads with either <see cref="Get">ArcGISFuture.Get</see> or <see cref="Wait">ArcGISFuture.Wait</see>, it is recommended to
	/// use the <see cref="Standard.ArcGISFutureCompletedEvent">ArcGISFutureCompletedEvent</see> to receive a completion notification before checking <see cref="GetError">ArcGISFuture.GetError</see>
	/// for errors and then calling <see cref="Get">ArcGISFuture.Get</see> to access the return value.
	///
	/// An executing <see cref="ArcGISFuture">ArcGISFuture</see> can be requested to cancel by calling <see cref="Cancel">ArcGISFuture.Cancel</see>. If the cancellation is
	/// observed by the executing code, then the <see cref="ArcGISFuture">ArcGISFuture</see> returns an <see cref="Standard.ArcGISError">ArcGISError</see> from <see cref="GetError">ArcGISFuture.GetError</see>.
	/// </remarks>
	/// <since>1.0.0</since>
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISFuture
	{
		#region Methods
		/// <summary>
		/// Request cancellation of the <see cref="ArcGISFuture">ArcGISFuture</see>.
		/// </summary>
		/// <remarks>
		/// Cancellation is a cooperative mechanism. Cancel can be called on the <see cref="ArcGISFuture">ArcGISFuture</see>, but the
		/// executing code must also periodically check for cancellation and terminate. It is possible
		/// that a canceled <see cref="ArcGISFuture">ArcGISFuture</see> could still complete successfully, if cancel was called after
		/// the executing code checked for cancellation.
		/// Once cancel is called, <see cref="IsCanceled">ArcGISFuture.IsCanceled</see> will return true.
		/// If the executing code observed the cancellation, the following will apply:
		/// * <see cref="GetError">ArcGISFuture.GetError</see> returns an error indicating cancellation
		/// * <see cref="Get">ArcGISFuture.Get</see> returns null
		/// * <see cref="Wait">ArcGISFuture.Wait</see> returns <see cref="ArcGISFutureStatus.Canceled">ArcGISFutureStatus.Canceled</see>
		/// </remarks>
		/// <returns>
		/// true if the <see cref="ArcGISFuture">ArcGISFuture</see> was requested to cancel, false if the <see cref="ArcGISFuture">ArcGISFuture</see> had already been
		/// requested to cancel.
		/// Returns false if an error occurs.
		/// </returns>
		/// <since>1.0.0</since>
		public bool Cancel()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_cancel(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Returns the result of the <see cref="ArcGISFuture">ArcGISFuture</see>.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is successful then <see cref="Get">ArcGISFuture.Get</see> will return the result. For a
		/// <see cref="ArcGISFuture">ArcGISFuture</see> which is successful but has no result then an empty <see cref="">Element</see> is returned.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> has failed during execution, the call to <see cref="Get">ArcGISFuture.Get</see> will result
		/// in an error.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is not complete, a call to <see cref="Get">ArcGISFuture.Get</see> will block the calling
		/// thread until the <see cref="ArcGISFuture">ArcGISFuture</see> completes execution.
		/// </remarks>
		/// <returns>
		/// The result of the <see cref="ArcGISFuture">ArcGISFuture</see> or null if the <see cref="ArcGISFuture">ArcGISFuture</see> was canceled.
		/// </returns>
		/// <since>1.0.0</since>
		public void Get()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			PInvoke.RT_Task_get(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);
		}

		/// <summary>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is executing, or has completed successfully, a null is returned. If the <see cref="ArcGISFuture">ArcGISFuture</see> has failed returns the error.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is executing, or completed successfully null is returned. For a
		/// completed but failed <see cref="ArcGISFuture">ArcGISFuture</see> the failure is returned in an <see cref="Standard.ArcGISError">ArcGISError</see>.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> terminated due to a cancellation request, this is also a failure and returns an error.
		/// </remarks>
		/// <returns>
		/// Returns the <see cref="Standard.ArcGISError">ArcGISError</see> instance or null.
		/// </returns>
		/// <since>1.0.0</since>
		public Exception GetError()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_getError(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return Convert.FromArcGISError(new Standard.ArcGISError(localResult));
		}

		/// <summary>
		/// Indicates if the <see cref="ArcGISFuture">ArcGISFuture</see> has been requested to cancel.
		/// </summary>
		/// <returns>
		/// true if the <see cref="ArcGISFuture">ArcGISFuture</see> has been requested to cancel or false otherwise.
		/// </returns>
		/// <since>1.0.0</since>
		public bool IsCanceled()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_isCanceled(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Indicates if the <see cref="ArcGISFuture">ArcGISFuture</see> has completed execution.
		/// </summary>
		/// <returns>
		/// true if the <see cref="ArcGISFuture">ArcGISFuture</see> has completed, false otherwise.
		/// </returns>
		/// <since>1.0.0</since>
		public bool IsDone()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_isDone(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Waits for the <see cref="ArcGISFuture">ArcGISFuture</see> to complete.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is successful or canceled then <see cref="Wait">ArcGISFuture.Wait</see> will return the
		/// <see cref="ArcGISFutureStatus">ArcGISFutureStatus</see>.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> has failed during execution, the call to <see cref="Wait">ArcGISFuture.Wait</see> will
		/// result in an error.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is not complete, a call to <see cref="Wait">ArcGISFuture.Wait</see> will block the calling
		/// thread until the <see cref="ArcGISFuture">ArcGISFuture</see> completes execution.
		/// </remarks>
		/// <returns>
		/// The <see cref="ArcGISFutureStatus">ArcGISFutureStatus</see>. Returns <see cref="ArcGISFutureStatus.Unknown">ArcGISFutureStatus.Unknown</see> if an error occurs.
		/// </returns>
		/// <since>1.0.0</since>
		public ArcGISFutureStatus Wait()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_wait(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}
		#endregion // Methods

		#region Events
		/// <summary>
		/// Sets the function that will be called when the <see cref="ArcGISFuture">ArcGISFuture</see> is completed.
		/// </summary>
		/// <remarks>
		/// When the <see cref="ArcGISFuture">ArcGISFuture</see> completes then <see cref="IsDone">ArcGISFuture.IsDone</see> is true and this callback
		/// will be called.
		///
		/// Setting this callback after <see cref="ArcGISFuture">ArcGISFuture</see> has completed will immediately call the
		/// callback.
		///
		/// Setting the callback to null after it has already been set will stop the function
		/// from being called.
		/// </remarks>
		/// <since>1.0.0</since>
		public Standard.ArcGISFutureCompletedEvent TaskCompleted
		{
			get
			{
				return _taskCompletedHandler.Delegate;
			}
			set
			{
				if (_taskCompletedHandler.Delegate == value)
				{
					return;
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				if (value != null)
				{
					_taskCompletedHandler.Delegate = value;

					PInvoke.RT_Task_setTaskCompletedCallback(Handle, Standard.ArcGISFutureCompletedEventHandler.HandlerFunction, _taskCompletedHandler.UserData, errorHandler);
				}
				else
				{
					PInvoke.RT_Task_setTaskCompletedCallback(Handle, null, _taskCompletedHandler.UserData, errorHandler);

					_taskCompletedHandler.Dispose();
				}

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}
		#endregion // Events

		#region Internal Members
		internal ArcGISFuture(IntPtr handle) => Handle = handle;

		~ArcGISFuture()
		{
			if (Handle != IntPtr.Zero)
			{
				if (_taskCompletedHandler.Delegate != null)
				{
					PInvoke.RT_Task_setTaskCompletedCallback(Handle, null, _taskCompletedHandler.UserData, IntPtr.Zero);

					_taskCompletedHandler.Dispose();
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_Task_destroy(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		internal IntPtr Handle { get; set; }

		internal Standard.ArcGISFutureCompletedEventHandler _taskCompletedHandler = new();
		#endregion // Internal Members
	}

	/// <summary>
	/// A <see cref="ArcGISFuture">ArcGISFuture</see> represents work that can be completed asynchronously and concurrently with other work.
	/// </summary>
	/// <remarks>
	/// A <see cref="ArcGISFuture">ArcGISFuture</see> can be either executing or complete as indicated by <see cref="ArcGISFuture.IsDone">ArcGISFuture.IsDone</see>.
	/// Notification of completion is available through the callback <see cref="Standard.ArcGISFutureCompletedEvent">ArcGISFutureCompletedEvent</see>.
	///
	/// When complete, the <see cref="ArcGISFuture">ArcGISFuture</see> will have either completed successfully or failed with an error
	/// during its execution. A successfully completed <see cref="ArcGISFuture">ArcGISFuture</see> returns null from <see cref="ArcGISFuture.GetError">ArcGISFuture.GetError</see>
	/// and the result can be obtained from <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see>. Whereas a failed <see cref="ArcGISFuture">ArcGISFuture</see> returns an <see cref="Standard.ArcGISError">ArcGISError</see>,
	/// including if the <see cref="ArcGISFuture">ArcGISFuture</see> was canceled.
	///
	/// Successfully completed <see cref="ArcGISFuture">ArcGISFuture</see> instances may return a result to the caller. The result is accessed by
	/// calling <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see>. If there is no result an empty <see cref="">Element</see> is returned.
	///
	/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is executing and <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> or <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see> is called the
	/// thread will be blocked until the completion of the <see cref="ArcGISFuture">ArcGISFuture</see>. Once the <see cref="ArcGISFuture">ArcGISFuture</see> is complete
	/// subsequent calls will no longer block. If the <see cref="ArcGISFuture">ArcGISFuture</see> fails, but not canceled, both
	/// <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> or <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see> will result in an error. A cancelled <see cref="ArcGISFuture">ArcGISFuture</see> will return a null
	/// from <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> and <see cref="ArcGISFutureStatus.Canceled">ArcGISFutureStatus.Canceled</see> from <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see>.
	///
	/// To avoid blocking calling threads with either <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> or <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see>, it is recommended to
	/// use the <see cref="Standard.ArcGISFutureCompletedEvent">ArcGISFutureCompletedEvent</see> to receive a completion notification before checking <see cref="ArcGISFuture.GetError">ArcGISFuture.GetError</see>
	/// for errors and then calling <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> to access the return value.
	///
	/// An executing <see cref="ArcGISFuture">ArcGISFuture</see> can be requested to cancel by calling <see cref="ArcGISFuture.Cancel">ArcGISFuture.Cancel</see>. If the cancellation is
	/// observed by the executing code, then the <see cref="ArcGISFuture">ArcGISFuture</see> returns an <see cref="Standard.ArcGISError">ArcGISError</see> from <see cref="ArcGISFuture.GetError">ArcGISFuture.GetError</see>.
	/// </remarks>
	/// <since>1.0.0</since>
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISFuture<T>
	{
		#region Methods
		/// <summary>
		/// Request cancellation of the <see cref="ArcGISFuture">ArcGISFuture</see>.
		/// </summary>
		/// <remarks>
		/// Cancellation is a cooperative mechanism. Cancel can be called on the <see cref="ArcGISFuture">ArcGISFuture</see>, but the
		/// executing code must also periodically check for cancellation and terminate. It is possible
		/// that a canceled <see cref="ArcGISFuture">ArcGISFuture</see> could still complete successfully, if cancel was called after
		/// the executing code checked for cancellation.
		/// Once cancel is called, <see cref="ArcGISFuture.IsCanceled">ArcGISFuture.IsCanceled</see> will return true.
		/// If the executing code observed the cancellation, the following will apply:
		/// * <see cref="ArcGISFuture.GetError">ArcGISFuture.GetError</see> returns an error indicating cancellation
		/// * <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> returns null
		/// * <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see> returns <see cref="ArcGISFutureStatus.Canceled">ArcGISFutureStatus.Canceled</see>
		/// </remarks>
		/// <returns>
		/// true if the <see cref="ArcGISFuture">ArcGISFuture</see> was requested to cancel, false if the <see cref="ArcGISFuture">ArcGISFuture</see> had already been
		/// requested to cancel.
		/// Returns false if an error occurs.
		/// </returns>
		/// <since>1.0.0</since>
		public bool Cancel()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_cancel(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Returns the result of the <see cref="ArcGISFuture">ArcGISFuture</see>.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is successful then <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> will return the result. For a
		/// <see cref="ArcGISFuture">ArcGISFuture</see> which is successful but has no result then an empty <see cref="">Element</see> is returned.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> has failed during execution, the call to <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> will result
		/// in an error.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is not complete, a call to <see cref="ArcGISFuture.Get">ArcGISFuture.Get</see> will block the calling
		/// thread until the <see cref="ArcGISFuture">ArcGISFuture</see> completes execution.
		/// </remarks>
		/// <returns>
		/// The result of the <see cref="ArcGISFuture">ArcGISFuture</see> or null if the <see cref="ArcGISFuture">ArcGISFuture</see> was canceled.
		/// </returns>
		/// <since>1.0.0</since>
		public T Get()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_get(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			Standard.ArcGISElement localLocalResult = null;

			if (localResult != IntPtr.Zero)
			{
				localLocalResult = new Standard.ArcGISElement(localResult);
			}

			return Convert.FromArcGISElement<T>(localLocalResult);
		}

		/// <summary>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is executing, or has completed successfully, a null is returned. If the <see cref="ArcGISFuture">ArcGISFuture</see> has failed returns the error.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is executing, or completed successfully null is returned. For a
		/// completed but failed <see cref="ArcGISFuture">ArcGISFuture</see> the failure is returned in an <see cref="Standard.ArcGISError">ArcGISError</see>.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> terminated due to a cancellation request, this is also a failure and returns an error.
		/// </remarks>
		/// <returns>
		/// Returns the <see cref="Standard.ArcGISError">ArcGISError</see> instance or null.
		/// </returns>
		/// <since>1.0.0</since>
		public Exception GetError()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_getError(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return Convert.FromArcGISError(new Standard.ArcGISError(localResult));
		}

		/// <summary>
		/// Indicates if the <see cref="ArcGISFuture">ArcGISFuture</see> has been requested to cancel.
		/// </summary>
		/// <returns>
		/// true if the <see cref="ArcGISFuture">ArcGISFuture</see> has been requested to cancel or false otherwise.
		/// </returns>
		/// <since>1.0.0</since>
		public bool IsCanceled()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_isCanceled(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Indicates if the <see cref="ArcGISFuture">ArcGISFuture</see> has completed execution.
		/// </summary>
		/// <returns>
		/// true if the <see cref="ArcGISFuture">ArcGISFuture</see> has completed, false otherwise.
		/// </returns>
		/// <since>1.0.0</since>
		public bool IsDone()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_isDone(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}

		/// <summary>
		/// Waits for the <see cref="ArcGISFuture">ArcGISFuture</see> to complete.
		/// </summary>
		/// <remarks>
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is successful or canceled then <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see> will return the
		/// <see cref="ArcGISFutureStatus">ArcGISFutureStatus</see>.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> has failed during execution, the call to <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see> will
		/// result in an error.
		///
		/// If the <see cref="ArcGISFuture">ArcGISFuture</see> is not complete, a call to <see cref="ArcGISFuture.Wait">ArcGISFuture.Wait</see> will block the calling
		/// thread until the <see cref="ArcGISFuture">ArcGISFuture</see> completes execution.
		/// </remarks>
		/// <returns>
		/// The <see cref="ArcGISFutureStatus">ArcGISFutureStatus</see>. Returns <see cref="ArcGISFutureStatus.Unknown">ArcGISFutureStatus.Unknown</see> if an error occurs.
		/// </returns>
		/// <since>1.0.0</since>
		public ArcGISFutureStatus Wait()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var localResult = PInvoke.RT_Task_wait(Handle, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return localResult;
		}
		#endregion // Methods

		#region Events
		/// <summary>
		/// Sets the function that will be called when the <see cref="ArcGISFuture">ArcGISFuture</see> is completed.
		/// </summary>
		/// <remarks>
		/// When the <see cref="ArcGISFuture">ArcGISFuture</see> completes then <see cref="ArcGISFuture.IsDone">ArcGISFuture.IsDone</see> is true and this callback
		/// will be called.
		///
		/// Setting this callback after <see cref="ArcGISFuture">ArcGISFuture</see> has completed will immediately call the
		/// callback.
		///
		/// Setting the callback to null after it has already been set will stop the function
		/// from being called.
		/// </remarks>
		/// <since>1.0.0</since>
		public Standard.ArcGISFutureCompletedEvent TaskCompleted
		{
			get
			{
				return _taskCompletedHandler.Delegate;
			}
			set
			{
				if (_taskCompletedHandler.Delegate == value)
				{
					return;
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				if (value != null)
				{
					_taskCompletedHandler.Delegate = value;

					PInvoke.RT_Task_setTaskCompletedCallback(Handle, Standard.ArcGISFutureCompletedEventHandler.HandlerFunction, _taskCompletedHandler.UserData, errorHandler);
				}
				else
				{
					PInvoke.RT_Task_setTaskCompletedCallback(Handle, null, _taskCompletedHandler.UserData, errorHandler);

					_taskCompletedHandler.Dispose();
				}

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}
		#endregion // Events

		#region Internal Members
		internal ArcGISFuture(IntPtr handle) => Handle = handle;

		~ArcGISFuture()
		{
			if (Handle != IntPtr.Zero)
			{
				if (_taskCompletedHandler.Delegate != null)
				{
					PInvoke.RT_Task_setTaskCompletedCallback(Handle, null, _taskCompletedHandler.UserData, IntPtr.Zero);

					_taskCompletedHandler.Dispose();
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_Task_destroy(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		internal IntPtr Handle { get; set; }

		internal Standard.ArcGISFutureCompletedEventHandler _taskCompletedHandler = new();
		#endregion // Internal Members
	}

	internal static partial class PInvoke
	{
		#region P-Invoke Declarations
		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Task_cancel(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Task_get(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_Task_getError(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Task_isCanceled(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RT_Task_isDone(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern ArcGISFutureStatus RT_Task_wait(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Task_setTaskCompletedCallback(IntPtr handle, Standard.ArcGISFutureCompletedEventInternal taskCompleted, IntPtr userData, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_Task_destroy(IntPtr handle, IntPtr errorHandle);
		#endregion // P-Invoke Declarations
	}
}
