using CleaningPic.Data;
using CleaningPic.Models;
using CleaningPic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class DoneViewModel : BindableBase
    {
        public ObservableCollection<GroupingItem> Items { get; set; } = new ObservableCollection<GroupingItem>();

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }
        private int ItemCount { get; set; }
        
        public const string navigateWebBrowserMessage = "navigateWebBrowserMessage_doneViewModel";

        public Command CleaningShoppingCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }
        
        public DoneViewModel()
        {
            // やりたいページで完了したものをMessageで確認、やったページに追加
            MessagingCenter.Subscribe<WantToDoViewModel, Cleaning>(
                this,
                WantToDoViewModel.cleaningDoneMessage,
                (sender, cleaning) => LoadItem());

            CleaningShoppingCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateWebBrowserMessage, c);
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
                LoadItem();
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });
        }

        public void OnAppearing()
        {
            // データ読み込み
            LoadItem();
        }
        
        private void LoadItem()
        {
            Items.Clear();
            foreach (int val in Enum.GetValues(typeof(Place)))
                Items.Add(new GroupingItem { PlaceLabel = ((Place)val).DisplayName() });

            using (var ds = new DataSource())
            {
                var cleanings = ds.ReadAllCleaning()
                    .Where(c => !c.Done)
                    .OrderByDescending(c => c.Created.Ticks);

                foreach (var c in cleanings)
                {
                    foreach (var g in Items)
                        if (g.PlaceLabel == c.Place.DisplayName())
                            g.Add(c);
                }
            }

            List<GroupingItem> removeItems = new List<GroupingItem>();
            foreach (var g in Items)
                if (g.Count == 0)
                    removeItems.Add(g);

            foreach (var g in removeItems)
                Items.Remove(g);
        }
    }
}
