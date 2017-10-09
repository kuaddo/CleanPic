using CleaningPic.Data;
using CleaningPic.Views;
using System;
using System.Collections.Generic;
using System.Text;
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
                    new Cleaning() { Dirt = "汚れ1", ImageData = null, Method = "手法1", Tools = new List<string> { "道具1", "道具1'" }, Created = now, Probability = 0.7,  CleaningTime = 10 },
                    new Cleaning() { Dirt = "汚れ2", ImageData = null, Method = "手法2", Tools = new List<string> { "道具2", "道具2'" }, Created = now, Probability = 0.13, CleaningTime = 50  },
                    new Cleaning() { Dirt = "汚れ3", ImageData = null, Method = "手法3", Tools = new List<string> { "道具3", "道具3'" }, Created = now, Probability = 0.11, CleaningTime = 30  },
                    new Cleaning() { Dirt = "汚れ4", ImageData = null, Method = "手法4", Tools = new List<string> { "道具4", "道具4'" }, Created = now, Probability = 0.05, CleaningTime = 120  },
                    new Cleaning() { Dirt = "汚れ5", ImageData = null, Method = "手法5", Tools = new List<string> { "道具5", "道具5'" }, Created = now, Probability = 0.01, CleaningTime = 150  }
                }.ToArray();

                // 画面遷移のメッセージ
                MessagingCenter.Send(this, navigateResultPageMessage, args);
                IsUploading = false;    // 画面遷移中にアップロードボタンがオンになるが妥協する
            }, () => !IsUploading);
        }
    }
}
