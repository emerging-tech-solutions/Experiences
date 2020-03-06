using Newtonsoft.Json;
using SharedServices.Helpers;
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
            this.token = await CognitiveServicesHelper.FetchTokenAsync(APITokenEndPoint + "/sts/v1.0/issuetoken", APIKey);
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
