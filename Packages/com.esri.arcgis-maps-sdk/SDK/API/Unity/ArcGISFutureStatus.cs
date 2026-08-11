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
namespace Esri.Unity
{
	/// <summary>
	/// The different states for <see cref="ArcGISFuture">ArcGISFuture</see>.
	/// </summary>
	/// <remarks>
	/// Each of the different states for a <see cref="ArcGISFuture">ArcGISFuture</see> instance.
	/// </remarks>
	/// <seealso cref="ArcGISFuture.Wait">ArcGISFuture.Wait</seealso>
	/// <since>1.0.0</since>
	public enum ArcGISFutureStatus
	{
		/// <summary>
		/// The <see cref="ArcGISFuture">ArcGISFuture</see> has completed.
		/// </summary>
		/// <since>1.0.0</since>
		Completed = 0,

		/// <summary>
		/// The <see cref="ArcGISFuture">ArcGISFuture</see> was canceled.
		/// </summary>
		/// <since>1.0.0</since>
		Canceled = 1,

		/// <summary>
		/// The <see cref="ArcGISFuture">ArcGISFuture</see> has not completed and is not canceled.
		/// </summary>
		/// <since>1.0.0</since>
		NotComplete = 2,

		/// <summary>
		/// The <see cref="ArcGISFuture">ArcGISFuture</see> status is unknown. Used for error conditions.
		/// </summary>
		/// <since>1.0.0</since>
		Unknown = -1
	};
}
