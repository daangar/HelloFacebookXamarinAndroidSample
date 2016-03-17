using System;
using Xamarin.Facebook;



namespace HelloFacebookXamarinAndroidSample
{
	public class XamProfileTracker : ProfileTracker
	{
		public Action HandleOnCurrentProfileChanged { get; set;}

		protected override void OnCurrentProfileChanged (Profile oldProfile, Profile currentProfile)
		{
			Action c = HandleOnCurrentProfileChanged;
			if (c != null)
				c ();

			
		}
	}
}

