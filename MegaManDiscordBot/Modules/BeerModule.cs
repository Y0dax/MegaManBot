using Discord;
using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules
{
    public class BeerModule : ModuleBase<SocketCommandContext>
    {
        static string baseUrl = $"http://api.brewerydb.com/v2/";
        static string key = $"key={Globals.BreweryKey}";

        [Command("beer")]
        [Summary("Search for a beer")]
        [Remarks("<beer name>")]
        [MinPermissions(AccessLevel.User)]
        public async Task BeerSearch([Remainder]string searchString)
        {

            Uri uri = new Uri($"{baseUrl}search?q={searchString.Replace(" ", "+")}&type=beer&{key}");
            ApiResponse<BrewerySearchResponse> response = await new ApiHandler<BrewerySearchResponse>().GetJSONAsync(uri);
            if (response.Success && response.responseObject != null && response.responseObject.status == "success")
            {
                if (response.responseObject.data != null && response.responseObject.data.Any())
                {
                    var beer = response.responseObject.data.Where(p => p.name == searchString).FirstOrDefault() ?? response.responseObject.data.First();

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
            Uri uri = new Uri($"{baseUrl}beer/random?{key}");
            ApiResponse<BreweryRandomResponse> response = await new ApiHandler<BreweryRandomResponse>().GetJSONAsync(uri);
            if (response.Success && response.responseObject != null && response.responseObject.status == "success" && response.responseObject.data != null)
            {
               var beer = response.responseObject.data;
                await ReplyAsync(
                    $"Try a pint of {Format.Bold(beer.name)}!\n\n" +
                    Format.Code($"Style: {beer.style.name}\n" +
                    (beer.abv != null ? $"ABV: {beer.abv}%\n" : "") +
                    (beer.IBU != null ? $"IBU's: {beer.IBU}\n" : "") +
                    (beer.description != null ? $"\n{beer.description}\n\n" : "")) +
                    (beer.labels?.medium != null ? $"{beer.labels.medium}" : "")
                    );
            }
        }


        public class Glass
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("createDate")]
            public string createDate { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
        }

        public class Category
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("createDate")]
            public string createDate { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
        }

        public class Style
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("category")]
            public Category category { get; set; }
            [JsonProperty("srmMax")]
            public double srmMax { get; set; }
            [JsonProperty("ibuMax")]
            public double ibuMax { get; set; }
            [JsonProperty("srmMin")]
            public double srmMin { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
            [JsonProperty("fgMin")]
            public double fgMin { get; set; }
            [JsonProperty("ibuMin")]
            public double ibuMin { get; set; }
            [JsonProperty("createDate")]
            public string createDate { get; set; }
            [JsonProperty("fgMax")]
            public double fgMax { get; set; }
            [JsonProperty("abvMax")]
            public double abvMax { get; set; }
            [JsonProperty("ogMin")]
            public double ogMin { get; set; }
            [JsonProperty("abvMin")]
            public double abvMin { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("categoryId")]
            public int categoryId { get; set; }
        }

        public class Labels
        {
            [JsonProperty("medium")]
            public string medium { get; set; }
            [JsonProperty("large")]
            public string large { get; set; }
            [JsonProperty("icon")]
            public string icon { get; set; }
        }

        public class Data
        {
            [JsonProperty("id")]
            public string id { get; set; }
            [JsonProperty("abv")]
            public string abv { get; set; }
            [JsonProperty("ibu")]
            public string IBU { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("nameDisplay")]
            public string NameDisplay { get; set; }
            [JsonProperty("glass")]
            public Glass glass { get; set; }
            [JsonProperty("style")]
            public Style style { get; set; }
            [JsonProperty("createDate")]
            public string createDate { get; set; }
            [JsonProperty("labels")]
            public Labels labels { get; set; }
            [JsonProperty("styleId")]
            public int styleId { get; set; }
            [JsonProperty("updateDate")]
            public string updateDate { get; set; }
            [JsonProperty("glasswareId")]
            public int glasswareId { get; set; }
            [JsonProperty("isOrganic")]
            public string isOrganic { get; set; }
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("statusDisplay")]
            public string statusDisplay { get; set; }
        }

        public class BreweryRandomResponse
        {
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("data")]
            public Data data { get; set; }
            [JsonProperty("message")]
            public string message { get; set; }
        }

        public class BrewerySearchResponse
        {
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("data")]
            public List<Data> data { get; set; }
        }

    }
}
