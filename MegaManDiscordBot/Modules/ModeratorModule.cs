using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Modules.Models.DomainModels;
using MegaManDiscordBot.Services.Common;
using MegaManDiscordBot.Services.Moderator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules
{
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        private static GuildOptionsService _guildService;

        public ModeratorModule(GuildOptionsService service)
        {
            _guildService = service;
        }

        [Command("echo")]
        [Summary("")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task Uptime([Remainder] string text)
        {
            await ReplyAsync(text);
        }

        [Command("uptime")]
        [Summary("Get the bots uptime")]
        //[CheckEnabled("Reddit", _guildService)]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task Uptime()
        {
            TimeSpan t = DateTime.Now - Globals.bootTime;
            await ReplyAsync($"{t.Days} Days, {t.Hours} Hours, {t.Minutes} Minutes");
        }

        //[Command("info")]
        //[Summary("Get user info")]
        //[Remarks("<user>")]
        //[MinPermissions(AccessLevel.ServerMod)]
        //public async Task UserInfo([Remainder]SocketGuildUser user)
        //{
        //    if (user == null)
        //        return;

        //    StringBuilder returnMesage = new StringBuilder();
        //    returnMesage.Append($" {user.Mention} joined the {user.Guild.Name} on {user.JoinedAt.Value.ToLocalTime()}.");
        //    returnMesage.Append($" {user.Nickname ?? user.Username} is currently {user.Status}");
        //    returnMesage.Append(user.Game.Value.Name != null ? $" and is playing {user.Game.Value.Name}." : "");
        //    //returnMesage.Append(user.Status?.Value == "online" && user.JoinedAt != null ? $" They were last active at {user.LastActivityAt.Value.ToLocalTime()}." : "");

        //    await ReplyAsync(returnMesage.ToString());
        //}

        [Command("guildOptions")]
        [Summary("Show the guild options and customizations.")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task ShowGuildOptions()
        {
            var guildOptions = await _guildService.GetGuildOptions(Context.Guild.Id);
            if (guildOptions == null) return;
            StringBuilder reply = new StringBuilder();
            reply.Append(Format.Bold($"Guild Options For {Context.Guild.Name}:\n\n"));
            reply.Append($"Command Prefix: \"{guildOptions.CommandString}\"\n\n");
            reply.Append(Format.Bold("Modules:\n"));
            foreach (var prop in guildOptions.GetType().GetProperties())
            {
                if(prop.PropertyType == typeof(bool))
                {
                    reply.Append($"{prop.Name} : {((bool)prop.GetValue(guildOptions) ? "Enabled" : "False")}\n");
                }
            }

            await ReplyAsync(reply.ToString());
        }

        [Command("prefix")]
        [Summary("Set a custom command prefix (Default is \".\")")]
        [Remarks("<prefix_char>")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task ChangePrefix(char newPrefix)
        {
            var response = await _guildService.UpdatePrefix(Context.Guild.Id, newPrefix);

            if (response.IsAcknowledged)
                await ReplyAsync($"Prefix changed to \"{newPrefix}\"");
        }

        //[Command("disable")]
        //[Summary("Disable a module. Choose from: Brewery, Giphy, Reddit, Weather, XKCD, Google, IMDB, and Polls.")]
        //[Remarks("<module_name>")]
        //[MinPermissions(AccessLevel.ServerOwner)]
        //public async Task DisableModule(string moduleName)
        //{

        //    await ReplyAsync($"");
        //}

        //[Command("enable")]
        //[Summary("Enable a module. Choose from: Brewery, Giphy, Reddit, Weather, XKCD, Google, IMDB, and Polls.")]
        //[Remarks("<module_name>")]
        //[MinPermissions(AccessLevel.ServerOwner)]
        //public async Task EnableModule(string moduleName)
        //{

        //    await ReplyAsync($"");
        //}
    }
}
