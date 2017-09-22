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
        public Command AddCommand { get; private set; }
        public Command ShowCommand { get; private set; }

        public ResultViewModel()
        {
            AddCommand = new Command<Cleaning>(data =>
            {
                // Realmに追加する処理
                using (var ds = new DataSource())
                {
                    if (ds.Exists(data))
                        Debug.WriteLine("このデータは存在しています");
                    else
                        ds.AddCleaning(data);
                }
            });

            ShowCommand = new Command(() => 
            {
                using (var ds = new DataSource())
                {
                    foreach (var c in ds.ReadAllCleaning())
                    {
                        Debug.WriteLine($"Id = {c.Id}, Dirt = {c.Dirt}");
                    }
                }
            });
        }
    }
}
