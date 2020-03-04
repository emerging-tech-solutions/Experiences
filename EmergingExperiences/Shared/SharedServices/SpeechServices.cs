using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedServices
{
    public class SpeechServices
    {
        private string APIKey = "e1d70faa757a4424bae02fc3284b0cbc";
        private string APIEndPoint = "https://northcentralus.api.cognitive.microsoft.com/sts/v1.0/";
        private string token;

        public async void Authenticate()
        {
            this.token = await FetchTokenAsync("issuetoken");
        }

        private async Task<string> FetchTokenAsync(string fetchUri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKey);
                UriBuilder uriBuilder = new UriBuilder(APIEndPoint + fetchUri);

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                Console.WriteLine("Token Uri: {0}", uriBuilder.Uri.AbsoluteUri);
                return await result.Content.ReadAsStringAsync();
            }
        }


    }


}
