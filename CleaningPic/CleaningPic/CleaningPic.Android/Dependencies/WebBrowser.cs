using System;
using Android.Content;
using Xamarin.Forms;
using CleaningPic.Utils;
using CleaningPic.Droid;

[assembly: Dependency(typeof(WebBrowserService))]
namespace CleaningPic.Droid
{
    public class WebBrowserService : IWebBrowser
    {
        public void Open(Uri uri)
        {
            Forms.Context.StartActivity(
                new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri.AbsoluteUri)));
        }
    }
}