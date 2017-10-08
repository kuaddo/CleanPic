using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        public Command CleaningDoneCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }

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

            // データの読み込み
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

        public async Task OnItemAppearing(Cleaning cleaning)
        {
            if (cleaning == Items.Last() && ItemCountString != "" && int.Parse(ItemCountString) != Items.Count)
            {
                // ObservableCollection にデータを追加する処理
                IsLoading = true;
                await Task.Run(() =>
                {
                    using (var ds = new DataSource())
                        foreach (var c in ds.ReadAllCleaning()
                            .Where(c => !c.Done)
                            .OrderByDescending(c => c.Created.Ticks)
                            .Skip(Items.Count)
                            .Take(loadingCount))
                        {
                            // ここのAddが重すぎて、非同期にするメリットが殆ど無い
                            Items.Add(c);
                        }
                });
                IsLoading = false;
            }
        }
    }
}
