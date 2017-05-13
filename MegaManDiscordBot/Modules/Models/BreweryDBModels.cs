using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Modules.Models
{
    public class BreweryDBModels
    {

        public class BreweryRandomBeer
        {
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("data")]
            public Beer data { get; set; }
            [JsonProperty("message")]
            public string message { get; set; }
        }

        public class BreweryRandomBrewery
        {
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("data")]
            public Brewery data { get; set; }
            [JsonProperty("message")]
            public string message { get; set; }
        }

        public class BrewerySearchResponse
        {
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("data")]
            public List<Beer> data { get; set; }
        }

        public class Beer
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
            public Images labels { get; set; }
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

        public class Images
        {
            [JsonProperty("medium")]
            public string medium { get; set; }
            [JsonProperty("large")]
            public string large { get; set; }
            [JsonProperty("icon")]
            public string icon { get; set; }
        }

        public class Brewery
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("ame")]
            public string Name { get; set; }
            [JsonProperty("isOrganic")]
            public string IsOrganic { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("statusDisplay")]
            public string StatusDisplay { get; set; }
            [JsonProperty("locations")]
            public List<Location> Locations { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("website")]
            public string Website { get; set; }
            [JsonProperty("established")]
            public string Established { get; set; }
            [JsonProperty("image")]
            public Images Image { get; set; }
        }

        public class Location
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string IsPrimary { get; set; }
            public string InPlanning { get; set; }
            public string IsClosed { get; set; }
            public string OpenToPublic { get; set; }
            public string LocationType { get; set; }
            public string LocationTypeDisplay { get; set; }
            public string CountryIsoCode { get; set; }
            public string Status { get; set; }
            public string StatusDisplay { get; set; }
            public Country Country { get; set; }
            public string StreetAddress { get; set; }
            public string Locality { get; set; }
            public string Region { get; set; }
            public string PostalCode { get; set; }
            public string Website { get; set; }
            public string HoursOfOperation { get; set; }
            public string YearOpened { get; set; }
            public string OpenTo { get; set; }
        }

        public class Country
        {
            public string Name { get; set; }
            public string DisplayNam { get; set; }
        }
    }
}
