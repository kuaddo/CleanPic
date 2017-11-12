using CleaningPic.Data;
using CleaningPic.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class DoneViewModel : BindableBase
    {
        public ObservableCollection<Cleaning> Items { get; set; } = new ObservableCollection<Cleaning>();

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }
        private int ItemCount { get; set; }
        
        public const string navigateWebBrowserMessage = "navigateWebBrowserMessage";

        public Command CleaningShoppingCommand { get; private set; }
        public Command CleaningRemoveCommand { get; private set; }

        private const int loadingCount = 10;

        public DoneViewModel()
        {
            // やりたいページで完了したものをMessageで確認、やったページに追加
            MessagingCenter.Subscribe<WantToDoViewModel, Cleaning>(
                this,
                WantToDoViewModel.cleaningDoneMessage,
                (sender, cleaning) => { Items.Add(cleaning); });

            CleaningShoppingCommand = new Command<Cleaning>(c =>
            {
                MessagingCenter.Send(this, navigateWebBrowserMessage, c);
            });

            CleaningRemoveCommand = new Command<Cleaning>(c =>
            {
                using (var ds = new DataSource())
                    ds.RemoveCleaning(c);
                Items.Remove(c);
                DependencyService.Get<IFormsToast>().Show(c.ToString() + "を削除しました");
            });

            Items.CollectionChanged += (sender, e) =>
            {
                using (var ds = new DataSource())
                    ItemCount = ds.ReadAllCleaning()
                        .Where(c => c.Done)
                        .Count();
            };

            LoadItem();
        }

        public async Task OnItemAppearing(Cleaning cleaning)
        {
            if (cleaning == Items.Last() && ItemCount != Items.Count)
            {
                // ObservableCollection にデータを追加する処理
                IsLoading = true;
                await Task.Run(() => LoadItem());
                IsLoading = false;
            }
        }

        private void LoadItem()
        {
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning()
                    .Where(c => c.Done)
                    .OrderByDescending(c => c.Created.Ticks)
                    .Skip(Items.Count)
                    .Take(loadingCount))
                {
                    Items.Add(c);
                }
        }
    }
}
