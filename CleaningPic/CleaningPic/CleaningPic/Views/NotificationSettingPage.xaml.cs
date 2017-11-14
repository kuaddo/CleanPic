using CleaningPic.Data;
using CleaningPic.ViewModels;
using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class NotificationSettingPage : ContentPage
	{
        // OnDisappearingで処理がしたい為、仕方なくVMを所持
        private NotificationSettingViewModel vm;

		public NotificationSettingPage(Cleaning cleaning)
		{
			InitializeComponent();
            vm = (BindingContext as NotificationSettingViewModel);
            vm.Cleaning = cleaning;
        }

        // 画面を離れる際にデータの保存処理をする
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.NotificationStateChanged();
        }
    }
}