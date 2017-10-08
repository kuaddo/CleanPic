using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class DoneViewModel : BindableBase
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();

        public Command CleaningRemoveCommand { get; private set; }

        public DoneViewModel()
        {
            // やりたいページで完了したものをMessageで確認、やったページに追加
            MessagingCenter.Subscribe<WantToDoViewModel, Cleaning>(
                this,
                WantToDoViewModel.cleaningDoneMessage,
                (sender, cleaning) => { Items.Add(cleaning); });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
                Items.Remove(c);
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });

            // データの読み込み
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning().Where(c => c.Done).OrderByDescending(c => c.Created.Ticks))
                    Items.Add(c);
        }
    }
}
