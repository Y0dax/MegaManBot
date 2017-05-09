﻿using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using MegaManDiscordBot.Services.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Public
{
    class XKCDModule : ModuleBase<SocketCommandContext>
    {
        static string baseUrl = $"http://xkcd.com/";
        static Random rand = new Random();

        [Command("xkcd")]
        [Remarks("Get the most recent xkcd")]
        //[MinPermissions(AccessLevel.User)]
        public async Task GetLatestXKCD()
        {
            Uri giphyUri = new Uri(baseUrl + "info.0.json");
            ApiResponse<XKCD> response = await new ApiHandler<XKCD>().GetJSONAsync(giphyUri);
            if (response.Success && response.responseObject.Url != null)
            {
                await ReplyAsync(response.responseObject.Title + ": " + response.responseObject.Url);

                if (response.responseObject.Num > Globals.xkcdNum) Globals.xkcdNum = response.responseObject.Num; //update latest xkcd number
            }
        }

        [Command("randXKCD")]
        [Remarks("Get a random xkcd")]
        //[MinPermissions(AccessLevel.User)]
        public async Task GetRandomXKCD()
        {
            Uri giphyUri = new Uri(String.Format("{0}{1}/info.0.json", baseUrl, rand.Next(1, Globals.xkcdNum)));
            ApiResponse<XKCD> response = await new ApiHandler<XKCD>().GetJSONAsync(giphyUri);
            if (response.Success && response.responseObject.Url != null)
            {
                await ReplyAsync(response.responseObject.Title + ": " + response.responseObject.Url);
                await ReplyAsync(Globals.xkcdNum.ToString());
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