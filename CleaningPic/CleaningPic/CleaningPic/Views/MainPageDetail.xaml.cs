using CleaningPic.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CleaningPic.Views
{
    public partial class MainPageDetail : ContentPage
    {
        private const string API = "http://35.189.147.104/api/";
        private static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(3000) };

        public MainPageDetail()
        {
            InitializeComponent();
            offlineUseSwitch.IsToggled = PropSource.UseOffline;
        }

        public async void GetButton_Clicked(object sender, EventArgs e)
        {
            if (apiPicker.SelectedIndex == -1) return;

            apiPicker.IsEnabled = false;
            getButton.IsEnabled = false;
            indicator.IsVisible = true;
            HttpResponseMessage result = null;

            try
            {
                result = await client.GetAsync(API + apiPicker.SelectedItem.ToString());
            }
            catch (TaskCanceledException _)
            {
                resultLabel.Text = "タイムアウト";
                indicator.IsVisible = false;
                apiPicker.IsEnabled = true;
                getButton.IsEnabled = true;
                return;
            }

            if (result?.StatusCode == System.Net.HttpStatusCode.OK)
                resultLabel.Text = await result.Content.ReadAsStringAsync();
            else
                resultLabel.Text = "通信失敗";

            indicator.IsVisible = false;
            apiPicker.IsEnabled = true;
            getButton.IsEnabled = true;
        }

        public async void PostButton_Clicked(object sender, EventArgs e)
        {
            apiPicker.IsEnabled = false;
            getButton.IsEnabled = false;
            indicator.IsVisible = true;
            HttpResponseMessage result = null;
            byte[] imageData = null;
            var content = new MultipartFormDataContent();
            var fileName = DateTime.Now.ToString("yyyy_MM_dd-HH:mm:ss") + ".jpg";

            using (var ds = new DataSource())
            {
                imageData = ds.ReadAllCleaning()[0].ImageData;
                content.Add(new ByteArrayContent(imageData), "upload_file", fileName);
                content.Add(new StringContent("114514"), "category_id");
            }
            
            postContentLabel.Text = content.Headers.ContentType.ToString() + await content.ReadAsStringAsync();

            try
            {
                result = await client.PostAsync("http://35.189.147.104/sub_uploader/", content);
            }
            catch (TaskCanceledException _)
            {
                resultLabel.Text = "タイムアウト";
                indicator.IsVisible = false;
                apiPicker.IsEnabled = true;
                getButton.IsEnabled = true;
                return;
            }

            if (result?.StatusCode == System.Net.HttpStatusCode.OK || result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var response = await result.Content.ReadAsStringAsync();
                resultLabel.Text = response;
                Console.WriteLine(response);
            }
            else
                resultLabel.Text = "通信失敗 : StatusCode = " + result?.StatusCode + ", " + await result?.Content?.ReadAsStringAsync() + ", " + result?.Headers?.ToString();

            indicator.IsVisible = false;
            apiPicker.IsEnabled = true;
            getButton.IsEnabled = true;
        }

        public void OfflineUseSwitch_Toggled(object sender, EventArgs e)
        {
            PropSource.UseOffline = offlineUseSwitch.IsToggled;
        }

        public void DeleteButton_Clicked(object sender, EventArgs e)
        {
            using (var ds = new DataSource())
            {
                ds.RemoveAllCleaning();
            }
        }
    }
}