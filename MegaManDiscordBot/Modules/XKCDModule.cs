using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using MegaManDiscordBot.Services.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules
{
    public class XKCDModule : ModuleBase<SocketCommandContext>
    {
        static string baseUrl = $"http://xkcd.com/";

        [Command("xkcdtoday")]
        [Summary("Get current xkcd")]
        [MinPermissions(AccessLevel.User)]
        public async Task GetLatestXKCD()
        {
            Uri uri = new Uri($"{baseUrl}info.0.json");
            ApiResponse<XKCD> response = await new ApiHandler<XKCD>().GetJSONAsync(uri);
            if (response.Success && response.responseObject.Url != null)
            {
                await ReplyAsync(response.responseObject.Title + ": " + response.responseObject.Url);

                if (response.responseObject.Num > Globals.xkcdNum) Globals.xkcdNum = response.responseObject.Num; //update latest xkcd number
            }
        }

        [Command("xkcd")]
        [Summary("Get a random xkcd")]
        [MinPermissions(AccessLevel.User)]
        public async Task GetRandomXKCD()
        {
            Uri uri = new Uri($"{baseUrl}{Globals.Random.Next(1, Globals.xkcdNum)}/info.0.json");
            ApiResponse<XKCD> response = await new ApiHandler<XKCD>().GetJSONAsync(uri);
            if (response.Success && response.responseObject.Url != null)
            {
                await ReplyAsync(response.responseObject.Title + ": " + response.responseObject.Url);
            }
        }

        public class XKCD
        {
            [JsonProperty("num")]
            public int Num { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("img")]
            public string Url { get; set; }
        }

    }
}
