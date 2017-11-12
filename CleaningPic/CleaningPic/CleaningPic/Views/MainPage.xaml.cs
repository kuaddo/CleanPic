using System;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            // MainPageはルートなのでUnsubscribeする必要はない
            MessagingCenter.Subscribe<TopPage>(
                this,
                TopPage.navigateWantToDoPageMessage,
                (sender) => Navigate(new MainPageMenuItem { Id = 1, Title = "やりたい", TargetType = typeof(CleaningTabbedPage), Params = new object[] { true } }));
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageMenuItem;
            if (item == null)
                return;
            Navigate(item);
        }

        private void Navigate(MainPageMenuItem item)
        {
            Page page = (Page)Activator.CreateInstance(item.TargetType, item.Params);
            page.Title = item.Title;
            Detail = new NavigationPage(page) {
                BarBackgroundColor = Color.FromHex("#338DD0"),
                BarTextColor = Color.White
            };
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}