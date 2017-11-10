using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class WantToDoViewModel : BindableBase
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();
        private string itemCountString;
        private bool isLoading = false;
        public string ItemCountString
        {
            get { return itemCountString; }
            set { SetProperty(ref itemCountString, value); }
        }
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private const int loadingCount = 10;
        public const string cleaningDoneMessage = "cleaningDoneMessage";
        public const string navigateNotificationSettingPageMessage = "navigateNotificationSettingPageMessage";

        public Command CleaningDoneCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }
        public Command CleaningNotificationCommand { get; private set; }

        public WantToDoViewModel()
        {
            CleaningDoneCommand = new Command<Cleaning>(c =>
            {
                c.Done = true;
                c.Created = DateTimeOffset.UtcNow;
                using (var ds = new DataSource())
                    ds.UpdateCleaning(c);
                Items.Remove(c);

                // Messageをやったページに送って、更新するようにする
                MessagingCenter.Send(
                    this,
                    cleaningDoneMessage,
                    c);

                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を完了しました");
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
                Items.Remove(c);
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });

            CleaningNotificationCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateNotificationSettingPageMessage, (c.CanNotify, c.Id));
            });

            MessagingCenter.Subscribe<NotificationSettingViewModel, (string, bool)>(
                this,
                NotificationSettingViewModel.notificationSettingDoneMessage,
                (sender, args) => UpdateCanNotify(args.Item1, args.Item2));

            // Itemsが変化した時にItemCountStringを更新するようにする
            Items.CollectionChanged += (sender, e) =>
            {
                var count = 0;
                using (var ds = new DataSource())
                    count = ds.ReadAllCleaning()
                        .Where(c => !c.Done)
                        .Count();
                if (count == 0) ItemCountString = "";
                else ItemCountString = count.ToString();
            };

            LoadItem();
        }

        public async Task OnItemAppearing(Cleaning cleaning)
        {
            if (cleaning == Items.Last() && ItemCountString != "" && int.Parse(ItemCountString) != Items.Count)
            {
                // ObservableCollection にデータを追加する処理
                IsLoading = true;
                await Task.Run(() => LoadItem());
                IsLoading = false;
            }
        }

        private void LoadItem()
        {
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning()
                    .Where(c => !c.Done)
                    .OrderByDescending(c => c.Created.Ticks)
                    .Skip(Items.Count)
                    .Take(loadingCount))
                {
                    Items.Add(c);
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
