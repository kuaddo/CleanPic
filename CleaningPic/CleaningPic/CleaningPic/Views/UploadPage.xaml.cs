using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UploadPage : ContentPage
	{
		public UploadPage(ImageSource source)
		{
			InitializeComponent();
            uploadImage.Source = source;
		}

        // ONになったSwitch以外を全てOFFにする
        public void SelectPlaceSwitch_Toggled(Object sender, EventArgs e)
        {
            var switchList = new List<Switch>() { livingSwitch, kitchenSwitch, bathSwitch, toiletSwitch };
            var onCount = switchList.Where(s => s.IsToggled).Count();
            switch (onCount)
            {
                case 0:     // ONのSwitchがOFFになった場合
                    (sender as Switch).IsToggled = true;
                    break;
                case 1:     // 何もしないイベント処理用
                    break;
                default:    // ONのSwitchが複数ある場合
                    var offList = switchList.Where(s => !ReferenceEquals(s, sender) && (s as Switch).IsToggled);
                    foreach (var s in offList) s.IsToggled = false;
                    (BindingContext as UploadViewModel).CleanigPlace = (Place)switchList.IndexOf(sender as Switch);
                    break;
            }
        }

        public async void UploadButton_Clicked(object sender, EventArgs e)
        {
            //Console.WriteLine((BindingContext as UploadViewModel).CleanigPlace);

            // MessagingCenterを利用して、プラットフォーム固有のダイアログを表示
            MessagingCenter.Send(this, "progress_dialog", true);
            await Task.Delay(3000);
            MessagingCenter.Send(this, "progress_dialog", false);
        }
	}
}