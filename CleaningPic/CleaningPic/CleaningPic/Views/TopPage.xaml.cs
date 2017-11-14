using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    // このページのMVVMは妥協することにした
    public partial class TopPage : CarouselPage
	{
        public const string navigateWantToDoPageMessage = "navigateWantToDoPageMessage_TopPage";
        private const int imageSize = 163;

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
                        var data = DependencyService.Get<IImageEditor>().SquareAndResize(imageData, imageSize);
                        await Navigation.PushAsync(new UploadPage(data));
                    }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (topPage.BindingContext as TopViewModel).OnAppearing();

            MessagingCenter.Subscribe<TopViewModel, Cleaning>(
                this,
                TopViewModel.navigateNotificationSettingPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new NotificationSettingPage(args)); });

            MessagingCenter.Subscribe<TopViewModel, Cleaning>(
                this,
                TopViewModel.navigateWebBrowserMessage,
                (sender, args) => DisplayLinkAsync(args));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (topPage.BindingContext as TopViewModel).OnDisappearing();
            MessagingCenter.Unsubscribe<TopViewModel, Cleaning>(this, TopViewModel.navigateNotificationSettingPageMessage);
            MessagingCenter.Unsubscribe<TopViewModel, Cleaning>(this, TopViewModel.navigateWebBrowserMessage);
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
                    var data = DependencyService.Get<IImageEditor>().SquareAndResize(ms.ToArray(), imageSize);
                    await Navigation.PushAsync(new UploadPage(data));
                }
            }
        }

        public void ListViewItem_Clicked(object sender, EventArgs e)
        {
            var listView = ((ListView)sender);
            if (listView.SelectedItem != null)
            {
                var cleaning = listView.SelectedItem as Cleaning;
                ((ListView)sender).SelectedItem = null;
                Navigation.PushAsync(new DetailPage(cleaning, false, true));
            }
        }

        public void GoWantToDoLabel_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, navigateWantToDoPageMessage);
        }

        private async void DisplayLinkAsync(Cleaning c)
        {
            var tools = new List<string>();
            var links = new List<string>();
            for (int i = 0; i < c.Tools.Count; i++)
            {
                if (!string.IsNullOrEmpty(c.Links[i]))
                {
                    tools.Add(c.Tools[i]);
                    links.Add(c.Links[i]);
                }
            }
            if (tools.Count == 0) return;
            var result = await DisplayActionSheet("買いたい物を選択してください", "キャンセル", null, tools.ToArray());
            if (result == null || result == "キャンセル") return;
            var link = links[tools.IndexOf(result)];
            DependencyService.Get<IWebBrowser>().Open(new Uri(link));
        }
    }
}