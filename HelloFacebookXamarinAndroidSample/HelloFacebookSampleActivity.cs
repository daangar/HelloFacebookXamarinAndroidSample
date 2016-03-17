
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Net;
using Android.Util;
using Android.Content.Res;

using Xamarin.Facebook;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share.Widget;





namespace HelloFacebookXamarinAndroidSample
{
	[Activity (Label = "HelloFacebookSampleActivity",MainLauncher = true, Icon = "@mipmap/icon")]			
	public class HelloFacebookSampleActivity : FragmentActivity
	{

		private static readonly String PERMISSION = "publish_actions";
		private static readonly Location SEATTLE_LOCATION = new Location(""){

			Latitude = 47.6097,
			Longitude = -122.3331

		};

		private readonly String PENDING_ACTION_BUNDLE_KEY =
			"com.example.hellofacebook:PendingAction";

		private enum PendingAction
		{
			NONE,
			POST_PHOTO,
			POST_STATUS_UPDATE
		}

		private Button postStatusUpdateButton;
		private Button postPhotoButton;
		private ProfilePictureView profilePictureView;
		private TextView greeting;
		private PendingAction pendingAction = PendingAction.NONE;
		private Boolean canPresentShareDialog;
		private Boolean canPresentShareDialogWithPhotos;
		private ICallbackManager callbackManager;
		private XamProfileTracker profileTracker;
		private ShareDialog shareDialog;




		private FacebookHelper.FacebookCallBack<Xamarin.Facebook.Share.SharerResult> shareCallback; 





		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			FacebookSdk.SdkInitialize (ApplicationContext);

			callbackManager = CallbackManagerFactory.Create ();

			shareCallback = new FacebookHelper.FacebookCallBack<Xamarin.Facebook.Share.SharerResult>()
			{


				HandleSuccess = result => {



					Console.WriteLine ("{0}: {1}","HelloFacebook","Sucess!!");
					if (result.PostId != null){
						String title = Messages.GetString(Messages.Message.success);
						String id = result.PostId;
						String alertMessage = Messages.GetString(Messages.Message.successfully_posted_post,id);
						showResult(title,alertMessage);


					}

				},

				HandleCancel = () => {

					Console.WriteLine ("{0}: {1}","HelloFacebook","Canceled");
				},

				HandleError = error => {

					Console.WriteLine ("Error: {0}",error.ToString());
					String title = Messages.GetString(Messages.Message.error);
					String alertMessage = error.Message;
					showResult(title,alertMessage);


				}

			};







			LoginManager.Instance.RegisterCallback(callbackManager,
				new FacebookHelper.FacebookCallBack<LoginResult>(){

					HandleSuccess = loginResult => {
						handlePendingAction();
						updateUI();
					},

					HandleCancel = () => {
						if(pendingAction != PendingAction.NONE){
							showAlert();
							pendingAction = PendingAction.NONE;
						}
						updateUI();
					},

					HandleError = error => {
						if (pendingAction == PendingAction.NONE
							&& error is FacebookAuthorizationException){

							showAlert();
							pendingAction = PendingAction.NONE;
						}
						updateUI();
					}


				});

			shareDialog = new ShareDialog (this);
			shareDialog.RegisterCallback (callbackManager, shareCallback);

			if (savedInstanceState != null) {
				String name = savedInstanceState.GetString (PENDING_ACTION_BUNDLE_KEY);
				pendingAction = (PendingAction)Enum.Parse (typeof(PendingAction),name,true);

			}

			SetContentView (Resource.Layout.Main);

			profileTracker = new XamProfileTracker () {
				HandleOnCurrentProfileChanged = () => {
					updateUI ();
					handlePendingAction ();
				}
			};


			profilePictureView = (ProfilePictureView)FindViewById (Resource.Id.profilePicture);
			greeting = (TextView)FindViewById (Resource.Id.greeting);

			postStatusUpdateButton = (Button)FindViewById (Resource.Id.postStatusUpdateButton);
			postStatusUpdateButton.Click += delegate(object sender, EventArgs e) {
				onClickPostStatusUpdate();
			};

			postPhotoButton = (Button)FindViewById (Resource.Id.postPhotoButton);
			postPhotoButton.Click += delegate(object sender, EventArgs e) {
				onClickPostPhoto();
			};

			canPresentShareDialog = ShareDialog.CanShow(Java.Lang.Class.FromType(typeof(ShareLinkContent)));

			canPresentShareDialogWithPhotos = ShareDialog.CanShow (Java.Lang.Class.FromType(typeof(SharePhotoContent)));

		}

		protected override void OnResume ()
		{
			base.OnResume ();

			AppEventsLogger.ActivateApp (this);

			updateUI ();
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);

