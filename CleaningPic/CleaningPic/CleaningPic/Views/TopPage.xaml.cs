using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
    // このページのMVVMは妥協することにした
	public partial class TopPage : CarouselPage
	{
		public TopPage ()
		{
			InitializeComponent();
            CurrentPageChanged += async (sender, e) =>
            {
                if (CurrentPage is CameraPage cameraPage)
                {
                    CurrentPage = topPage;      // カメラ処理を立ち上げる前に元ページに戻す
                    var imageData = await cameraPage.LaunchCamera();
                    if (imageData != null)
                    {
                        var data = DependencyService.Get<IImageEditor>().SquareAndResize(imageData, 14);   // 72 * 2 = 144
                        await Navigation.PushAsync(new UploadPage(data));
                    }
                }
            };

            MessagingCenter.Subscribe<TopViewModel, (bool, string)>(
                this,
                TopViewModel.navigateNotificationSettingPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new NotificationSettingPage(args.Item1, args.Item2)); });
        }

        public void CameraMenu_Clicked(object sender, EventArgs e)
        {
            CurrentPage = cameraPage;
        }

        public async void SelectImageButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
            if (photo != null)
            {
                using (var stream = photo.GetStream())
                using (var ms = new MemoryStream())
                {
                    if (stream == null) return;
                    stream.CopyTo(ms);
                    var data = DependencyService.Get<IImageEditor>().SquareAndResize(ms.ToArray(), 144);    // 72 * 2 = 144
                    await Navigation.PushAsync(new UploadPage(data));
                }
            }
        }

        public void DeleteButton_Clicked(object sender, EventArgs e)
        {
            using (var ds = new DataSource())
            {
                ds.RemoveAllCleaning();
            }
        }
    }
}