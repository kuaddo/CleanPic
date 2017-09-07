using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class UploadViewModel : BindableBase
    {
        private Place cleaningPlace;

        public Place CleanigPlace
        {
            get { return cleaningPlace; }
            set { SetProperty(ref cleaningPlace, value); }
        }
    }
}
