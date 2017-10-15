using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class UploadPage : ContentPage
	{
		public UploadPage(byte[] imageData)
		{
			InitializeComponent();
            uploadImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;

            // 画面遷移のメッセージ
            MessagingCenter.Subscribe<UploadViewModel, Cleaning[]>(
                this, 
                UploadViewModel.navigateResultPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new ResultPage(imageData, args)); });
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
	}
}