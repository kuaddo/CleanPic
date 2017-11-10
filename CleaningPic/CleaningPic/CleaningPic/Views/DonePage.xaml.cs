using CleaningPic.Data;
using CleaningPic.ViewModels;
using System;

using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class DonePage : ContentPage
	{
        public DonePage()
        {
            InitializeComponent();
        }

        public async void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await (BindingContext as DoneViewModel).OnItemAppearing(e.Item as Cleaning);
        }

        public void ListViewItem_Clicked(object sender, EventArgs e)
        {
            listView.SelectedItem = null;
        }
    }
}