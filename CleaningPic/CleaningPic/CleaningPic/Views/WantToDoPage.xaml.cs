using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
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
            (BindingContext as WantToDoViewModel).OnAppearing();

            MessagingCenter.Subscribe<WantToDoViewModel, Cleaning>(
                this,
                WantToDoViewModel.navigateNotificationSettingPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new NotificationSettingPage(args)); });

            MessagingCenter.Subscribe<WantToDoViewModel, Cleaning>(
                this,
                WantToDoViewModel.navigateWebBrowserMessage,
                (sender, args) => DisplayLinkAsync(args));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<WantToDoViewModel, Cleaning>(this, WantToDoViewModel.navigateNotificationSettingPageMessage);
            MessagingCenter.Unsubscribe<WantToDoViewModel, Cleaning>(this, WantToDoViewModel.navigateWebBrowserMessage);
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