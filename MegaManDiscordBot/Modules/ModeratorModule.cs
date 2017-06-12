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
        [MinPermissions(AccessLevel.User)]
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

        [Command("options")]
        [Summary("Show the bot options")]
        [MinPermissions(AccessLevel.User)]
        public async Task ShowGuildOptions()
        {
            var guildOptions = await _guildService.GetGuildOptions(Context.Guild.Id);
            StringBuilder reply = new StringBuilder();
            reply.Append(Format.Bold($"Guild Options For {Context.Guild.Name}:\n\n"));
            reply.Append($"Command Prefix: \"{guildOptions.CommandString}\"\n\n");
            reply.Append(Format.Bold("Modules:\n"));
            foreach (var prop in guildOptions.Modules.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(bool))
                {
                    reply.Append($"{prop.Name} : {((bool)prop.GetValue(guildOptions.Modules) ? "Enabled" : "Disabled")}\n");
                }
            }

            await ReplyAsync(reply.ToString());
        }

        [Command("prefix")]
        [Summary("Set a custom command prefix. Default is \"" + Globals.CommandKey + "\"")]
        [Remarks("<prefix_char>")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task ChangePrefix(char newPrefix)
        {
            var response = await _guildService.UpdatePrefix(Context.Guild.Id, newPrefix);

            if (response.IsAcknowledged)
                await ReplyAsync($"Prefix changed to \"{newPrefix}\"");
        }

        [Command("disable")]
        [Summary("Disable a module")]
        [Remarks("<module_name>")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task DisableModule(string moduleName)
        {
            var propertyExists = Globals.ModuleNames.GetType().GetProperties().Any(x => x.Name == moduleName);
            if (!propertyExists)
            {
                await ModuleNotFound();
                return;
            }

            await _guildService.UpdateModule(Context.Guild.Id, moduleName, false);
            await ReplyAsync($"The {moduleName.ToLower()} module has been disabled.");
        }

        [Command("enable")]
        [Summary("Enable a module")]
        [Remarks("<module_name>")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task EnableModule(string moduleName)
        {
            var propertyExists = Globals.ModuleNames.GetType().GetProperties().Any(x => x.Name == moduleName);
            if (!propertyExists)
            {
                await ModuleNotFound();
                return;
            }

            await _guildService.UpdateModule(Context.Guild.Id, moduleName, true);
            await ReplyAsync($"The {moduleName.ToLower()} module has been enabled.");
        }

        public async Task ModuleNotFound()
        {
            StringBuilder reply = new StringBuilder();
            reply.Append($"That module does not exist. This command is case-sensitive. Try one of these:\n");
            foreach (var prop in Globals.ModuleNames.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(bool))
                {
                    reply.Append($"{prop.Name}, ");//TODO: this will output an extra comma
                }
            }

            await ReplyAsync(reply.ToString());
        }
    }
}
