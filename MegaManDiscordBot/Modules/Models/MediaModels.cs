using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models
{
    class MediaModels
    {
        public class GiphyModels
        {
            public class GiphySearchResult
            {
                [JsonProperty("data")]
                public List<GiphyData> Data { get; set; }
            }

            public class GiphySingleResult
            {
                [JsonProperty("data")]
                public GiphyData Data { get; set; }
            }

            public class GiphyData
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("url")]
                public string Url { get; set; }
            }
        }

        public class XKCD
        {
            [JsonProperty("num")]
            public int Num { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("img")]
            public string Url { get; set; }
        }


    }
}
