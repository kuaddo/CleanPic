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
                    // カメラで写真を取れなかった時に元の画面に戻る
                    if (!await cameraPage.LaunchCamera())
                        CurrentPage = TopPageDetail;
                }
            };
		}
    }
}