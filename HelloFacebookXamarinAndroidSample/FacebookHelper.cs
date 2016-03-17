using System;

using Android.Runtime;
using Android.Widget;

using Xamarin.Facebook;
using Android.App;
using Android.Content.Res;

namespace HelloFacebookXamarinAndroidSample
{
	public class FacebookHelper
	{


		public class FacebookCallBack<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
		{


		

			public Action HandleCancel { get; set; }

			public Action<FacebookException> HandleError { get; set; }

			public Action<TResult> HandleSuccess { get; set; }





			public void OnCancel ()
			{
				Action c = HandleCancel;
				if (c != null)
					c ();
			}

			public void OnError (FacebookException error)
			{
				Action<FacebookException> c = HandleError;
				if (c != null)
					c (error);
			}

			public void OnSuccess(Java.Lang.Object result)
			{
				Action<TResult> c = HandleSuccess;
				if (c != null)
					c (result.JavaCast<TResult>());
			}
				

		}
	}
}

