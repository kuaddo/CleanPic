using CleaningPic.CustomViews;
using CleaningPic.Data;
using CleaningPic.Utils;
using CleaningPic.ViewModels;
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

        private void CreateDynamicLayout(IList<Cleaning> cleaningList)
        {
            var bc = (BindingContext as ResultViewModel);
            var first = cleaningList[0];

            dirtLabel.Text = first.Dirt;
            firstLabel.Text = $"{first.Place.DisplayName()}の{first.Dirt}の落とし方";
            firstCleaningView.DirtOrPlace = first.Dirt;
            firstCleaningView.ToolsString = first.ToolsString;
            firstCleaningView.CleaningTime = first.CleaningTime;
            firstCleaningView.DirtImageIsVisible = false;
            firstCleaningView.AddIsVisible = true;
            firstCleaningView.AddCommand = bc.CleaningAddCommand;
            firstCleaningView.AddParam = first;

            foreach (var c in cleaningList.Skip(1))
            {
                var view = new CleaningView()
                {
                    DirtOrPlace = c.Dirt,
                    ToolsString = c.ToolsString,
                    CleaningTime = c.CleaningTime,
                    DirtImageIsVisible = false,
                    AddIsVisible = true,
                    AddCommand = bc.CleaningAddCommand,
                    AddParam = c
                };
                otherStack.Children.Add(view);
            }
        }
    }
}