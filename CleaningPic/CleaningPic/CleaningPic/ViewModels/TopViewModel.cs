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
        private int cleaningCount = -1;

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

            // データのロードはOnAppearing()で行う
        }

        public void OnAppearing()
        {
            // 通知用処理
            if (changedItemIndex != -1)
            {
                var c = Items[changedItemIndex];
                Items.RemoveAt(changedItemIndex);
                Items.Insert(changedItemIndex, c);
                changedItemIndex = -1;
            }

            // トップ更新用処理
            if (cleaningCount != CountWantToDoCleaning())
                LoadCleaning();
        }

        public void OnDisappearing()
        {
            cleaningCount = CountWantToDoCleaning();
        }

        private void LoadCleaning()
        {
            Items.Clear();
            using (var ds = new DataSource())
            {
                foreach (var c in ds.ReadAllCleaning()
                    .Where(cleaning => !cleaning.Done)
                    .OrderByDescending(c => c.Created.Ticks)
                    .Take(displayLimit))
                {
                    Items.Add(c);
                }
                HasMoreItem = CountWantToDoCleaning() > displayLimit;
            }
        }

        private int CountWantToDoCleaning()
        {
            var count = 0;
            using (var ds = new DataSource())
                count = ds.ReadAllCleaning().Where(cleaning => !cleaning.Done).Count();
            return count;
        }
    }
}
