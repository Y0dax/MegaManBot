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
    public static class Globals
    {
        public static void Initiate(Config config)
        {
            WeatherKey = config.WeatherKey;
            BreweryKey = config.BreweryKey;
            GiphyKey = config.GiphyKey;
            Random = new Random();
        }
        public static Random Random { get; set; }
        public static DateTime bootTime { get; set; } = DateTime.Now;
        public static int xkcdNum { get; set; } = 1830;
        public static string WeatherKey { get; set; }
        public static string BreweryKey { get; set; }
        public static string GiphyKey { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private Config _config;
        private CommandHandler _handler;
        public static CommandService _commandService = new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false, ThrowOnError = false });


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
            Globals.Initiate(_config);

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
                .AddSingleton(_commandService);
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
