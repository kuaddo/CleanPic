using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
	public partial class CameraPage : ContentPage
	{
		public CameraPage()
		{
			InitializeComponent();
        }

        public async Task<byte[]> LaunchCamera()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo != null)
            {
                using (var stream = photo.GetStream())
                using (var ms = new MemoryStream())
                {
                    if (stream == null) return null;
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            return null;
        }
    }
}