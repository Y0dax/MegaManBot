using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static MegaManDiscordBot.Modules.Models.MediaModels;
using static MegaManDiscordBot.Modules.Models.MediaModels.GiphyModels;

namespace MegaManDiscordBot.Modules
{
    public class MediaModule : ModuleBase<SocketCommandContext>
    {
        static string giphyUrl = $"http://api.giphy.com/v1/gifs/";
        static string key = $"api_key={Globals.GiphyKey}";


        [Command("gif")]
        [Summary("Search for a giphy")]
        [Remarks("<search text>")]
        [MinPermissions(AccessLevel.User)]
        public async Task GiphySearch([Remainder]string searchString)
        {
            
            Uri uri = new Uri($"{giphyUrl}search?q={searchString.Replace(" ", "+")}&rating=pg-13&{key}");
            var response = await new ApiHandler<GiphySearchResult>().GetJSONAsync(uri);
            if (response != null)
            {
                await ReplyAsync(response.Data.Any() ? response.Data.RandomItemLowerBias().Url : "Sorry, I couldn't find a gif for that.");
            }
        }

        [Command("gifnsfw")]
        [Summary("Search for a nsfw giphy")]
        [Remarks("<search text>")]
        [MinPermissions(AccessLevel.User)]
        public async Task NSFWGiphySearch([Remainder]string searchString)
        {
            if (!Context.Channel.IsNsfw)
            {
                await ReplyAsync("Slow down there pal, try that out in a NSFW Channel.");
                return;
            }

            Uri uri = new Uri($"{giphyUrl}search?q={searchString.Replace(" ", "+")}&rating=r&limit=40&{key}");
            var response = await new ApiHandler<GiphySearchResult>().GetJSONAsync(uri);
            if (response?.Data != null)
            {
                var gifs = response.Data.Where(x => x.Rating.ToLower() == "r").ToList();
                await ReplyAsync(gifs.Any() ? gifs.RandomItemLowerBias().Url : "Sorry, I couldn't find a gif for that.");
            }
        }

        [Command("gif")]
        [Summary("Get a random giphy")]
        [Remarks("(optional) <nsfw>")]
        [MinPermissions(AccessLevel.User)]
        public async Task GiphyRandom()
        {
            Uri uri = new Uri($"{giphyUrl}random?rating=pg-13&{key}");

            var response = await new ApiHandler<GiphySingleResult>().GetJSONAsync(uri);
            if (response?.Data != null)
            {
                await ReplyAsync(response.Data.Url);
            }
        }

        static string xkcdUrl = $"http://xkcd.com/";

        [Command("xkcdtoday")]
        [Summary("Get current xkcd")]
        [MinPermissions(AccessLevel.User)]
        public async Task GetLatestXKCD()
        {
            Uri uri = new Uri($"{xkcdUrl}info.0.json");
            var response = await new ApiHandler<XKCD>().GetJSONAsync(uri);
            if (response?.Url != null)
            {
                await ReplyAsync(response.Title + ": " + response.Url);

                if (response.Num > Globals.xkcdNum) Globals.xkcdNum = response.Num; //update latest xkcd number
            }
        }

        [Command("xkcd")]
        [Summary("Get a random xkcd")]
        [MinPermissions(AccessLevel.User)]
        public async Task GetRandomXKCD()
        {
            Uri uri = new Uri($"{xkcdUrl}{Globals.Random.Next(1, Globals.xkcdNum)}/info.0.json");
            var response = await new ApiHandler<XKCD>().GetJSONAsync(uri);
            if (response?.Url != null)
            {
                await ReplyAsync(response.Title + ": " + response.Url);
            }
        }

        //[Command("tgif")]
        //[Summary("Get a giphy by translation")]
        //[Remarks("<search text>")]
        //[MinPermissions(AccessLevel.User)]
        //public async Task GiphyTranslate([Remainder]string searchString)
        //{
        //    Uri uri = new Uri($"{baseUrl}translate?s={searchString.Replace(" ", "+")}&{key}");
        //    ApiResponse<GiphySingleResult> response = await new ApiHandler<GiphySingleResult>().GetJSONAsync(uri);
        //    if (response.Success)
        //    {
        //        await ReplyAsync(response.responseObject.Data != null ? response.responseObject.Data.Url : "Sorry, I couldn't find a gif for that.");
        //    }
        //}


    }
}
