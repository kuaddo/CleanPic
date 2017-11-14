using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class DetailPage : ContentPage
	{
		public DetailPage(Cleaning cleaning, bool showsWantToDo, bool showsDone)
		{
			InitializeComponent();
            CreateDynamicLayout(cleaning, showsWantToDo, showsDone);
        }

        private void CreateDynamicLayout(Cleaning cleaning, bool showsWantToDo, bool showsDone)
        {
            bool hasLink = false;
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

                if (!string.IsNullOrEmpty(link))
                {
                    var label = new Label
                    {
                        Margin = new Thickness(5, 0, 0, 0),
                        Text = text,
                        TextColor = Color.FromHex("#338DD0")
                    };

                    var recognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            DependencyService.Get<IWebBrowser>().Open(new Uri(link));
                        })
                    };
                    label.GestureRecognizers.Add(recognizer);
                    amazonLayout.Children.Add(label);
                    hasLink = true;
                }
            }
            
            methodLabel.Text = cleaning.Method;
            cautionLabel.Text = cleaning.Caution;

            cautionLayout.IsVisible = cleaning.Caution.Length != 0;
            amazonLayout.IsVisible = hasLink;

            if (showsWantToDo)
            {
                adds.IsVisible = true;
                using (var ds = new DataSource())
                {
                    if (ds.Exists(cleaning))
                        addCancelLayout.IsVisible = true;
                    else
                        addLayout.IsVisible = true;
                }
                SetAddRecognizer();
            }

            if (showsDone)
            {
                dones.IsVisible = true;
                SetDoneRecognizer();
            }
        }

        private void SetAddRecognizer()
        {
            var addRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    addLayout.IsVisible = false;
                    addCancelLayout.IsVisible = true;
                })
            };
            var addCancelRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    addLayout.IsVisible = true;
                    addCancelLayout.IsVisible = false;
                })
            };

            addLayout.GestureRecognizers.Add(addRecognizer);
            addCancelLayout.GestureRecognizers.Add(addCancelRecognizer);
        }

        private void SetDoneRecognizer()
        {
            var doneRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    doneLayout.IsVisible = false;
                    doneCancelLayout.IsVisible = true;
                })
            };
            var doneCancelRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    doneLayout.IsVisible = true;
                    doneCancelLayout.IsVisible = false;
                })
            };

            doneLayout.GestureRecognizers.Add(doneRecognizer);
            doneCancelLayout.GestureRecognizers.Add(doneCancelRecognizer);
        }
    }
}