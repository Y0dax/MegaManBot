using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models
{
    public class SearchModels
    { 
        public class RedditModels
        {
            public class RedditListingResult
            {
                [JsonProperty("data")]
                public Results Results { get; set; }
            }

            public class Results
            {
                [JsonProperty("children")]
                public List<Post> Posts { get; set; }
            }

            public class Post
            {
                [JsonProperty("data")]
                public PostData PostData { get; set; }
            }
            public class PostData
            {
                [JsonProperty("stickied")]
                public bool Stickied { get; set; }
                [JsonProperty("url")]
                public string Url { get; set; }
                [JsonProperty("title")]
                public string Title { get; set; }
                [JsonProperty("permalink")]
                public string Permalink { get; set; }
            }
        }

        public class OmdbMovie
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string ImdbRating { get; set; }
            public string ImdbId { get; set; }
            public string Genre { get; set; }
            public string Plot { get; set; }
            public string Poster { get; set; }

            public EmbedBuilder GetEmbed() =>
                new EmbedBuilder().WithTitle(Title)
                                  .WithUrl($"http://www.imdb.com/title/{ImdbId}/")
                                  .WithDescription(Plot)
                                  .AddField(efb => efb.WithName("Rating").WithValue(ImdbRating).WithIsInline(true))
                                  .AddField(efb => efb.WithName("Genre").WithValue(Genre).WithIsInline(true))
                                  .AddField(efb => efb.WithName("Year").WithValue(Year).WithIsInline(true))
                                  .WithImageUrl(Poster);

            public override string ToString() =>
            $@"`Title:` {Title}
        `Year:` {Year}
        `Rating:` {ImdbRating}
        `Genre:` {Genre}
        `Link:` http://www.imdb.com/title/{ImdbId}/
        `Plot:` {Plot}";
        }

        public class WeatherModels
        {
            public class Weather
            {
                [JsonProperty("description")]
                public string Description { get; set; }
            }

            public class Main
            {
                [JsonProperty("temp")]
                public double Temp { get; set; }
                [JsonProperty("pressure")]
                public int Pressure { get; set; }
                [JsonProperty("humidity")]
                public int Humidity { get; set; }
                [JsonProperty("temp_min")]
                public double MinTemp { get; set; }
                [JsonProperty("temp_max")]
                public double MaxTemp { get; set; }
            }

            public class Wind
            {
                [JsonProperty("speed")]
                public double Speed { get; set; }
                [JsonProperty("deg")]
                public int Deg { get; set; }
            }

            public class WeatherObj
            {
                [JsonProperty("weather")]
                public List<Weather> Weather { get; set; }
                [JsonProperty("main")]
                public Main Main { get; set; }
                [JsonProperty("name")]
                public string Name { get; set; }

            }
        }
    }
}
