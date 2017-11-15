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
        private int changedItemIndex = -1;
        private IList<Cleaning> cleaningList;

        public ResultPage(byte[] imageData, Cleaning[] cleanings)
		{
			InitializeComponent();
            cleaningList = cleanings;
            resultImage.Source = new ImageConverter().Convert(imageData, null, null, null) as ImageSource;
            CreateDynamicLayout();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (changedItemIndex != -1)
            {
                var c = cleaningList[changedItemIndex];
                CleaningView view;
                bool addDone;
                using (var ds = new DataSource())
                    addDone = ds.Exists(c);

                if (changedItemIndex == 0)
                    view = firstCleaningView;
                else
                    view = otherStack.Children[changedItemIndex - 1] as CleaningView;
                
                view.ShowsAddLayout = !addDone;
                view.ShowsAddCancelLayout = addDone;

                changedItemIndex = -1;
            }
        }

        public void GoBackTop_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        private void CreateDynamicLayout()
        {
            var bc = (BindingContext as ResultViewModel);
            var first = cleaningList[0];
            var goDetailRecognizer = new TapGestureRecognizer { Command = new Command(() =>
            {
                changedItemIndex = 0;
                Navigation.PushAsync(new DetailPage(first, true, true));
            })};

            dirtLabel.Text = first.Dirt;
            firstLabel.Text = $"{first.Place.DisplayName()}の{first.Dirt}の落とし方";

            firstCleaningView.DirtImageIsVisible = false;
            firstCleaningView.AddIsVisible = true;
            firstCleaningView.AddCommand = bc.CleaningAddCommand;
            firstCleaningView.AddCancelCommand = bc.CleaningAddCancelCommand;
            firstCleaningView.BindingContext = first;
            firstCleaningView.GestureRecognizers.Add(goDetailRecognizer);

            foreach (var c in cleaningList.Skip(1))
            {
                var r = new TapGestureRecognizer { Command = new Command(() =>
                {
                    changedItemIndex = cleaningList.IndexOf(c);
                    Navigation.PushAsync(new DetailPage(c, true, true));
                }) };
                var view = new CleaningView()
                {
                    DirtImageIsVisible = false,
                    AddIsVisible = true,
                    AddCommand = bc.CleaningAddCommand,
                    AddCancelCommand = bc.CleaningAddCancelCommand,
                    BindingContext = c
                };
                view.GestureRecognizers.Add(r);
                otherStack.Children.Add(view);
            }
        }
    }
}