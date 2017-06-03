using Discord.Commands;
using MegaManDiscordBot.Modules.Models.DomainModels;
using MegaManDiscordBot.Services.Common;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Moderator
{
    public class GuildOptionsService
    {
        private readonly IServiceProvider _provider;
        private readonly Database _dbHandler;
        private readonly ILogger _logger;
        private IMongoCollection<GuildOptions> _guildOptions;

        public GuildOptionsService(IServiceProvider provider)
        {
            _provider = provider;
            _dbHandler = _provider.GetService<Database>();
            _guildOptions = _dbHandler._database.GetCollection<GuildOptions>("GuildOptions");
            _logger = _provider.GetService<Logger>().ForContext<CommandService>();
        }

        public async Task<List<GuildOptions>> GetAllGuildOptions()
        {
            return await _guildOptions.Find(_ => true).ToListAsync();
        }

        public async Task<GuildOptions> GetGuildOptions(UInt64 guildId)
        {
            return await _guildOptions.Find(x => x.GuildId == guildId).FirstOrDefaultAsync();
        }

        public async Task<string> GetCommandString(UInt64 guildId)
        {
            return await _guildOptions.Find(x => x.GuildId == guildId).Project(x => x.CommandString).FirstOrDefaultAsync();
        }

        public async Task<GuildOptions> GetEnabledStatus(UInt64 guildId, string moduleName)
        {
            return await _guildOptions.Find(x => x.GuildId == guildId).FirstOrDefaultAsync();
        }

        public async Task<UpdateResult> UpdatePrefix(UInt64 guildId, char prefix)
        {
            var filter = Builders<GuildOptions>.Filter.Eq(x => x.GuildId, guildId);
            var update = Builders<GuildOptions>.Update.Set<string>(e => e.CommandString, prefix.ToString());
            var response = await _guildOptions.UpdateOneAsync(filter, update, options: new UpdateOptions { IsUpsert = true });
            return response;
        }

        public async Task<ReplaceOneResult> UpdateGuildOptions(GuildOptions guildOptions)
        {
            return await _guildOptions.ReplaceOneAsync(x => x.GuildId == guildOptions.GuildId, guildOptions,  options: new UpdateOptions { IsUpsert = true });
        }

    }
}
