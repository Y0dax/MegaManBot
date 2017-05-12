using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules
{
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {

        [Command("echo")]
        [Summary("")]
        [MinPermissions(AccessLevel.ServerOwner)]
        public async Task Uptime([Remainder] string text)
        {
            await ReplyAsync(text);
        }

        [Command("uptime")]
        [Summary("Get the bots uptime")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task Uptime()
        {
            TimeSpan t = DateTime.Now - Globals.bootTime;
            await ReplyAsync($"{t.Days} Days, {t.Hours} Hours, {t.Minutes} Minutes");
        }

        [Command("info")]
        [Summary("Get user info")]
        [Remarks("<user>")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task UserInfo([Remainder]SocketGuildUser user)
        {
            StringBuilder returnMesage = new StringBuilder();
            returnMesage.Append($" {user.Mention} joined the {user.Guild.Name} on {user.JoinedAt.Value.ToLocalTime()}.");
            returnMesage.Append($" {user.Nickname ?? user.Username} is currently {user.Status}");
            returnMesage.Append(user.Game.Value.Name != null ? $" and is playing {user.Game.Value.Name}." : "");
            //returnMesage.Append(user.Status?.Value == "online" && user.JoinedAt != null ? $" They were last active at {user.LastActivityAt.Value.ToLocalTime()}." : "");

            await ReplyAsync(returnMesage.ToString());
        }

    }
}
