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
	public partial class ResultPage : ContentPage
	{
        public ResultPage(byte[] imageData, Cleaning[] methods)
		{
			InitializeComponent();
            // TODO: 後で綺麗にする
            resultImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;
            listView.ItemsSource = methods.ToList();
        }
    }
}