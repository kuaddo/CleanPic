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
	public partial class NotificationSettingPage : ContentPage
	{
        public const string notificationSettingDoneMessage = "notificationSettingDoneMessage";
        private string cleaningId;

		public NotificationSettingPage(bool canNotify, string cleaningId)
		{
			InitializeComponent();
            this.cleaningId = cleaningId;
            notificationSwitch.IsToggled = canNotify;
        }

        public void NotifySwitch_Toggled(object sender, EventArgs e)
        {
            MessagingCenter.Send(
                this,
                notificationSettingDoneMessage,
                (cleaningId, notificationSwitch.IsToggled));
        }
    }
}