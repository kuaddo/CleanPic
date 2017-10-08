using CleaningPic.Data;
using CleaningPic.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CleaningPic.Views
{
	public partial class WantToDoPage : ContentPage
	{
        public WantToDoPage()
        {
            InitializeComponent();
        }

        public async void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await (BindingContext as WantToDoViewModel).OnItemAppearing(e.Item as Cleaning);
        }
    }
}