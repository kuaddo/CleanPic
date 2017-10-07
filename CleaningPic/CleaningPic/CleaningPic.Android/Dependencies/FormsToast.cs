using Xamarin.Forms;
using CleaningPic.Utils;
using Android.Widget;
using CleaningPic.Droid;
using static Android.App.Application;

[assembly: Dependency(typeof(FormsToast))]
namespace CleaningPic.Droid
{
    class FormsToast : IFormsToast
    {
        public void Show(string mesaage)
        {
            Toast.MakeText(Context, mesaage, ToastLength.Short).Show();
        }
    }
}