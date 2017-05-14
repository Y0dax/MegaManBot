using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models.DomainModels
{
    class PollModels
    {
        public class Poll
        {
            [BsonId]
            public ObjectId PollId { get; set; }
            public UInt64 GuildId { get; set; }
            public UInt64 CreatorId { get; set; }
            public DateTime CreateDate { get; set; }
            public string Title { get; set; }
            public List<PollOptions> Options { get; set; }
            public List<PollVotes> Votes { get; set; }
            public bool IsOpen { get; set; }
            public bool AllowAddOptions { get; set; }
        }

        public class PollVotes
        {
            [BsonId]
            public ObjectId VoteId { get; set; }
            public ObjectId PollId { get; set; }
            public UInt64 UserId { get; set; }
            public ObjectId OptionsId { get; set; }
        }

        public class PollOptions
        {
            [BsonId]
            public ObjectId OptionId { get; set; }
            public ObjectId PollId { get; set; }
            public string OptionText { get; set; }
        }
    }
}
