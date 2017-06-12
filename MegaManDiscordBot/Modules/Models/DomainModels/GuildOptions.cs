using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models.DomainModels
{
    public class GuildOptions
    {
        [BsonId]
        public UInt64 GuildId { get; set; }
        public string CommandString { get; set; } = ".";
        public CommandModules Modules { get; set; } = new CommandModules();
    }

    public class CommandModules
    {
        public bool Brewery { get; set; } = true;
        public bool Giphy { get; set; } = true;
        public bool Reddit { get; set; } = true;
        public bool Weather { get; set; } = true;
        public bool Google { get; set; } = true;
        public bool XKCD { get; set; } = true;
        public bool IMDB { get; set; } = true;
        public bool Polls { get; set; } = true;
    }
}
