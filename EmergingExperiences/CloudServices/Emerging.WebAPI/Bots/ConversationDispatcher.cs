using Emerging.WebAPI.Helpers;
using Emerging.WebAPI.Models;
using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Emerging.WebAPI.Bots
{
    public class ConversationDispatcher: IBot
    {
        //LuisRecognizer _dispatch { get; set; }
        List<BotIntent> _botIntents { get; set; }
        public async Task<string> OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            //await turnContext.SendActivityAsync($"You sent '{turnContext.Activity.Text}'", cancellationToken: cancellationToken);

            //var dispatchResult = await _dispatch.RecognizeAsync(turnContext, cancellationToken);
            //var topIntent = dispatchResult.GetTopScoringIntent();
            //var botIntent = _botIntents.Where(b => string.Equals(b.Intent, topIntent.intent)).FirstOrDefault();
            var botIntent = new BotIntent
            {
                HostName = "westus",
                Id = "871a0e23-83dc-43da-b1f2-a460bd6b1448",
                Intent = "EmergingTech_Ryerson",
                IsLuis = true,
                Key = "c37efd10cf26495e90d584da0fa2c9d3"
            };
            //if (botIntent.IsLuis)
            //{
            return await AccessLUIS(turnContext, cancellationToken, botIntent);
            //}
            //else
            //{
            //    return await AccessQnAMaker(turnContext, cancellationToken, botIntent);
            //};
        }
        private async Task<string> AccessLUIS(ITurnContext turnContext, CancellationToken cancellationToken, BotIntent intent)
        {
            var luisIntent = await LuisHelper.ExecuteLuisQuery(intent, null, turnContext, cancellationToken);
            return JsonConvert.SerializeObject(luisIntent);
            //await turnContext.SendActivityAsync(MessageFactory.Text($"Luis Intent: {luisIntent.Result} || Score: {luisIntent.Score}"), cancellationToken);
            //LogIntent(luisIntent);
        }

        Task IBot.OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
