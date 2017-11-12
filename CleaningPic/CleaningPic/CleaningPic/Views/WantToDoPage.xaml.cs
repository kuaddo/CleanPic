using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class WantToDoPage : ContentPage
	{
        public WantToDoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<WantToDoViewModel, (bool, string)>(
                this,
                WantToDoViewModel.navigateNotificationSettingPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new NotificationSettingPage(args.Item1, args.Item2)); });

            MessagingCenter.Subscribe<WantToDoViewModel, Cleaning>(
                this,
                WantToDoViewModel.navigateWebBrowserMessage,
                (sender, args) => DisplayLinkAsync(args));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<WantToDoViewModel, (bool, string)>(this, WantToDoViewModel.navigateNotificationSettingPageMessage);
            MessagingCenter.Unsubscribe<WantToDoViewModel, Cleaning>(this, WantToDoViewModel.navigateWebBrowserMessage);
        }

        public async void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await (BindingContext as WantToDoViewModel).OnItemAppearing(e.Item as Cleaning);
        }

        public void ListViewItem_Clicked(object sender, EventArgs e)
        {
            var listView = ((ListView)sender);
            if (listView.SelectedItem != null)
            {
                var cleaning = listView.SelectedItem as Cleaning;
                ((ListView)sender).SelectedItem = null;
                Navigation.PushAsync(new DetailPage(cleaning));
            }
        }

        private async void DisplayLinkAsync(Cleaning c)
        {
            var result = await DisplayActionSheet("買いたい物を選択してください", "キャンセル", null, c.Tools.ToArray());
            if (result == null || result == "キャンセル") return;
            var link = c.Links[c.Tools.IndexOf(result)];
            DependencyService.Get<IWebBrowser>().Open(new Uri(link));
        }
    }
}