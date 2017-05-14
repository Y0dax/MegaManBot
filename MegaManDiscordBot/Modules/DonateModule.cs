using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules
{
    public class DonateModule : ModuleBase<SocketCommandContext>
    {
        [Command("paypal")]
        [Summary("Donations always appreciated")]
        public async Task Paypal()
        {
            await ReplyAsync("Visit https://www.paypal.me/yodax to support the developer.");
        }
    }
}
