using System;
using Android.Content.Res;
using Android.App;

using Android;
using Android.Runtime;
using Android.OS;
using Android.Widget;

using Xamarin.Facebook;

namespace HelloFacebookXamarinAndroidSample
{
	public static class Messages
	{

		public enum Message 
		{
			success = 0,
			successfully_posted_post = 1,
			error = 2
		}

		public static string GetString(Message message){
		
			string str = "";
			switch (message) {
			case Message.success:
				str = R.GetString (Resource.String.success);
				break;
			case Message.successfully_posted_post:
				str = R.GetString (Resource.String.successfully_posted_post);
				break;
			case Message.error:
				str = R.GetString (Resource.String.error);
				break;
			}

			return str;

		}

		public static string GetString(Message message, String formatArgs){

			string str = "";
			switch (message) {
			case Message.success:
				str = R.GetString (Resource.String.success,formatArgs);
				break;
			case Message.successfully_posted_post:
				str = R.GetString (Resource.String.successfully_posted_post,formatArgs);
				break;
			case Message.error:
				str = R.GetString (Resource.String.error,formatArgs);
				break;
			}

			return str;
		}
			


		private static Android.Content.Res.Resources R
		{
			get { 
				return Application.Context.Resources;
			}
		}


		public static void showResult(String title,String alertMessage,Android.Content.Context context){
		
			new AlertDialog.Builder (context)
				.SetTitle (title)
				.SetMessage (alertMessage)
				.SetPositiveButton (R.GetString(Resource.String.ok),(senderAlert,args) => {})
				.Show ();
		}

		public static void showAlert(){
			new AlertDialog.Builder (Application.Context)
				.SetTitle (R.GetString (Resource.String.cancelled))
				.SetMessage (R.GetString (Resource.String.permission_not_granted))
				.SetPositiveButton (R.GetString (Resource.String.ok), (senderAlert, args) => {})
 				.Show ();
		}









	}
}

