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
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        static string baseUrl = $"http://api.openweathermap.org/data/2.5/";

        [Command("weather")]
        [Summary("Get weather")]
        [Remarks("<location>")]
        [MinPermissions(AccessLevel.User)]
        public async Task WeatherSearch([Remainder]string searchString)
        {

            Uri uri = new Uri($"{baseUrl}weather?q={searchString.Replace(" ", "+")}&units=imperial&appid={Globals.WeatherKey}");
            ApiResponse<WeatherObj> response = await new ApiHandler<WeatherObj>().GetJSONAsync(uri);
            if (response.Success && response.responseObject != null)
            {
                var r = response.responseObject;
                //await ReplyAsync(
                //    $"{Format.Bold(r.Name + " Weather Conditions")}\n\n" +
                //    $"Conditions: {r.Weather.First().Description}\n" +
                //    $"Current Temp: {r.Main.temp.ToString()} degrees\n" +
                //    $"Low: {r.Main.temp_min.ToString()}, High: {r.Main.temp_max.ToString()}\n" +
                //    $"Humidity: {r.Main.humidity.ToString()}"
                //    );

                await ReplyAsync($"The current weather condition for {Format.Bold(r.Name)} is {r.Weather.First().Description}. The tempurature is {r.Main.Temp.ToString()} degrees with a low of {r.Main.MinTemp.ToString()} and a high of {r.Main.MaxTemp.ToString()}. Humidity is {r.Main.Humidity.ToString()}%.");
            }
            else
            {
                await ReplyAsync("Sorry, I couldn't find weather for that location.");
            }
        }

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

