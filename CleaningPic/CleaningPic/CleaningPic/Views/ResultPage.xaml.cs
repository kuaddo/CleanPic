﻿using CleaningPic.Data;
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
        ObservableCollection<Cleaning> Items = new ObservableCollection<Cleaning>();

        public ResultPage(byte[] imageData, Cleaning[] methods)
		{
			InitializeComponent();
            resultImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;
            foreach (var method in methods) Items.Add(method);
            listView.ItemsSource = Items;
        }
    }
}