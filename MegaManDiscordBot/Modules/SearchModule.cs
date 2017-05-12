using Discord;
using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MegaManDiscordBot.Modules.Models.SearchModels;
using static MegaManDiscordBot.Modules.Models.SearchModels.BreweryDBModels;
using static MegaManDiscordBot.Modules.Models.SearchModels.RedditModels;
using static MegaManDiscordBot.Modules.Models.SearchModels.WeatherModels;

namespace MegaManDiscordBot.Modules
{
    public class SearchModule : ModuleBase<SocketCommandContext>
    {
        static string beerUrl = $"http://api.brewerydb.com/v2/";

        [Command("beer")]
        [Summary("Search for a beer")]
        [Remarks("<beer name>")]
        [MinPermissions(AccessLevel.User)]
        public async Task BeerSearch([Remainder]string searchString)
        {

            Uri uri = new Uri($"{beerUrl}search?q={searchString.Replace(" ", "+")}&type=beer&key={Globals.BreweryKey}");
            var response = await new ApiHandler<BrewerySearchResponse>().GetJSONAsync(uri);
            if (response != null && response.status == "success")
            {
                if (response.data != null && response.data.Any())
                {
                    var beer = response.data.Where(p => p.name == searchString).FirstOrDefault() ?? response.data.First();

                    var embed = new EmbedBuilder().WithColor(new Color(Convert.ToUInt32("00a4e3", 16)))
                            .WithTitle($"I found {Format.Bold(beer.name)}!")
                            .WithImageUrl((beer.labels?.medium != null ? $"{beer.labels.medium}" : ""))
                            .WithDescription($"Style: {beer.style.name}\n" +
                                                        (beer.abv != null ? $"ABV: {beer.abv}%\n" : "") +
                                                        (beer.IBU != null ? $"IBU's: {beer.IBU}\n" : "") +
                                                        (beer.description != null ? $"\n{beer.description}\n\n" : ""));

                    await ReplyAsync("", false, embed);
                }
                else
                {
                    await ReplyAsync("Sorry, I couldn't find your beer.");
                }
            }
        }

        [Command("beer")]
        [Summary("Get a random beer")]
        [MinPermissions(AccessLevel.User)]
        public async Task RandomBeer()
        {
            Uri uri = new Uri($"{beerUrl}beer/random?key={Globals.BreweryKey}");
            var response = await new ApiHandler<BreweryRandomResponse>().GetJSONAsync(uri);
            if (response?.data != null)
            {
                var beer = response.data;

                var embed = new EmbedBuilder().WithColor(new Color(Convert.ToUInt32("00a4e3", 16)))
                                                .WithTitle($"I found {Format.Bold(beer.name)}!")
                                                .WithImageUrl((beer.labels?.medium != null ? $"{beer.labels.medium}" : ""))
                                                .WithDescription($"Style: {beer.style.name}\n" +
                                                (beer.abv != null ? $"ABV: {beer.abv}%\n" : "") +
                                                (beer.IBU != null ? $"IBU's: {beer.IBU}\n" : "") +
                                                (beer.description != null ? $"\n{beer.description}\n\n" : ""));

                await ReplyAsync("", false, embed);

            }
        }

        string googleUrl = "http://www.google.com/";

        [Command("google")]
        [Summary("Search goolge")]
        [Remarks("<seach text>")]
        public async Task GiphySearch([Remainder]string searchString)
        {
            await ReplyAsync($"{googleUrl}search?q={searchString.Replace(" ", "+")}");
        }

        static string redditUrl = "https://www.reddit.com";

        [Command("reddit")]
        [Summary("Get hot reddit posts")]
        [Remarks("<sub>")]
        [MinPermissions(AccessLevel.User)]
        public async Task GetHotPosts([Remainder]string searchString)
        {
            Uri uri = new Uri($"{redditUrl}/r/{searchString}/hot/.json?limit=5");
            var response = await new ApiHandler<RedditListingResult>().GetJSONAsync(uri);
            if (response?.Results != null)
            {
                if (response.Results.Posts != null && response.Results.Posts.Any())
                {
                    StringBuilder replyString = new StringBuilder();
                    var posts = response.Results.Posts;
                    int i = 0;
                    posts.Where(p => !p.PostData.Stickied).Take(3).ToList().ForEach(p =>
                    {
                        replyString.AppendLine($"{++i}: {redditUrl}{p.PostData.Permalink}");
                    });
                    await ReplyAsync($"Top Hot Posts in r/{Format.Bold(searchString)}:\n\n{replyString}");
                }
                else
                {
                    await ReplyAsync("Sorry, I couldn't find any posts for that subreddit.");
                }
            }
        }

        private const string movieUrl = "http://www.omdbapi.com/";

        [Command("movie")]
        [Summary("Search for a movie")]
        [Remarks("<search text>")]
        [MinPermissions(AccessLevel.User)]
        public async Task FindMovie([Remainder]string searchString)
        {

            Uri uri = new Uri($"{movieUrl}?t={searchString.Trim().Replace(" ", "+")}&y=&plot=full&r=json");
            var movie = await new ApiHandler<OmdbMovie>().GetJSONAsync(uri);
            if (movie != null)
            {
                await ReplyAsync("", false, movie.GetEmbed());
            }
        }

        static string weatherUrl = $"http://api.openweathermap.org/data/2.5/";

        [Command("weather")]
        [Summary("Get weather")]
        [Remarks("<location>")]
        [MinPermissions(AccessLevel.User)]
        public async Task WeatherSearch([Remainder]string searchString)
        {

            Uri uri = new Uri($"{weatherUrl}weather?q={searchString.Replace(" ", "+")}&units=imperial&appid={Globals.WeatherKey}");
            var response = await new ApiHandler<WeatherObj>().GetJSONAsync(uri);
            if (response?.Weather != null)
            {
                var r = response;

                await ReplyAsync($"The current weather condition for {Format.Bold(r.Name)} is {r.Weather.First().Description}. The tempurature is {r.Main.Temp.ToString()} degrees with a low of {r.Main.MinTemp.ToString()} and a high of {r.Main.MaxTemp.ToString()}. Humidity is {r.Main.Humidity.ToString()}%.");
            }
        }

    }
}
