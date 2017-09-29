using CleaningPic.Data;
using CleaningPic.Utils;
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
        private byte[] data;

		public UploadPage(byte[] imageData)
		{
			InitializeComponent();
            data = imageData;
            uploadImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;
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
            var now = DateTimeOffset.UtcNow;
            await Navigation.PushAsync(new ResultPage(data, new List<Cleaning>()
            {
                new Cleaning() { Dirt = "汚れ1", ImageData = null, Method = "手法1", Tools = new List<string> { "道具1", "道具1'" }, Created = now, Probability = 0.7,  CleaningTime = 10 },
                new Cleaning() { Dirt = "汚れ2", ImageData = null, Method = "手法2", Tools = new List<string> { "道具2", "道具2'" }, Created = now, Probability = 0.13, CleaningTime = 50  },
                new Cleaning() { Dirt = "汚れ3", ImageData = null, Method = "手法3", Tools = new List<string> { "道具3", "道具3'" }, Created = now, Probability = 0.11, CleaningTime = 30  },
                new Cleaning() { Dirt = "汚れ4", ImageData = null, Method = "手法4", Tools = new List<string> { "道具4", "道具4'" }, Created = now, Probability = 0.05, CleaningTime = 120  },
                new Cleaning() { Dirt = "汚れ5", ImageData = null, Method = "手法5", Tools = new List<string> { "道具5", "道具5'" }, Created = now, Probability = 0.01, CleaningTime = 150  }
            }.ToArray()));
        }
	}
}