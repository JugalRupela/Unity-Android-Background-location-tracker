using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LocationTracking.Internal;
using UnityEngine;

namespace LocationTracking.Scripts
{
	/// <summary>
	/// Allows to check Android runtime location permissions and request them
	/// </summary>
	[PublicAPI]
	public class LocationPermissionHelper
	{
		const string RequestPermissionActivityClass = "com.nineva.studios.locationtracker.RequestPermissionActivity";
		const int PERMISSION_GRANTED = 0;
		const int PERMISSION_DENIED = -1;
		
		/// <summary>
		/// Permission request result.
		/// </summary>
		[PublicAPI]
		public class PermissionRequestResult
		{
			PermissionRequestResult()
			{
			}

			public static PermissionRequestResult FromJson(Dictionary<string, object> serialized)
			{
				var result = new PermissionRequestResult
				{
					Permission = serialized.GetStr("permission"),
					ShouldShowRequestPermissionRationale = (bool) serialized["shouldShowRequestPermissionRationale"],
					Status = (PermissionStatus) (int) (long) serialized["result"]
				};
				return result;
			}

			/// <summary>
			/// Gets the requested permission.
			/// </summary>
			/// <value>The requested permission.</value>
			[PublicAPI]
			public string Permission { get; private set; }

			/// <summary>
			/// Gets the requested permission status.
			/// </summary>
			/// <value>The status of requested permission.</value>
			[PublicAPI]
			public PermissionStatus Status { get; private set; }

			/// <summary>
			/// Gets whether you should show UI with rationale for requesting a permission.
			/// 
			/// </summary>
			/// <value><c>true</c> if should show explanation why permission is needed; otherwise, <c>false</c>.</value>
			[PublicAPI]
			public bool ShouldShowRequestPermissionRationale { get; private set; }

			public override string ToString()
			{
				return string.Format(
					"[PermissionRequestResult: Permission={0}, Status={1}, ShouldShowRequestPermissionRationale={2}]",
					Permission, Status, ShouldShowRequestPermissionRationale);
			}
		}

		/// <summary>
		/// Permission status
		/// </summary>
		[PublicAPI]
		public enum PermissionStatus
		{
			/// <summary>
			/// The permission has been granted.
			/// </summary>
			[PublicAPI] Granted = PERMISSION_GRANTED,

			/// <summary>
			/// The permission has not been granted.
			/// </summary>
			[PublicAPI] Denied = PERMISSION_DENIED
		}

		/// <summary>
		/// Returns whether the location permission has been granted
		/// </summary>
		public static bool IsLocationPermissionGranted
		{
			get
			{
				return new AndroidJavaClass(RequestPermissionActivityClass).CallStatic<bool>("hasLocationPermission");
			}
		}

		// TODO implement similarly as in Android goodies
		public static void RequestLocationPermission([NotNull] Action<PermissionRequestResult> onRequestPermissionResult, bool isFineLocation = true)
		{
			if (onRequestPermissionResult == null)
			{
				throw new ArgumentNullException("onRequestPermissionResult");
			}
			
			new AndroidJavaClass(RequestPermissionActivityClass).CallStatic("requestPermissions", isFineLocation);
		}

		public static void InternalOnPermissionResult(PermissionRequestResult result)
		{
			// TODO
		}
	}
}