using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using MegaManDiscordBot.Services.Common;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using static MegaManDiscordBot.Modules.Models.DomainModels.PollModels;

namespace MegaManDiscordBot.Services.Polls
{
    public class PollService
    {
        private readonly IServiceProvider _provider;
        private readonly Database _dbHandler;
        private readonly ILogger _logger;
        private IMongoCollection<Poll> _polls;

        public PollService(IServiceProvider provider)
        {
            _provider = provider;
            _dbHandler = _provider.GetService<Database>();
            _polls = _dbHandler._database.GetCollection<Poll>("Polls");
            _logger = _provider.GetService<Logger>().ForContext<CommandService>();
        }

        public async Task<List<Poll>> GetAllPolls()
        {
            var allPolls = await _polls.Find(_ => true).ToListAsync();
            return allPolls;
        }

        public async Task<List<Poll>> GetGuildPolls(UInt64 guildId)
        {
            var polls = await _polls.Find(x => x.GuildId == guildId).Limit(10).SortByDescending(x => x.CreateDate).ToListAsync();
            return polls;
        }

        public async Task<Poll> GetPoll(ObjectId pollId)
        {
            var poll = await _polls.Find(x => x.PollId == pollId).FirstOrDefaultAsync();
            return poll;
        }

        public async Task AddPoll(Poll poll)
        {
            await _polls.InsertOneAsync(poll);
        }

        public async Task<Poll> AddVote(ObjectId pollId, PollVote vote)
        {
            var filter = Builders<Poll>.Filter.Eq(x => x.PollId, pollId);
            var update = Builders<Poll>.Update.Push<PollVote>(e => e.Votes, vote);
            var response = await _polls.FindOneAndUpdateAsync(filter, update);
            return response;
        }

        public async Task<Poll> ChangeVote(ObjectId pollId, PollVote vote)
        {
            var response = await _polls.FindOneAndUpdateAsync(p => p.PollId == pollId && p.Votes.Any(v => v.UserId == vote.UserId), Builders<Poll>.Update.Set(p => p.Votes[-1], vote));     // -1 means update first matching array element
            return response;
        }

        public async Task<Poll> ClosePoll(ObjectId pollId, UInt64 userId, UInt64 guildId)
        {
            var response = await _polls.FindOneAndUpdateAsync(p => p.PollId == pollId && p.CreatorId == userId && p.GuildId == guildId, Builders<Poll>.Update.Set(p => p.IsOpen, false));
            return response;
        }

        public async Task<DeleteResult> DeletePoll(ObjectId pollId, UInt64 guildId)
        {
            var response = await _polls.DeleteOneAsync(p => p.PollId == pollId && p.GuildId == guildId);
            return response;
        }

    }
}
