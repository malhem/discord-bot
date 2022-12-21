using Discord;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class MusicService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IAudioService _audio;

        public MusicService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _audio = services.GetRequiredService<IAudioService>();
        }

        public async Task PlaySongAsync(ulong guildId, ulong channelId, string query)
        {

            var player = _audio.GetPlayer<LavalinkPlayer>(guildId)
                ?? await _audio.JoinAsync<LavalinkPlayer>(guildId, channelId);

            var track = await _audio.GetTrackAsync(query, SearchMode.YouTube);

            await player.PlayAsync(track);
        }

        public async Task Join(ulong guildId, ulong channelId) => await _audio.JoinAsync(guildId, channelId);
        public async Task Leave(ulong guildId) => await _audio.GetPlayer(guildId).DisconnectAsync();
    }
}
