using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using DiscordBot.Services;
using System.Numerics;

namespace DiscordBot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public PictureService PictureService { get; set; }
        public MusicService MusicService { get; set; }

        [Command("gandalf")]
        [Alias("Gandalf")]
        public Task GandalfAsync() 
            => ReplyAsync(GetGandalfQuote());

        [Command("sven")]
        [Alias("Sven")]
        public async Task CatAsync()
        {
            // Get a stream containing an image of a cat
            var stream = await PictureService.GetCat();
            // Streams must be seeked to their beginning before being uploaded!
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        [Command("meme")]
        [Alias("Meme")]
        public async Task MemeAsync()
        {
            string url = await PictureService.GetMeme();
            await Context.Channel.SendMessageAsync(url);
        }
        
        [Command("wholesome")]
        [Alias("Wholesome")]
        public async Task WholesomeAsync()
        {
            string url = await PictureService.GetMeme("wholesomememes");
            await Context.Channel.SendMessageAsync(url);
        }

        [Command("commands")]
        [Alias("Commands")]
        public Task ListCommands()
            => ReplyAsync(GetCommands());

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayMusic(params String[] input)
        {
            ulong guildId = Context.Guild.Id;

            IVoiceChannel channel = null;
            channel = channel ?? (Context.User as IGuildUser).VoiceChannel;
            if (channel == null) 
            { 
                await Context.Channel.SendMessageAsync("User must be in a voice channel"); 
                return; 
            }

            //store input in array, else user can not input multiple words with whitespace
            //store each element of the array in a string representing our search query
            string query = "";
            foreach (string s in input) { query += s; }

            await MusicService.PlaySongAsync(guildId, channel.Id, query);
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannel()
        {
            ulong guildId = Context.Guild.Id;

            IVoiceChannel channel = null;
            channel = channel ?? (Context.User as IGuildUser).VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("User must be in a voice channel");
                return;
            }
            
            await MusicService.Join(guildId, channel.Id);
        }
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveChannel() => 
            await MusicService.Leave(Context.Guild.Id);
        
        private static String GetGandalfQuote()
        {
            string quote1 = "**I was talking aloud to myself. A habit of the old: they choose the wisest person present to speak to.**";
            string quote2 = "**I am a servant of the Secret Fire, wielder of the flame of Anor. " +
                            "You cannot pass.The dark fire will not avail you, flame of Udûn. Go back to the Shadow! You cannot pass.**";
            string quote3 = "**He that breaks a thing to find out what it is has left the path of wisdom.**";
            string quote4 = "**Only a small part is played in great deeds by any hero.**";
            string quote5 = "**Fool of a Took!**";
            string[] quotes = { quote1, quote2, quote3, quote4, quote5 };

            string[] titles = { "*- Gandalf the Grey*", "*- Mithrandir*", "*- Olorin*", "*- Gandalf Stormcrow*", "*- Gandalf the White*" };
            
            Random random = new Random();
            return Format.Bold(quotes[random.Next(0, 4)]) + " " + titles[random.Next(0, 4)];
        }

        private static String GetCommands()
        {
            string commands = "**!gandalf =>** Get Gandalf quote\n**!sven =>** Get cat picture\n**!meme =>** Get dank meme\n" +
                              "**!wholesome =>** Get wholesome meme\n**!commands =>** List all commands\n**!join =>** Join voice chat\n" +
                              "**!play =>** Play audio from Youtube URL\n**!leave =>** Leave voice chat";

            return Format.BlockQuote(commands);
        }
    }
}
