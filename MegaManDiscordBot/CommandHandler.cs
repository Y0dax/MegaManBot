using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MegaManDiscordBot.Services.Configuration;
using MegaManDiscordBot.Services.Common;
using MegaManDiscordBot.Modules;
using Serilog;
using UtilityBot.Services.Logging;
using Serilog.Core;
using MongoDB.Driver;
using MegaManDiscordBot.Services.Moderator;

namespace MegaManDiscordBot
{
    public class CommandHandler
    {
        private readonly IServiceProvider _provider;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly Database _database;
        private readonly Config _config;
        private readonly ILogger _logger;
        private readonly GuildOptionsService _guildService;

        public CommandHandler(IServiceProvider provider)
        {
            _provider = provider;
            _client = _provider.GetService<DiscordSocketClient>();
            _client.MessageReceived += ProcessCommandAsync;
            _database = _provider.GetService<Database>();
            _commands = _provider.GetService<CommandService>();
            var log = _provider.GetService<LogAdaptor>();
            _commands.Log += log.LogCommand;
            _config = _provider.GetService<Config>();
            _logger = _provider.GetService<Logger>().ForContext<CommandService>();
            _guildService = provider.GetService<GuildOptionsService>();
        }

        public async Task ConfigureAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task ProcessCommandAsync(SocketMessage pMsg)
        {
            var msg = pMsg as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(_client, msg);
            var commandString = _guildService.GetCommandString(context.Guild.Id).Result;
            if(commandString == null) commandString = _config.CommandString;

            int argPos = 0;
            if (msg.HasStringPrefix(commandString, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);

                //if (!result.IsSuccess) // If execution failed, reply with the error message.
                //    await context.Channel.SendMessageAsync(result.ToString());
                _logger.Debug($"Invoked {msg} in {context.Channel} with {result}");
            }
        }
    }
}