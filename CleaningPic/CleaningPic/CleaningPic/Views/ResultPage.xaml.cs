using CleaningPic.CustomViews;
using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class ResultPage : ContentPage
	{
        public ResultPage(byte[] imageData, Cleaning[] cleanings)
		{
			InitializeComponent();
            // TODO: 後で綺麗にする
            resultImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;
            CreateDynamicLayout(cleanings);
        }

        public void GoBackTop_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        private void CreateDynamicLayout(IList<Cleaning> cleaningList)
        {
            var bc = (BindingContext as ResultViewModel);
            var first = cleaningList[0];
            var goDetailRecognizer = new TapGestureRecognizer { Command = new Command(() => Navigation.PushAsync(new DetailPage(first))) };

            dirtLabel.Text = first.Dirt;
            firstLabel.Text = $"{first.Place.DisplayName()}の{first.Dirt}の落とし方";
            firstCleaningView.DirtOrPlace = first.Dirt;
            firstCleaningView.ToolsString = first.ToolsString;
            firstCleaningView.CleaningTime = first.CleaningTime;
            firstCleaningView.DirtImageIsVisible = false;
            firstCleaningView.AddIsVisible = true;
            firstCleaningView.AddCommand = bc.CleaningAddCommand;
            firstCleaningView.AddParam = first;
            firstCleaningView.GestureRecognizers.Add(goDetailRecognizer);

            foreach (var c in cleaningList.Skip(1))
            {
                var r = new TapGestureRecognizer { Command = new Command(() => Navigation.PushAsync(new DetailPage(c))) };
                var view = new CleaningView()
                {
                    DirtOrPlace = c.Dirt,
                    ToolsString = c.ToolsString,
                    CleaningTime = c.CleaningTime,
                    DirtImageIsVisible = false,
                    AddIsVisible = true,
                    AddCommand = bc.CleaningAddCommand,
                    AddParam = c,
                };
                view.GestureRecognizers.Add(r);
                otherStack.Children.Add(view);
            }
        }
    }
}