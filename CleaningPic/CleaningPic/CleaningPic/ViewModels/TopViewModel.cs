using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class TopViewModel : BindableBase
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();
        public const string navigateNotificationSettingPageMessage = "navigateNotificationSettingPageMessage_TopViewModel";
        public const string navigateWebBrowserMessage = "navigateWebBrowserMessage_TopViewModel";

        public bool _HasMoreItem = true;
        public bool HasMoreItem
        {
            get { return _HasMoreItem; }
            set { SetProperty(ref _HasMoreItem, value); }
        }

        public Command CleaningRefreshCommand      { get; private set; }
        public Command CleaningShoppingCommand     { get; private set; }
        public Command CleaningDoneCommand         { get; private set; }
        public Command CleaningRemoveCommand       { get; private set; }
        public Command CleaningNotificationCommand { get; private set; }

        private const int displayLimit = 5;
        private int changedItemIndex = -1;

        public TopViewModel()
        {
            CleaningRefreshCommand = new Command(() => LoadCleaning());

            CleaningShoppingCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateWebBrowserMessage, c);
            });

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

                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を完了しました");
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                {
                    ds.RemoveCleaning(c);
                    HasMoreItem = ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Count() > displayLimit;
                }
                Items.Remove(c);
                
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });

            CleaningNotificationCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateNotificationSettingPageMessage, c);
                changedItemIndex = Items.IndexOf(c);
            });
            
            LoadCleaning();
        }

        public void OnAppearing()
        {
            if (changedItemIndex != -1)
            {
                var c = Items[changedItemIndex];
                Items.RemoveAt(changedItemIndex);
                Items.Insert(changedItemIndex, c);
                changedItemIndex = -1;
            }
        }

        private void LoadCleaning()
        {
            Items.Clear();
            using (var ds = new DataSource())
            {
                foreach (var c in ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Take(displayLimit))
                    Items.Add(c);
                HasMoreItem = ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Count() > displayLimit;
            }
        }
    }
}
