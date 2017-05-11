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
using MegaManDiscordBot;
using System.IO;

namespace MegaManDiscordBot.Modules
{
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("help"), Alias("commands")]
        [Summary("Display bot commands")]
        //[MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Help()
        {
            StringBuilder commands = new StringBuilder();
            Program._commandService.Modules.ToList().ForEach(m =>
            {
                commands.Append(Format.Bold($"\n{m.Name.Remove(m.Name.Length - 6)}\n"));
                Program._commandService.Commands.Where(c => c.Module == m).ToList().ForEach(async c =>
                {
                    var result = await c.CheckPreconditionsAsync(Context);
                    if(result.IsSuccess)
                        commands.Append($"!{Format.Bold($"{c.Name} {c.Remarks ?? ""}")} - {c.Summary}\n");
                });
            });

            var embed = new EmbedBuilder().WithColor(new Color(Convert.ToUInt32("71cd40", 16)))
           .WithTitle("Mega Man Commands")
           .WithDescription(commands.ToString());

            await ReplyAsync("", false, embed);
        }

        //[Command("avatar")]
        //[Summary("")]
        //[MinPermissions(AccessLevel.ServerOwner)]
        //public async Task AvatarUpdate()
        //{
        //    var avatar = new FileStream(@"", FileMode.Open);
        //    await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Discord.Image(avatar));
        //}

        [Command("uptime")]
        [Summary("Get the bots uptime")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Uptime()
        {
            TimeSpan t = DateTime.Now - Globals.bootTime;
            await ReplyAsync($"{t.Days} Days, {t.Hours} Hours, {t.Minutes} Minutes");
        }

        [Command("info")]
        [Summary("Get user info")]
        [Remarks("<user>")]
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
        [Summary("Picks a random item from a list")]
        [Remarks("<items>")]
        public async Task PickFromList(params string[] items)
        {
            await ReplyAsync($"I choose {items.ToList().RandomItem()}!");
        }

        [Command("teams")]
        [Summary("Creates teams from users in current channel")]
        [Remarks("<number_of_teams>")]
        public async Task MakeTeams(int numTeams)
        {
            List<SocketGuildUser> Users = Context.Guild.GetUser(Context.User.Id).VoiceChannel.Users.ToList().Shuffle();

            List<List<SocketGuildUser>> Teams = Users.Select((s, index) => new { String = s, Index = index })
                                                        .GroupBy(x => x.Index % numTeams)
                                                        .Select(g => g.Select(x => x.String).ToList())
                                                        .ToList();

            StringBuilder reply = new StringBuilder();
            int teamIndex = 1;

            Teams.ForEach(t =>
            {
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
