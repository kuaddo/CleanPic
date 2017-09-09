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
	public partial class WantToDoPage : ContentPage
	{
        ObservableCollection<CleaningMethod> Items = new ObservableCollection<CleaningMethod>();

		public WantToDoPage()
		{
			InitializeComponent();
            listView.ItemsSource = Items;
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

            Items.Add(new CleaningMethod()
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