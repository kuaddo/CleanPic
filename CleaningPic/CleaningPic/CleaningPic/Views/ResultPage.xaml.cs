using CleaningPic.Data;
using CleaningPic.Utils;
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
	public partial class ResultPage : ContentPage
	{
        ObservableCollection<CleaningMethod> Items = new ObservableCollection<CleaningMethod>();

        public ResultPage(byte[] imageData, CleaningMethod[] methods)
		{
			InitializeComponent();
            resultImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;
            foreach (var method in methods) Items.Add(method);
            listView.ItemsSource = Items;
        }

        public void CleanAfter_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Clean after button is clicked");
        }

        public void CleanNow_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Clean now button is clicked");
        }
    }
}