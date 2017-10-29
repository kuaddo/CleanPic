﻿using CleaningPic.Data;
using CleaningPic.ViewModels;
using System.Threading.Tasks;
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
    }
}