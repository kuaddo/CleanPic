using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class DetailPage : ContentPage
	{
		public DetailPage(Cleaning cleaning)
		{
			InitializeComponent();
            CreateDynamicLayout(cleaning);
        }

        private void CreateDynamicLayout(Cleaning cleaning)
        {
            titleLabel.Text = cleaning.ToString() + "の落とし方";
            dirtImage.Source = new ImageConverter().Convert(cleaning.ImageData, null, null, null) as ImageSource;
            for (int i = 0; i < cleaning.Tools.Count; i++)
            {
                var tool = cleaning.Tools[i];
                var link = cleaning.Links[i];
                var text = "・" + tool;
                toolsLayout.Children.Add(new Label
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    Text = text
                });

                var label = new Label
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    Text = text,
                    TextColor = Color.FromHex("#338DD0")
                };
                if (!string.IsNullOrEmpty(link))
                {
                    var recognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            DependencyService.Get<IWebBrowser>().Open(new Uri(link));
                        })
                    };
                    label.GestureRecognizers.Add(recognizer);
                }
                amazonLayout.Children.Add(label);
            }
            methodLabel.Text = cleaning.Method;
            cautionLayout.IsVisible = cleaning.Caution.Length != 0;
            cautionLabel.Text = cleaning.Caution;
        }
    }
}