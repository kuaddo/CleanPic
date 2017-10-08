using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using Xamarin.Forms;
using CleaningPic.Views;
using FFImageLoading.Forms.Droid;

[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
namespace CleaningPic.Droid
{
	[Activity (Label = "CleaningPic", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        private ProgressDialog dialog = null;
            
        protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
            CachedImageRenderer.Init();
            LoadApplication (new CleaningPic.App ());

            // LoadingDialogのメッセージ待ち
            MessagingCenter.Subscribe<UploadPage, bool>(this, "progress_dialog", (page, isVisible) =>
            {
                RunOnUiThread(() =>
                {
                    if (isVisible)
                    {
                        dialog = new ProgressDialog(this);
                        dialog.SetTitle("");
                        dialog.SetMessage("画像送信中...");
                        dialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                        dialog.SetCanceledOnTouchOutside(false);
                        dialog.Show();
                    }
                    else 
                    {
                        if (dialog != null)
                            dialog.Dismiss();
                    }
                });
            });
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

