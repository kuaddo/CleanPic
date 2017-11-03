using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
    public partial class MainPageDetail : ContentPage
    {
        private const string API = "http://35.189.147.104/api/";
        private static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(3000) };

        public MainPageDetail()
        {
            InitializeComponent();
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
    }
}