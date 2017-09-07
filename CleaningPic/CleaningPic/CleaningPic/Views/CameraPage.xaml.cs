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
	public partial class CameraPage : ContentPage
	{
		public CameraPage()
		{
			InitializeComponent();
        }

        public async Task<ImageSource> LaunchCamera()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo != null)
                return ImageSource.FromStream(() => { return photo.GetStream(); });
            else
                return null;
        }
    }
}