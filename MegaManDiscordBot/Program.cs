using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MegaManDiscordBot.Services.Configuration;
using MegaManDiscordBot.Services.Common;

namespace MegaManDiscordBot
{
    public class Program
    {

        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private Config _config;
        private CommandHandler _handler;

        public async Task Start()
        {
            PrettyConsole.NewLine("~~   Mega Man Bot Booting Up  ~~");
            PrettyConsole.NewLine();
         
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                LogLevel = LogSeverity.Debug,
            });
            _config = Config.Load();
            var serviceProvider = ConfigureServices();

            // Login and connect to Discord.
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            _client.Log += LogAsync;

            _handler = new CommandHandler(serviceProvider);
            await _handler.ConfigureAsync();

            // Block this program until it is closed.
            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_config)
                .AddSingleton(new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false, ThrowOnError = false }));
            var provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            // Autowire and create these dependencies now
            //provider.GetService<LogAdaptor>();
            return provider;
        }

        private Task LogAsync(LogMessage msg)
        {
            PrettyConsole.LogAsync(msg.Severity, msg.Source, msg.Exception?.ToString() ?? msg.Message);
            return Task.CompletedTask;
        }
    }
}
