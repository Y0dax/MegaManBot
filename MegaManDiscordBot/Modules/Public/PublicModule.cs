using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Services.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MegaManDiscordBot.Modules.Public
{
    class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task Test()
        {
            await ReplyAsync("Test");
        }

        [Command("echo")]
        [Remarks("Make the bot repeat a phrase")]
        //[MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Echo([Remainder]string text)
        {
            await ReplyAsync(text);
        }

        [Command("info")]
        [Remarks("Get info on a user")]
        //[MinPermissions(AccessLevel.User)]
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
