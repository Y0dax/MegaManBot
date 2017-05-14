using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models.DomainModels
{
    class GuildOptions
    {
        [BsonId]
        public UInt64 GuildId { get; set; }
        public string CommandString { get; set; } = ".";
        public bool BreweryEnabled { get; set; } = true;
        public bool GiphyEnabled { get; set; } = true;
        public bool RedditEnabled { get; set; } = true;
        public bool WeatherEnabled { get; set; } = true;
        public bool GoogleEnabled { get; set; } = true;
        public bool XKCDEnabled { get; set; } = true;
        public bool IMDBEnabled { get; set; } = true;
    }
}
