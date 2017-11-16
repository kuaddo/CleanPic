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
    public class WantToDoViewModel : BindableBase
    {
        public ObservableCollection<GroupingItem> Items { get; set; } = new ObservableCollection<GroupingItem>();
        private string itemCountString;
        private bool isLoading = false;
        public bool Reversed { get; set; } = false; 
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
        
        public const string cleaningDoneMessage = "cleaningDoneMessage_WantToDoViewModel";
        public const string navigateNotificationSettingPageMessage = "navigateNotificationSettingPageMessage_WantToDoViewModel";
        public const string navigateWebBrowserMessage = "navigateWebBrowserMessage_WantToDoViewModel";

        public Command CleaningShoppingCommand { get; private set; }
        public Command CleaningDoneCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }
        public Command CleaningNotificationCommand { get; private set; }

        public WantToDoViewModel()
        {
            CleaningShoppingCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateWebBrowserMessage, c);
            });

            CleaningDoneCommand = new Command<Cleaning>(c =>
            {
                c.Done = true;
                c.Created = DateTimeOffset.UtcNow;
                using (var ds = new DataSource())
                    ds.UpdateCleaning(c);
                LoadItem();

                // Messageをやったページに送って、更新するようにする
                MessagingCenter.Send(this, cleaningDoneMessage, c);
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を完了しました");
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
                LoadItem();
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });

            CleaningNotificationCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateNotificationSettingPageMessage, c);
            });
        }

        public void OnAppearing()
        {
            // データ再読込
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
                    .Where(c => !c.Done);
                if (Reversed)
                    cleanings = cleanings.OrderBy(c => c.Created.Ticks);
                else
                    cleanings = cleanings.OrderByDescending(c => c.Created.Ticks);

                if (cleanings.Count() == 0) ItemCountString = "";
                else                        ItemCountString = cleanings.Count().ToString();

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
