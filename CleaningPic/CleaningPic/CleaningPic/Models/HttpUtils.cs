using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleaningPic.Models
{
    // タイムアウトを含む通信失敗の場合にはnullを返す
    public class HttpUtils
    {
        private const string host = "http://35.189.147.104/";
        private static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };

        public static async Task<string> PostDirtImageAsync(byte[] imageData, Place place)
        {
            HttpResponseMessage result = null;
            var content = new MultipartFormDataContent();
            var fileName = DateTime.Now.ToString("yyyy_MM_dd-HH:mm:ss") + ".jpg";
            content.Add(new ByteArrayContent(imageData), "upload_file", fileName);
            content.Add(new StringContent(((int)place).ToString()), "location_id");

            try { result = await client.PostAsync(host + "upload/", content); }
            catch (Exception) { return null; }

            if (result?.StatusCode == System.Net.HttpStatusCode.OK)
                return await result.Content.ReadAsStringAsync();
            else
                return null;
        }

        public static async Task<string> GetResult()
        {
            HttpResponseMessage result = null;

            try { result = await client.GetAsync(host + "api/results"); }
            catch (Exception) { return null; }

            if (result?.StatusCode == System.Net.HttpStatusCode.OK)
                return await result.Content.ReadAsStringAsync();
            else
                return null;
        }
    }
}
