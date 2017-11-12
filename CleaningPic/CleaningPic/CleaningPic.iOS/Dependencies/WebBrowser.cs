using System;
using CleaningPic.Utils;
using Xamarin.Forms;
using CleaningPic.iOS;
using UIKit;

[assembly: Dependency(typeof(WebBrowser))]
namespace CleaningPic.iOS
{
    public class WebBrowser : IWebBrowser
    {
        public void Open(Uri uri)
        {
            UIApplication.SharedApplication.OpenUrl(uri);
        }
    }
}