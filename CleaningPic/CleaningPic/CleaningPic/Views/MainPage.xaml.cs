using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageMenuItem;
            if (item == null)
                return;

            Page page = (Page)Activator.CreateInstance(item.TargetType, item.Params);
            page.Title = item.Title;
            Detail = new NavigationPage(page) { BarBackgroundColor = Color.FromRgb(51, 141, 208) };
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}