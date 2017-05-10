using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using Newtonsoft.Json;

namespace MegaManDiscordBot.Modules
{
    public class RedditModule : ModuleBase<SocketCommandContext>
    {
        static string baseUrl = "https://www.reddit.com";

        [Command("reddit")]
        [Remarks("Get top reddit posts given a subreddit")]
        [MinPermissions(AccessLevel.User)]
        public async Task GetHotPosts([Remainder]string searchString)
        {
            Uri uri = new Uri($"{baseUrl}/r/{searchString}/hot/.json?limit=5");
            ApiResponse<RedditListingResult> response = await new ApiHandler<RedditListingResult>().GetJSONAsync(uri);
            if (response.Success)
            {
                if (response.responseObject != null && response.responseObject.Results != null && response.responseObject.Results.Posts != null && response.responseObject.Results.Posts.Any())
                {
                    StringBuilder replyString = new StringBuilder();
                    var posts = response.responseObject.Results.Posts;
                    int i = 0;
                    posts.Where(p => !p.PostData.Stickied).Take(3).ToList().ForEach(p =>
                    {
                        replyString.AppendLine($"{++i}: {baseUrl}{p.PostData.Permalink}");
                    });
                    await ReplyAsync($"Top Hot Posts in r/{Format.Bold(searchString)}:\n\n{replyString}");
                }
                else
                {
                    await ReplyAsync("Sorry, I couldn't find any posts for that subreddit.");
                }
            }
        }

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
}
