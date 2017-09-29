using CleaningPic.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class ResultViewModel
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();

        public Command CleaningAddCommand { get; private set; }

        public ResultViewModel()
        {
            CleaningAddCommand = new Command<Cleaning>(c =>
            {
                c.Created = DateTimeOffset.UtcNow;
                // Realmに追加する処理
                using (var ds = new DataSource())
                {
                    if (ds.Exists(c))
                        Debug.WriteLine("このデータは存在しています");   // TODO: トーストか何かでユーザに伝える必要がある
                    else
                        ds.AddCleaning(c);
                }
            });
        }
    }
}
