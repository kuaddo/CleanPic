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
	public partial class WantToDoPage : ContentPage
	{
        ObservableCollection<Cleaning> Items = new ObservableCollection<Cleaning>();
        // BindablePropertyを利用してTabbedPageのバッチとバインドできるようにする
        public static readonly BindableProperty ItemCountStringProperty = BindableProperty.Create(
            "ItemCountString",
            typeof(string),
            typeof(WantToDoPage));
        public string ItemCountString
        {
            get { return (string)GetValue(ItemCountStringProperty); }
            set { SetValue(ItemCountStringProperty, value); }
        }

        public WantToDoPage()
        {
            InitializeComponent();
            using (var ds = new DataSource())
                foreach (var c in ds.ReadAllCleaning().Where(c => !c.Done))
                    Items.Add(c);
            listView.ItemsSource = Items;
            if (Items.Count != 0)
                ItemCountString = Items.Count.ToString();
            // Itemsが変化した時にItemCountStringを更新するようにする
            Items.CollectionChanged += (sender, e) =>
            {
                if (Items.Count == 0) ItemCountString = "";
                else ItemCountString = Items.Count.ToString();
            };
        }
    }
}