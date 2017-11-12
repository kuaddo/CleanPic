using System;
using Android.Content;
using Xamarin.Forms;
using CleaningPic.Utils;
using CleaningPic.Droid;

[assembly: Dependency(typeof(WebBrowser))]
namespace CleaningPic.Droid
{
    public class WebBrowser : IWebBrowser
    {
        public void Open(Uri uri)
        {
            Forms.Context.StartActivity(
                new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri.AbsoluteUri)));
        }
    }
}