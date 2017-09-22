using CleaningPic.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class ResultViewModel
    {
        public Command AddNotDoneCommand { get; private set; }
        public Command AddDoneCommand { get; private set; }

        public ResultViewModel()
        {
            AddNotDoneCommand = new Command<Cleaning>(cleaning =>
            {
                AddCleaning(cleaning);
            });

            AddDoneCommand = new Command<Cleaning>(cleaning =>
            {
                cleaning.Done = true;
                AddCleaning(cleaning);
            });
        }

        private void AddCleaning(Cleaning c)
        {
            // Realmに追加する処理
            using (var ds = new DataSource())
            {
                if (ds.Exists(c))
                    Debug.WriteLine("このデータは存在しています");
                else
                    ds.AddCleaning(c);
            }
        }
    }
}
