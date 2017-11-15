using CleaningPic.Data;
using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class DetailPage : ContentPage
	{
        private Cleaning cleaning;
        private bool showsWantToDo;
        private bool showsDone;
        private bool CheckedWantToDo => addCancelLayout.IsVisible;
        private bool CheckedDone => doneCancelLayout.IsVisible;

		public DetailPage(Cleaning cleaning, bool showsWantToDo, bool showsDone)
		{
			InitializeComponent();
            this.cleaning = cleaning;
            this.showsWantToDo = showsWantToDo;
            this.showsDone = showsDone;
            CreateDynamicLayout();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (!showsWantToDo && showsDone && !CheckedDone)
                return;

            if (CheckedWantToDo)
                cleaning.Done = false;
            if (CheckedDone)
                cleaning.Done = true;

            if (CheckedWantToDo || CheckedDone)
            {
                cleaning.Created = DateTimeOffset.UtcNow;
                using (var ds = new DataSource()) ds.UpdateCleaning(cleaning);
            }

            if (!CheckedWantToDo && !CheckedDone)
            {
                using (var ds = new DataSource())
                {
                    if (ds.Exists(cleaning))
                        ds.RemoveCleaning(cleaning);
                }
            }
        }

        private void CreateDynamicLayout()
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

            footer.IsVisible = showsDone || showsWantToDo;
            cautionLayout.IsVisible = cleaning.Caution.Length != 0;
            amazonLayout.IsVisible = hasLink;

            if (showsWantToDo && showsDone)
            {
                adds.IsVisible = true;
                if (cleaning.Done)
                {
                    addLayout.IsVisible = true;
                    doneCancelLayout.IsVisible = true;
                }
                else
                {
                    doneLayout.IsVisible = true;
                    using (var ds = new DataSource())
                    {
                        if (ds.Exists(cleaning))
                            addCancelLayout.IsVisible = true;
                        else
                            addLayout.IsVisible = true;
                    }
                }
                SetAddAndDoneRecognizer();
            }
            else if (showsDone)
            {
                addLayout.IsVisible = true;
                doneLayout.IsVisible = true;
                adds.BackgroundColor = Color.FromHex("#E6E6E6");
                SetDoneRecognizer();
            }
        }

        private void SetAddAndDoneRecognizer()
        {
            var addRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    addLayout.IsVisible = false;
                    addCancelLayout.IsVisible = true;
                    doneLayout.IsVisible = true;
                    doneCancelLayout.IsVisible = false;
                    DependencyService.Get<IFormsToast>().Show($"{cleaning.ToString()}をやりたいに追加しました");
                })
            };
            var addCancelRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    addLayout.IsVisible = true;
                    addCancelLayout.IsVisible = false;
                    DependencyService.Get<IFormsToast>().Show($"{cleaning.ToString()}をやりたいから削除しました");
                })
            };
            var doneRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    addLayout.IsVisible = true;
                    addCancelLayout.IsVisible = false;
                    doneLayout.IsVisible = false;
                    doneCancelLayout.IsVisible = true;
                    DependencyService.Get<IFormsToast>().Show($"{cleaning.ToString()}をやったに追加しました");
                })
            };
            var doneCancelRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    doneLayout.IsVisible = true;
                    doneCancelLayout.IsVisible = false;
                    DependencyService.Get<IFormsToast>().Show($"{cleaning.ToString()}をやったから削除しました");
                })
            };

            addLayout.GestureRecognizers.Add(addRecognizer);
            addCancelLayout.GestureRecognizers.Add(addCancelRecognizer);
            doneLayout.GestureRecognizers.Add(doneRecognizer);
            doneCancelLayout.GestureRecognizers.Add(doneCancelRecognizer);
        }

        private void SetDoneRecognizer()
        {
            var doneRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    doneLayout.IsVisible = false;
                    doneCancelLayout.IsVisible = true;
                    DependencyService.Get<IFormsToast>().Show($"{cleaning.ToString()}を掃除しました");
                })
            };
            var doneCancelRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    doneLayout.IsVisible = true;
                    doneCancelLayout.IsVisible = false;
                    DependencyService.Get<IFormsToast>().Show($"{cleaning.ToString()}をやりたいに戻しました");
                })
            };

            doneLayout.GestureRecognizers.Add(doneRecognizer);
            doneCancelLayout.GestureRecognizers.Add(doneCancelRecognizer);
        }
    }
}