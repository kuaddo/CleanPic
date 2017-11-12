using CleaningPic.Data;
using CleaningPic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CleaningPic.ViewModels
{
    public class UploadViewModel : BindableBase
    {
        public const string navigateResultPageMessage = "navigateResultPageMessage_UploadViewModel";
        public const string connectionFailedMessage = "connectionFailedMessage_UploadViewModel";
        private Place cleaningPlace;
        private bool isUploading = false;

        public Place CleaningPlace
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
                var now = DateTimeOffset.UtcNow;
                Cleaning[] args = null;

                if (PropSource.UseOffline)
                {
                    await Task.Delay(1500);
                    args = new List<Cleaning>()
                    {
                        new Cleaning() { Place = CleaningPlace, Dirt = "汚れ1", ImageData = ImageData, Method = "手法1", Caution = "注意点1", Tools = new List<string> { "道具1", "道具1'" },                        Created = now, CleaningTime = 10,  NotificationDate = now },
                        new Cleaning() { Place = CleaningPlace, Dirt = "汚れ2", ImageData = ImageData, Method = "手法2", Caution = "",        Tools = new List<string> { "道具2", "道具2'", "道具2''", "道具2'''" }, Created = now, CleaningTime = 50,  NotificationDate = now },
                        new Cleaning() { Place = CleaningPlace, Dirt = "汚れ3", ImageData = ImageData, Method = "手法3", Caution = "注意点3", Tools = new List<string> { "道具3" },                                  Created = now, CleaningTime = 30,  NotificationDate = now },
                        new Cleaning() { Place = CleaningPlace, Dirt = "汚れ4", ImageData = ImageData, Method = "手法4", Caution = "注意点4", Tools = new List<string> { "道具4", "道具4'" },                        Created = now, CleaningTime = 120, NotificationDate = now },
                        new Cleaning() { Place = CleaningPlace, Dirt = "汚れ5", ImageData = ImageData, Method = "手法5", Caution = "",        Tools = new List<string> { "道具5", "道具5'" },                        Created = now, CleaningTime = 150, NotificationDate = now }
                    }.ToArray();
                }
                else
                {
                    var response = await HttpUtils.PostDirtImageAsync(ImageData, CleaningPlace);
                    if (response == null)
                    {
                        ConnectionFailed();
                        return;
                    }
                    var resultIds = response.Split(',').Select(r => int.Parse(r)).ToArray();
                    response = await HttpUtils.GetResult();
                    if (response == null)
                    {
                        ConnectionFailed();
                        return;
                    }
                    args = GetCleanings(response, resultIds, now);
                }

                // 画面遷移のメッセージ
                MessagingCenter.Send(this, navigateResultPageMessage, args);
                IsUploading = false;    // 画面遷移中にアップロードボタンがオンになるが妥協する
            }, () => !IsUploading);
        }

        private void ConnectionFailed()
        {
            IsUploading = false;
            MessagingCenter.Send(this, connectionFailedMessage);
        }

        private Cleaning[] GetCleanings(string json, int[] resultIds, DateTimeOffset dt)
        {
            var results = JsonConvert.DeserializeObject<IList<Result>>(json).Where(r => resultIds.Contains(r.ID));
            return results.Select(r => new Cleaning
            {
                Place =            CleaningPlace,
                Dirt =             r.Category.Name,
                ImageData =        ImageData,
                Method =           r.Text,
                Caution =          r.CautionText,
                Tools =            r.Tools.Select(t => t.Name).ToList(),
                Links =            r.Tools.Select(t => t.Link).ToList(),
                Created =          dt,
                CleaningTime =     r.TimeToFinish,
                NotificationDate = dt
            }).ToArray();
        }
    }
}
