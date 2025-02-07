using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace yuuka_chan
{
    internal class Program
    {
        // main client connection
        public static DiscordClient Client { get; set; }

        // interactivity centre: emoji reactions, replies, etc.
        public static InteractivityExtension Interactivity { get; private set; }

        // commands centre, where to assign BaseCommandModule classes
        public static SlashCommandsExtension Commands { get; private set; }

        
        public static async Task Main(string[] args)
        {
            // JSON reader
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            var configJson = JsonConvert.DeserializeObject<ConfigJSON>(json);

            // Discord client main configuration (for 2 hand shake protocol)
            var config = new DiscordConfiguration()
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            
            Client = new DiscordClient(config);
            var slash = Client.UseSlashCommands();

            // Starts connecting (2 hand shake protocol)
            await Client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
