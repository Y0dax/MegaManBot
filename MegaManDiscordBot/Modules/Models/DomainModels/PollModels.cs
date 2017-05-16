using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models.DomainModels
{
    public class PollModels
    {
        public class Poll
        {
            [BsonId]
            public ObjectId PollId { get; set; }
            public UInt64 GuildId { get; set; }
            public UInt64 CreatorId { get; set; }
            public DateTime CreateDate { get; set; }
            public string Title { get; set; }
            public List<PollOption> Options { get; set; } = new List<PollOption>();
            public List<PollVote> Votes { get; set; } = new List<PollVote>();
            public bool IsOpen { get; set; }
            public bool AllowAddOptions { get; set; }
        }

        public class PollVote
        {
            //public int VoteId { get; set; }
            public UInt64 UserId { get; set; }
            public int OptionId { get; set; }
        }

        public class PollOption
        {
            public int OptionId { get; set; }
            public string OptionText { get; set; }
        }
    }
}
