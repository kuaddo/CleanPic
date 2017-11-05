using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class NotificationSettingPage : ContentPage
	{
        // SwitchにCommandをバインドできないので仕方なくVMを所持
        private NotificationSettingViewModel viewModel;

		public NotificationSettingPage(bool canNotify, string cleaningId)
		{
			InitializeComponent();
            viewModel = (BindingContext as NotificationSettingViewModel);
            viewModel.CanNotify = canNotify;
            viewModel.CleaningId = cleaningId;
        }

        public void NotifySwitch_Toggled(object sender, EventArgs e)
        {
            viewModel.NotificationStateChanged();
        }
    }
}