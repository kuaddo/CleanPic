using CleaningPic.Data;
using CleaningPic.Utils;

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
            foreach (var tool in cleaning.Tools)
            {
                var text = "・" + tool;
                toolsLayout.Children.Add(new Label
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    Text = text
                });
                amazonLayout.Children.Add(new Label
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    Text = text,
                    TextColor = Color.FromHex("#338DD0")
                });
            }
            methodLabel.Text = cleaning.Method;
            cautionLayout.IsVisible = cleaning.Caution.Length != 0;
            cautionLabel.Text = cleaning.Caution;
        }
    }
}