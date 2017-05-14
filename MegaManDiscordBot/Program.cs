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
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using UtilityBot.Services.Logging;
using MegaManDiscordBot.Services.Music;

namespace MegaManDiscordBot
{ 
    public static class Globals
    {
        public static void Initiate(Config config)
        {
            WeatherKey = config.WeatherKey;
            BreweryKey = config.BreweryKey;
            GiphyKey = config.GiphyKey;
            SpotifyClientId = config.SpotifyClientId;
            SpotifyClientSecret = config.SpotifyClientSecret;
            CommandKey = config.CommandString;
            Random = new Random();

           SpotifyToken = new ClientCredentialsAuth()
            {
                ClientId = SpotifyClientId,
                ClientSecret = SpotifyClientSecret,
                Scope = Scope.PlaylistReadPrivate,
            }.DoAuth();

        }
        public static Token SpotifyToken { get; set; }
        public static string CommandKey { get; set; }
        public static string SpotifyClientId { get; set; }
        public static string SpotifyClientSecret { get; set; }
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
                .AddSingleton(_commandService)
                .AddSingleton<MusicService>()
                .AddSingleton(LogAdaptor.CreateLogger())
                .AddSingleton<LogAdaptor>();
            var provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            // Autowire and create these dependencies now
            provider.GetService<LogAdaptor>();
            provider.GetService<MusicService>();

            return provider;
        }

    }
}