			outState.PutString (PENDING_ACTION_BUNDLE_KEY, pendingAction.ToString ());
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			callbackManager.OnActivityResult (requestCode, (int)resultCode, data);
		}


		protected override void OnPause ()
		{
			base.OnPause ();

			AppEventsLogger.DeactivateApp (this);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			profileTracker.StopTracking ();
		}

		private void updateUI(){
			Boolean enableButtons = AccessToken.CurrentAccessToken != null;
			postStatusUpdateButton.Enabled = enableButtons || canPresentShareDialog;
			postPhotoButton.Enabled = enableButtons || canPresentShareDialogWithPhotos;

			Android.Content.Res.Resources res = ApplicationContext.Resources;



			Profile profile = Profile.CurrentProfile;
			if (enableButtons && profile != null) {
				profilePictureView.ProfileId = profile.Id;
				greeting.Text = res.GetString (Resource.String.hello_user, profile.FirstName);
			}

		}


		private void handlePendingAction(){
			PendingAction previouslyPendingAction = pendingAction;

			pendingAction = PendingAction.NONE;

			switch (previouslyPendingAction) {
			case PendingAction.NONE:
				break;
			case PendingAction.POST_PHOTO:
				postPhoto ();
				break;
			case PendingAction.POST_STATUS_UPDATE:
				postStatusUpdate ();
				break;
			}

		}

		private void postStatusUpdate(){
			Profile profile = Profile.CurrentProfile;
			ShareLinkContent linkContent = new ShareLinkContent.Builder ().Build();
//				.SetContentTitle ("Helo Facebook")
//				.SetContentDescription ("The Hello Facebook sample showcases simple Facebook integration")
//				.Build ();
//
//			ShareContent content = new ShareContent.Builder()
//				.SetContentUrl(Android.Net.Uri.Parse("http://developers.facebook.com/docs/android"))





			if (canPresentShareDialog) {
				shareDialog.Show (linkContent);
			} else if (profile != null && hasPublishPermission ()) {
				Xamarin.Facebook.Share.ShareApi.Share (linkContent, shareCallback);
			} else {
				pendingAction = PendingAction.POST_STATUS_UPDATE;
			}


			
		}

		private Boolean hasPublishPermission() {
			AccessToken accesToken = AccessToken.CurrentAccessToken;
			return accesToken != null && accesToken.Permissions.AsQueryable().Contains("publish_actions");
		}

		private void performPublish(PendingAction action,Boolean allowToken){
			AccessToken accesstoken = AccessToken.CurrentAccessToken;
			if (accesstoken != null || allowToken) {
				pendingAction = action;
				handlePendingAction ();
			}
		}

		private void postPhoto() {
			Bitmap image = BitmapFactory.DecodeResource (ApplicationContext.Resources, Resource.Drawable.icon);
			SharePhoto sharePhoto = new SharePhoto.Builder ().SetBitmap (image).Build () as SharePhoto;
			List<SharePhoto> photos = new List<SharePhoto>();
			photos.Add (sharePhoto);

			SharePhotoContent sharePhotoContent =
				new SharePhotoContent.Builder().SetPhotos(photos).Build();
			
			if (canPresentShareDialogWithPhotos) {
				shareDialog.Show(sharePhotoContent);
			} else if (hasPublishPermission()) {
				Xamarin.Facebook.Share.ShareApi.Share (sharePhotoContent, shareCallback);
			} else {
				pendingAction = PendingAction.POST_PHOTO;
				// We need to get new permissions, then complete the action when we get called back.
				LoginManager.Instance.LogInWithPublishPermissions(
					this,
					Arrays.AsList(PERMISSION)
					);
			}


		}

		private void onClickPostStatusUpdate() {
			performPublish(PendingAction.POST_STATUS_UPDATE, canPresentShareDialog);
		}

		private void onClickPostPhoto() {
			performPublish(PendingAction.POST_PHOTO, canPresentShareDialogWithPhotos);
		}


		private void showResult(String title,String alertMessage){

			new AlertDialog.Builder (this)
				.SetTitle (title)
				.SetMessage (alertMessage)
				.SetPositiveButton (Resources.GetString(Resource.String.ok),(senderAlert,args) => {})
				.Show ();
		}

		public void showAlert(){
			new AlertDialog.Builder (this)
				.SetTitle (Resources.GetString (Resource.String.cancelled))
				.SetMessage (Resources.GetString (Resource.String.permission_not_granted))
				.SetPositiveButton (Resources.GetString (Resource.String.ok), (senderAlert, args) => {})
				.Show ();
		}

	}
}

