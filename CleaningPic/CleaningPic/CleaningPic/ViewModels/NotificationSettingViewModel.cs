using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class NotificationSettingViewModel : BindableBase
    {
        private bool _CanNotify;
        private DateTime _NotificationDate = DateTime.Now.AddHours(3);
        private TimeSpan _NotificationTime = DateTime.Now.AddHours(3).TimeOfDay;
        private Cleaning _Cleaning;

        public bool CanNotify
        {
            get { return _CanNotify; }
            set { SetProperty(ref _CanNotify, value); }
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

        public Cleaning Cleaning
        {
            get { return _Cleaning; }
            set
            {
                _Cleaning = value;
                
                if (value.NotificationDate > DateTimeOffset.UtcNow)
                {
                    // Date/Time PickerではUTCは用いない
                    var date = new DateTime(value.NotificationDate.AddHours(9).Ticks);
                    NotificationDate = date;
                    NotificationTime = date.TimeOfDay;
                    CanNotify = value.CanNotify;
                }
                else
                    CanNotify = false;
            }
        }
        public DateTime NotificationDateTime => new DateTime(NotificationDate.Date.Ticks + NotificationTime.Ticks - NotificationTime.Seconds * TimeSpan.TicksPerSecond);
        public bool ValidDate => NotificationDateTime > DateTime.Now;

        public void NotificationStateChanged()
        {
            if (!ValidDate)
                return;

            // ON -> ON (設定時刻変化あり)
            if (CanNotify && Cleaning.CanNotify &&
                Cleaning.NotificationDate.AddHours(9).Ticks / TimeSpan.TicksPerSecond != NotificationDateTime.Ticks / TimeSpan.TicksPerSecond)
                ResetNotification();
            
            // 通知状態変化なし
            if (CanNotify == Cleaning.CanNotify)
                return;

            // 通知状態変化なし 
            if (CanNotify)
                SetNotification();
            else
                CancelNotification();
        }

        private void SetNotification()
        {
            // UTCに変換
            UpdateDataSource();
            DependencyService.Get<IFormsNotification>().Notify("Let's clean up!", "掃除をする時間です!!", NotificationDateTime.AddHours(-9));
            DependencyService.Get<IFormsToast>().Show(NotificationDateTime + "に通知を設定");
        }

        private void ResetNotification()
        {
            // TODO: キャンセル
            UpdateDataSource();
            DependencyService.Get<IFormsNotification>().Notify("Let's clean up!", "掃除をする時間です!!", NotificationDateTime.AddHours(-9));
            DependencyService.Get<IFormsToast>().Show(NotificationDateTime + "に通知を再設定");
        }

        private void CancelNotification()
        {
            // TODO: キャンセル
            UpdateDataSource();
            DependencyService.Get<IFormsToast>().Show("通知をキャンセルしました");
        }

        private void UpdateDataSource()
        {
            Cleaning.CanNotify = CanNotify;
            Cleaning.NotificationDate = new DateTimeOffset(NotificationDateTime.AddHours(-9).Ticks, TimeSpan.Zero);
            using (var ds = new DataSource())
                ds.UpdateCleaning(Cleaning);
        }
    }
}
