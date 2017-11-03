using CleaningPic.Data;
using CleaningPic.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class TopViewModel : BindableBase
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();
        public const string navigateNotificationSettingPageMessage = "navigateNotificationSettingPageMessage";

        public bool _HasMoreItem = true;
        public bool HasMoreItem
        {
            get { return _HasMoreItem; }
            set { SetProperty(ref _HasMoreItem, value); }
        }

        public Command CleaningDoneCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }
        public Command CleaningNotificationCommand { get; private set; }

        private const int displayLimit = 5;

        public TopViewModel()
        {
            CleaningDoneCommand = new Command<Cleaning>(c =>
            {
                c.Done = true;
                c.Created = DateTimeOffset.UtcNow;
                using (var ds = new DataSource())
                {
                    ds.UpdateCleaning(c);
                    HasMoreItem = ds.ReadAllCleaning().Where(cleanig => !cleanig.Done).Count() > displayLimit;
                }
                Items.Remove(c);
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                {
                    ds.RemoveCleaning(c);
                    HasMoreItem = ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Count() > displayLimit;
                }
                Items.Remove(c);
            });

            CleaningNotificationCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateNotificationSettingPageMessage, (c.CanNotify, c.Id));
            });

            MessagingCenter.Subscribe<NotificationSettingPage, (string, bool)>(
                this,
                NotificationSettingPage.notificationSettingDoneMessage,
                (sender, args) => UpdateCanNotify(args.Item1, args.Item2));

            // データの読み込み
            using (var ds = new DataSource())
            {
                foreach (var c in ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Take(displayLimit))
                    Items.Add(c);
                HasMoreItem = ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Count() > displayLimit;
            }
        }

        private void UpdateCanNotify(string cleaningId, bool canNotify)
        {
            var cleaning = Items.First(c => c.Id == cleaningId);
            cleaning.CanNotify = canNotify;
            using (var ds = new DataSource())
                ds.UpdateCleaning(cleaning);
        }
    }
}
