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
using System.Diagnostics;

namespace MegaManDiscordBot.Modules
{
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task Test()
        {
            await ReplyAsync("Test");
        }

        [Command("echo")]
        [Remarks("Make the bot repeat a phrase")]
        //[MinPermissions(AccessLevel.ServerAdmin)]
        public async Task ListInput([Remainder]string text)
        {
            await ReplyAsync(text);
        }

        [Command("info")]
        [Remarks("Get info on a user")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task UserInfo([Remainder]SocketGuildUser user)
        {
            StringBuilder returnMesage = new StringBuilder();
            returnMesage.Append($" {user.Mention} joined the {user.Guild.Name} on {user.JoinedAt.Value.ToLocalTime()}.");
            returnMesage.Append($" {user.Nickname ?? user.Username} is currently {user.Status}");
            returnMesage.Append(user.Game.Value.Name != null ? $" and is playing {user.Game.Value.Name}." : "");
            //returnMesage.Append(user.Status?.Value == "online" && user.JoinedAt != null ? $" They were last active at {user.LastActivityAt.Value.ToLocalTime()}." : "");

            await ReplyAsync(returnMesage.ToString());
        }

        [Command("pick")]
        [Remarks("Picks a random item from a list")]
        public async Task PickFromList(params string[] items)
        {
            await ReplyAsync($"I choose {items.ToList().RandomItem()}!");
        }

        [Command("teams")]
        [Remarks("Splits all users in current voice channel into two teams")]
        public async Task MakeTeams(int numTeams)
        {
            List<SocketGuildUser> Users = Context.Guild.GetUser(Context.User.Id).VoiceChannel.Users.ToList().Shuffle();

            List<List<SocketGuildUser>> Teams = Users.Select((s, index) => new { String = s, Index = index })
                                                        .GroupBy(x => x.Index % numTeams)
                                                        .Select(g => g.Select(x => x.String).ToList())
                                                        .ToList();

            StringBuilder reply = new StringBuilder();
            int teamIndex = 1;

            Teams.ForEach(t => {
                reply.Append(Format.Bold($"Team {teamIndex++}:\n"));
                t.ForEach(u =>
                {
                    reply.Append($"{u.Nickname ?? u.Username}\n");
                });
                reply.Append("\n");
            });

            await ReplyAsync(reply.ToString());
        }
    }
}
