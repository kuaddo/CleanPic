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

        public void ListViewItem_Clicked(object sender, EventArgs e)
        {
            listView.SelectedItem = null;
        }
    }
}