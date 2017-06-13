using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Services.Common;
using MegaManDiscordBot.Services.Moderator;

namespace MegaManDiscordBot.Modules
{
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        private static GuildOptionsService _guildService;
        private static IServiceProvider _provider;

        public UtilityModule(GuildOptionsService service, IServiceProvider provider)
        {
            _provider = provider;
            _guildService = service;
        }

        [Command("help"), Alias("commands")]
        [Summary("Display bot commands")]
        [MinPermissions(AccessLevel.User)]
        public async Task Help()
        {
            string Extract(string s) { return (s.Remove(s.Length - "Module".Length)); };
            StringBuilder response = new StringBuilder();
            SortedDictionary<string, List<CommandInfo>> ModuleDict = new SortedDictionary<string, List<CommandInfo>>();

            var commandString = await _guildService.GetCommandString(Context.Guild.Id);
            if (commandString == null) commandString = Globals.CommandKey;

            Program._commandService.Modules.ToList().ForEach(m => { ModuleDict.Add(m.Name, new List<CommandInfo>()); });

            var tasks = Program._commandService.Commands.Select(async c =>
            {
                var result = await c.CheckPreconditionsAsync(Context, _provider);
                if (result.IsSuccess)
                    ModuleDict[c.Module.Name].Add(c);
            });

            await Task.WhenAll(tasks);

            foreach (KeyValuePair<string, List<CommandInfo>> entry in ModuleDict)
            {
                if (entry.Value.Any())
                {
                    response.Append(Format.Bold($"\n{Extract(entry.Key)}\n"));
                    entry.Value.ForEach(c => { response.Append($"{commandString}{Format.Bold($"{c.Name} {c.Remarks ?? ""}")} - {c.Summary}\n"); });
                }
            }

            var embed = new EmbedBuilder().WithColor(new Color(Convert.ToUInt32("71cd40", 16)))
           .WithTitle("Mega Man Commands")
           .WithDescription(response.ToString());
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
