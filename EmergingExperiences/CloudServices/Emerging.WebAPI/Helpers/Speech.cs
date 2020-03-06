using Emerging.WebAPI.Models;
using SharedServices.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Emerging.WebAPI.Helpers
{
    public class Speech
    {

        SpeechConfig Config = new SpeechConfig();

        public Speech(SpeechConfig config)
        {
            Config = config;
        }

        public string ConvertToBase64([NotNull] byte[] audioBytes)
        {
            try
            {
                var speechEncodedBytes = Convert.ToBase64String(audioBytes);
                return speechEncodedBytes;
            }
            catch (Exception e)
            {
                var message = $"An error occurred converting the {nameof(audioBytes)}. {e.Message}";
                throw;
            }
        }

        public string ConvertTextToSpeechBase64([NotNull] string text)
        {
            var data = ConvertTextToSpeechAsync(text).Result;
            return ConvertToBase64(data);
        }

        public byte[] ConvertTextToSpeech([NotNull] string text)
        {
            return ConvertTextToSpeechAsync(text).Result;
        }

        public async Task<byte[]> ConvertTextToSpeechAsync([NotNull] string text)
        {
            var host = Config.AzureEndPointUri;
            var accessToken = await CognitiveServicesHelper.FetchTokenAsync(Config.AzureTokenUri, Config.APIKey);

            var body = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
                @"<voice name='Microsoft Server Speech Text to Speech Voice (en-US, Jessa24kRUS)'" +
                $@">{text}</voice></speak>";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, host))
            {
                request.Content = new StringContent(body, Encoding.UTF8, "application/ssml+xml");
                request.Headers.Add(@"Authorization", $@"Bearer {accessToken}");
                request.Headers.Add(@"Connection", @"Keep-Alive");
                request.Headers.Add(@"User-Agent", Config.AppName);
                request.Headers.Add(@"X-Microsoft-OutputFormat", Config.AudioFormat);

                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    using (var dataStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var memStream = new MemoryStream())
                    {
                        await dataStream.CopyToAsync(memStream).ConfigureAwait(false);

                        memStream.Close();

                        return memStream.ToArray();
                    }
                }

            }

        }

    }
}
