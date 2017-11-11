using CleaningPic.Data;
using CleaningPic.ViewModels;
using System;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class WantToDoPage : ContentPage
	{
        public WantToDoPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<WantToDoViewModel, (bool, string)>(
                this,
                WantToDoViewModel.navigateNotificationSettingPageMessage,
                async (sender, args) => { await Navigation.PushAsync(new NotificationSettingPage(args.Item1, args.Item2)); });
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
    }
}