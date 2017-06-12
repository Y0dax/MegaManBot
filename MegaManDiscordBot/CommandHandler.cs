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
using MegaManDiscordBot.Modules.Models.DomainModels;

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
            int argPos = 0;
            IResult result = null;
            var context = new SocketCommandContext(_client, msg);

            var commandString = await _guildService.GetCommandString(context.Guild.Id);

            if (commandString != null)
            {
                if (msg.HasStringPrefix(commandString, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    result = await _commands.ExecuteAsync(context, argPos, _provider);
                    _logger.Debug($"Invoked {msg} in {context.Channel} with {result}");
                    //if (!result.IsSuccess) // If execution failed, reply with the error message.
                    //    await context.Channel.SendMessageAsync(result.ToString());
                }

            }
            else //Use default command string and create a guildOptions
            {
                commandString = _config.CommandString;

                var updateResponse = await _guildService.UpdateGuildOptions(new GuildOptions { GuildId = context.Guild.Id });
                if (updateResponse.IsAcknowledged)
                {
                    if (msg.HasStringPrefix(commandString, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                    {
                        result = await _commands.ExecuteAsync(context, argPos, _provider);
                        _logger.Debug($"Invoked {msg} in {context.Channel} with {result}");
                    }
                }
            }

            

        }
    }
}