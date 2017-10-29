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

        public Command CleaningDoneCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }
        public Command CleaningNotificationCommand { get; private set; }

        public TopViewModel()
        {
            CleaningDoneCommand = new Command<Cleaning>(c =>
            {
                c.Done = true;
                c.Created = DateTimeOffset.UtcNow;
                using (var ds = new DataSource())
                    ds.UpdateCleaning(c);
                Items.Remove(c);
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
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
                foreach (var c in ds.ReadAllCleaning().Where(c => !c.Done).Take(5))
                    Items.Add(c);
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
