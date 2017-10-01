using CleaningPic.Data;
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

        public Command CleaningDoneCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }

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

            // データの読み込み
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning().Where(c => !c.Done))
                    Items.Add(c);
        }
    }
}
