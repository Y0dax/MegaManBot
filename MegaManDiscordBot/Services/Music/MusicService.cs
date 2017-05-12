using Discord.Commands;
using Discord.WebSocket;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Music
{
    public class MusicService
    {
        public MusicService()
        {
        }

        public async Task WriteSongs(ISocketMessageChannel channel, List<PlaylistTrack> songs)
        {
            foreach(var song in songs)
            {
                await channel.SendMessageAsync($"!add {song.Track.Name} {song.Track.Artists?.FirstOrDefault()?.Name ?? ""}");
                await Task.Delay(5000);
            }
        }

    }
}
