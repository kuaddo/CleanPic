using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TopPage : CarouselPage
	{
		public TopPage ()
		{
			InitializeComponent();
            CurrentPageChanged += async (sender, e) =>
            {
                if (CurrentPage is CameraPage cameraPage)
                {
                    var source = await cameraPage.LaunchCamera();
                    // カメラで写真を取れなかった時に元の画面に戻る
                    if (source != null)
                        await Navigation.PushAsync(new UploadPage(source));
                    else
                        CurrentPage = topPage;
                }
            };
		}

        public void CameraMenu_Clicked(object sender, EventArgs e)
        {
            CurrentPage = cameraPage;
        }
    }
}