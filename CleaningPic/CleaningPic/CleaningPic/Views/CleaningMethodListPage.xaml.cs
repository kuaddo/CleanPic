using CleaningPic.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CleaningMethodListPage : ContentPage
	{
        ObservableCollection<Cleaning> Items = new ObservableCollection<Cleaning>();
        
        // BindablePropertyを利用してTabbedPageのバッチとバインドできるようにする
        public static readonly BindableProperty ItemCountStringProperty = BindableProperty.Create(
            "ItemCountString",
            typeof(string),
            typeof(CleaningMethodListPage));
        public string ItemCountString
        {
            get { return (string)GetValue(ItemCountStringProperty); }
            set { SetValue(ItemCountStringProperty, value); }
        }

        public CleaningMethodListPage()
		{
			InitializeComponent();
            listView.ItemsSource = Items;
            // Itemsが変化した時にItemCountStringを更新するようにする
            Items.CollectionChanged += (sender, e) =>
            {
                if (Items.Count == 0) ItemCountString = "";
                else ItemCountString = Items.Count.ToString();
            };
        }

        private async void AddListItemButton_Clicked(object sender, EventArgs e)
        {
            var stream = await LaunchCamera();
            if (stream == null) return;
            byte[] byteArray;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byteArray = ms.ToArray();
            }

            Items.Add(new Cleaning()
            {
                Dirt = "汚れテスト",
                Method = "手法テスト",
                Tools = new List<string>() { "道具1", "道具2" },
                ImageData = byteArray,
                Created = DateTime.Now
            });
        }

        public async Task<Stream> LaunchCamera()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo != null)
                return photo.GetStream();
            else
                return null;
        }
    }
}