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
    public partial class CleaningTabbedPage : TabbedPage
    {
        public CleaningTabbedPage(bool startWantToDoPage)
        {
            InitializeComponent();
            wantToDoPage.Title = "やりたい";
            donePage.Title = "やった";
            if (startWantToDoPage)
                CurrentPage = wantToDoPage;
            else
                CurrentPage = donePage;
        }
    }
}