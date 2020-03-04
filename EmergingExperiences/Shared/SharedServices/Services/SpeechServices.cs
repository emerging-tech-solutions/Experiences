using Newtonsoft.Json;
using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SharedServices.Services
{
    public class SpeechServices
    {
        private string APIKey = "e1d70faa757a4424bae02fc3284b0cbc";
        private string Region = "northcentralus";
        //private string APIEndPoint = $"https://{Region}.api.cognitive.microsoft.com";
        private string APITokenEndPoint = "";
        private string APIEndPoint = "";
        private string token;

        public async void Authenticate()
        {
            APIEndPoint = $"https://{Region}.stt.speech.microsoft.com";
            APITokenEndPoint = $"https://{Region}.api.cognitive.microsoft.com";
            this.token = await FetchTokenAsync("/sts/v1.0/issuetoken");
        }

        private async Task<string> FetchTokenAsync(string fetchUri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKey);
                UriBuilder uriBuilder = new UriBuilder(APITokenEndPoint + fetchUri);

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                Console.WriteLine("Token Uri: {0}", uriBuilder.Uri.AbsoluteUri);
                return await result.Content.ReadAsStringAsync();
            }
        }
        public async Task<SpeechSynthesisResult> SynthesiseSpeech(byte[] array, string lang ="en-US")
        {
            var baseURI = APIEndPoint + $"/speech/recognition/conversation/cognitiveservices/v1?language={lang}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKey);
            byte[] byteData = array;
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(baseURI, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<SpeechSynthesisResult>(responseString);
                    return result;
                }
                else
                    return null;
            }
        }

    }


}
