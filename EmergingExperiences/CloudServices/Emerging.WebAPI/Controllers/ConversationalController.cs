using Emerging.WebAPI.Bots;
using Emerging.WebAPI.Helpers;
using Emerging.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Emerging.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationalController : ControllerBase
    {

        private readonly ILogger<ConversationalController> _logger;

        public ConversationalController(ILogger<ConversationalController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public string Get ()
        {
            return "hello";
        }
        [HttpPost]
        public async Task<ActionResult<BotResponse>> Post(string message)
        {
            var adapter = new ConversationBotAdapter();
            ConversationDispatcher dispatcher = new ConversationDispatcher();
            BotResponse botResponse = new BotResponse();
            adapter.ProcessActivityAsync(
                async (turnContext, cancellationToken)=>
                {
                    var  intentResult = await dispatcher.OnTurnAsync(turnContext);
                    botResponse = ProcessIntentToResponse(intentResult);
                }
                , message
                ).Wait();
            return botResponse;
        }
        private BotResponse ProcessIntentToResponse(string botResponse)
        {
            var result = new BotResponse { TextResponse = "Can you repeat your request?", Confidence = "0.00" };
            try
            {
                var intentwithScore = JsonConvert.DeserializeObject<IntentWithScore>(botResponse);
                result.Confidence = intentwithScore.Score;
                switch(intentwithScore.Result.ToLower().Replace("_"," "))
                {
                    case "ryerson - order":
                        result.TextResponse = "What product are you looking for?";
                        break;
                    case "ryerson - product":
                        result.TextResponse = "What type of form is this product?";
                        break;
                    case "ryerson - form":
                        result.TextResponse = "Let's get the shape that you want next.";
                        break;
                    case "ryerson - shape":
                        result.TextResponse = "Tell me the grade that you want for this order.";
                        break;
                    case "ryerson - grade":
                        result.TextResponse = "Okay. I got it!";
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
            }
            var APITokenEndPoint = "https://northcentralus.api.cognitive.microsoft.com";
            Speech speech = new Speech(new SpeechConfig
            {
                APIKey = "e1d70faa757a4424bae02fc3284b0cbc",
                AppName= "EmergingSpeechRecognitionDemo",
                AudioFormat= "riff-24khz-16bit-mono-pcm",
                AzureEndPointUri= "https://northcentralus.tts.speech.microsoft.com/cognitiveservices/v1", //APITokenEndPoint + "/speech/recognition/conversation/cognitiveservices/v1",
                AzureTokenUri = "https://northcentralus.api.cognitive.microsoft.com/sts/v1.0/issuetoken",// APITokenEndPoint +"/sts/v1.0/issuetoken",
                AzureRegion = "northcentralus",
            });
            result.Base64Audio = speech.ConvertTextToSpeechBase64(result.TextResponse);
            return result;
        }
    }
}