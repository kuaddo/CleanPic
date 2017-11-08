using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class NotificationSettingViewModel : BindableBase
    {
        public const string notificationSettingDoneMessage = "notificationSettingDoneMessage";
        private bool _CanNotify;
        private DateTime _NotificationDate = DateTime.Now.AddHours(3);
        private TimeSpan _NotificationTime = DateTime.Now.AddHours(3).TimeOfDay;

        public bool CanNotify
        {
            get { return _CanNotify; }
            set
            {
                SetProperty(ref _CanNotify, value);
                OnPropertyChanged(nameof(ValidDate));
            }
        }

        public DateTime NotificationDate
        {
            get { return _NotificationDate; }
            set
            {
                SetProperty(ref _NotificationDate, value);
                OnPropertyChanged(nameof(NotificationDateTime));
                OnPropertyChanged(nameof(ValidDate));
            }
        }

        public TimeSpan NotificationTime
        {
            get { return _NotificationTime; }
            set
            {
                SetProperty(ref _NotificationTime, value);
                OnPropertyChanged(nameof(NotificationDateTime));
                OnPropertyChanged(nameof(ValidDate));
            }
        }

        public string CleaningId { get; set; }
        public DateTime NotificationDateTime => new DateTime(NotificationDate.Date.Ticks + NotificationTime.Ticks);
        public bool ValidDate => CanNotify || NotificationDateTime > DateTime.Now;

        public void NotificationStateChanged()
        {
            if (CanNotify)
            {
                // UTCに変換
                DependencyService.Get<IFormsNotification>().Notify("Test title", "これは通知テストです", NotificationDateTime.AddHours(-9));
                DependencyService.Get<IFormsToast>().Show(NotificationDateTime + "に通知");
            }
            else
                DependencyService.Get<IFormsToast>().Show("通知をキャンセルしました");
            MessagingCenter.Send(
                this,
                notificationSettingDoneMessage,
                (CleaningId, CanNotify));
        }
    }
}
