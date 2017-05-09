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

namespace MegaManDiscordBot.Modules.Public
{
    class GiphyModule : ModuleBase<SocketCommandContext>
    {
        static string baseUrl = $"http://api.giphy.com/v1/gifs/";
        static string key = $"api_key={Globals.GiphyKey}";

        [Command("gif")]
        [Remarks("Search for a giphy")]
        [MinPermissions(AccessLevel.User)]
        public async Task GiphySearch([Remainder]string searchString)
        {
            
            Uri uri = new Uri($"{baseUrl}search?q={searchString.Replace(" ", "+")}&{key}");
            ApiResponse<GiphySearchResult> response = await new ApiHandler<GiphySearchResult>().GetJSONAsync(uri);
            if (response.Success && response.responseObject.Data.Any())
            {
                await ReplyAsync(response.responseObject.Data.RandomItem().Url);
            }
        }

        [Command("gif")]
        [Remarks("Get a random giphy")]
        [MinPermissions(AccessLevel.User)]
        public async Task GiphyRandom()
        {
            Uri uri = new Uri($"{baseUrl}random?{key}");
            ApiResponse<GiphySingleResult> response = await new ApiHandler<GiphySingleResult>().GetJSONAsync(uri);
            if (response.Success && response.responseObject.Data != null)
            {
                await ReplyAsync(response.responseObject.Data.Url);
            }
        }

        [Command("tgif")]
        [Remarks("Get a giphy by translation")]
        [MinPermissions(AccessLevel.User)]
        public async Task GiphyTranslate([Remainder]string searchString)
        {
            Uri uri = new Uri($"{baseUrl}translate?s={searchString.Replace(" ", "+")}&{key}");
            ApiResponse<GiphySingleResult> response = await new ApiHandler<GiphySingleResult>().GetJSONAsync(uri);
            if (response.Success && response.responseObject.Data != null)
            {
                await ReplyAsync(response.responseObject.Data.Url);
            }
        }

        public class GiphySearchResult
        {
            [JsonProperty("data")]
            public List<Data> Data { get; set; }
        }

        public class GiphySingleResult
        {
            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public class Data
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

    }
}
