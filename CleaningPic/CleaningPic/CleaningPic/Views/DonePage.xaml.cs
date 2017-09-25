using CleaningPic.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DonePage : ContentPage
	{
        ObservableCollection<Cleaning> Items = new ObservableCollection<Cleaning>();

        public DonePage()
        {
            InitializeComponent();
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning().Where(c => c.Done))
                    Items.Add(c);
            listView.ItemsSource = Items;
        }
    }
}