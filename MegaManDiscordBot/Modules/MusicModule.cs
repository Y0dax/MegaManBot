using Discord.Commands;
using MegaManDiscordBot.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using MegaManDiscordBot.Services.Music;

namespace MegaManDiscordBot.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly MusicService _service;

        public MusicModule(MusicService service)
        {
            _service = service;
        }

        [Command("addplaylist")]
        [Summary("Add 5 songs from spotify playlist to Meeseeks")]
        [Remarks("<playlist url> <(optional)start_track_number >")]
        [MinPermissions(AccessLevel.User)]
        public async Task AddPlaylist(string playlistUrl, int offset = 0)
        {

            var _spotify = new SpotifyWebAPI()
            {
                TokenType = Globals.SpotifyToken.TokenType,
                AccessToken = Globals.SpotifyToken.AccessToken,
                UseAuth = true
            };
            //https://open.spotify.com/user/spotify/playlist/37i9dQZF1DX3YlUroplxjF
            var items = playlistUrl.Split('/');

            var response = await _spotify.GetPlaylistTracksAsync(items[4], items[6], null, 5, offset);
            if (response != null && !response.HasError() && response.Items.Any())
            {

                //await _service.WriteSongs(this.Context.Channel, response.Items);
                foreach (var item in response.Items)
                {
                    await ReplyAsync($"!add {item.Track.Name} {item.Track.Artists?.FirstOrDefault()?.Name ?? ""}");
                }
            }
        }
    }
}
