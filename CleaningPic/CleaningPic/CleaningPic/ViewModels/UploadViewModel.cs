using CleaningPic.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class UploadViewModel : BindableBase
    {
        public const string navigateResultPageMessage = "navigateResultPageMessage";
        private Place cleaningPlace;
        private bool isUploading = false;

        public Place CleanigPlace
        {
            get { return cleaningPlace; }
            set { SetProperty(ref cleaningPlace, value); }
        }

        public bool IsUploading
        {
            get { return isUploading; }
            set
            {
                SetProperty(ref isUploading, value);
                UploadCommand.ChangeCanExecute();   // 実行可能変更通知
            }
        }

        public byte[] ImageData { get; set; }
        public Command UploadCommand { get; private set; }

        public UploadViewModel()
        {
            UploadCommand = new Command(async () =>
            {
                IsUploading = true;
                // 実際の通信処理をここに書く
                await Task.Delay(3000);

                var now = DateTimeOffset.UtcNow;
                var args = new List<Cleaning>()
                {
                    new Cleaning() { Place = cleaningPlace, Dirt = "汚れ1", ImageData = ImageData, Method = "手法1", Caution = "注意点1", Tools = new List<string> { "道具1", "道具1'" }, Created = now, CleaningTime = 10,  CanNotify = false, NotificationDate = now },
                    new Cleaning() { Place = cleaningPlace, Dirt = "汚れ2", ImageData = ImageData, Method = "手法2", Caution = "注意点2", Tools = new List<string> { "道具2", "道具2'" }, Created = now, CleaningTime = 50,  CanNotify = true,  NotificationDate = now },
                    new Cleaning() { Place = cleaningPlace, Dirt = "汚れ3", ImageData = ImageData, Method = "手法3", Caution = "注意点3", Tools = new List<string> { "道具3", "道具3'" }, Created = now, CleaningTime = 30,  CanNotify = false, NotificationDate = now },
                    new Cleaning() { Place = cleaningPlace, Dirt = "汚れ4", ImageData = ImageData, Method = "手法4", Caution = "注意点4", Tools = new List<string> { "道具4", "道具4'" }, Created = now, CleaningTime = 120, CanNotify = false, NotificationDate = now },
                    new Cleaning() { Place = cleaningPlace, Dirt = "汚れ5", ImageData = ImageData, Method = "手法5", Caution = "注意点5", Tools = new List<string> { "道具5", "道具5'" }, Created = now, CleaningTime = 150, CanNotify = true,  NotificationDate = now }
                }.ToArray();

                // 画面遷移のメッセージ
                MessagingCenter.Send(this, navigateResultPageMessage, args);
                IsUploading = false;    // 画面遷移中にアップロードボタンがオンになるが妥協する
            }, () => !IsUploading);
        }
    }
}
