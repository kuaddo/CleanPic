using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class ResultViewModel
    {
        public Command CleaningAddCommand { get; private set; }
        public Command CleaningAddCancelCommand { get; set; }

        public ResultViewModel()
        {
            CleaningAddCommand = new Command<Cleaning>(c =>
            {
                c.Created = DateTimeOffset.UtcNow;
                // Realmに追加する処理
                using (var ds = new DataSource())
                {
                    if (!ds.Exists(c))
                    {
                        ds.AddCleaning(c);
                        DependencyService.Get<IFormsToast>().Show(c.ToString() + "を追加しました");
                    }
                }
            });

            CleaningAddCancelCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });
        }
    }
}
