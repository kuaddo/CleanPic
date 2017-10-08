using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class WantToDoViewModel : BindableBase
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();
        private string itemCountString;
        public string ItemCountString
        {
            get { return itemCountString; }
            set { SetProperty(ref itemCountString, value); }
        }
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
                if (Items.Count == 0) ItemCountString = "";
                else ItemCountString = Items.Count.ToString();
            };

            // データの読み込み
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning().Where(c => !c.Done).OrderByDescending(c => c.Created.Ticks))
                    Items.Add(c);
        }
    }
}
