using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules
{
    public class GoogleModule : ModuleBase<SocketCommandContext>
    {
        string baseUrl = "http://www.google.com/";

        [Command("google")]
        [Summary("Search goolge")]
        [Remarks("<seach text>")]
        public async Task GiphySearch([Remainder]string searchString)
        {
            await ReplyAsync($"{baseUrl}search?q={searchString.Replace(" ", "+")}");
        }
    }
}
