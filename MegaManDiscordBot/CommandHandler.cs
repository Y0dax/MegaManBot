using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MegaManDiscordBot.Services.Configuration;

namespace MegaManDiscordBot
{
    public class CommandHandler
    {
        private readonly IServiceProvider _provider;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        //private IEnumerable<ulong> Whitelist => _config.ChannelWhitelist;

        public CommandHandler(IServiceProvider provider)
        {
            _provider = provider;
            _client = _provider.GetService<DiscordSocketClient>();
            _client.MessageReceived += ProcessCommandAsync;
            _commands = _provider.GetService<CommandService>();
            //_commands.Log += LogAsync;
            _config = _provider.GetService<Config>();
        }

        public async Task ConfigureAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task ProcessCommandAsync(SocketMessage pMsg)
        {
            var msg = pMsg as SocketUserMessage;
            if (msg == null)                                          // Check if the received message is from a user.
                return;

            var context = new SocketCommandContext(_client, msg);     // Create a new command context.

            int argPos = 0;                                           // Check if the message has either a string or mention prefix.
            if (msg.HasStringPrefix(_config.CommandString, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {                                                         // Try and execute a command with the given context.
                var result = await _commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)                                // If execution failed, reply with the error message.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }

        //private bool ParseTriggers(SocketUserMessage message, ref int argPos)
        //{
        //    bool flag = false;
        //    if (message.HasMentionPrefix(_client.CurrentUser, ref argPos)) flag = true;
        //    else
        //    {
        //        foreach (var prefix in _config.CommandString)
        //        {
        //            if (message.HasStringPrefix(prefix, ref argPos))
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }
        //    return flag ? Whitelist.Any(id => id == message.Channel.Id) : false;
        //}
    }
}