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
using MegaManDiscordBot.Modules.Public;

namespace MegaManDiscordBot
{
    public class CommandHandler
    {
        private readonly IServiceProvider _provider;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public CommandHandler(IServiceProvider provider)
        {
            _provider = provider;
            _client = _provider.GetService<DiscordSocketClient>();
            _client.MessageReceived += ProcessCommandAsync;
            _commands = _provider.GetService<CommandService>();
            //_commands.Log += PrettyConsole.Log();
            _config = _provider.GetService<Config>();
        }

        public async Task ConfigureAsync()
        {
            //await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            await _commands.AddModuleAsync<PublicModule>();
            await _commands.AddModuleAsync<GiphyModule>();
        }

        private async Task ProcessCommandAsync(SocketMessage pMsg)
        {
            var msg = pMsg as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(_config.CommandString, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess) // If execution failed, reply with the error message.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}