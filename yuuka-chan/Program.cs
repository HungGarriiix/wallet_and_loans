using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using yuuka_chan.Command;
using yuuka_chan.Types.Response.Auth;
using System.Diagnostics;
using System.Net;
using yuuka_chan.Common;

namespace yuuka_chan
{
    public class Program
    {
        // main client connection
        public static DiscordClient Client { get; set; }

        // interactivity centre: emoji reactions, replies, etc.
        public static InteractivityExtension Interactivity { get; private set; }

        // commands centre, where to assign BaseCommandModule classes
        public static SlashCommandsExtension Commands { get; private set; }

        public static string URL { get; private set; }
        public static ulong GuildID { get; private set; }
        public static HttpClient Service { get; private set; } = new HttpClient();

        // Cache: Discord user ID → JWT token
        private static readonly Dictionary<ulong, string> _tokenCache = new();

        // Called per command to ensure the user has a valid JWT.
        // Sends their Discord user ID to /api/auth/login and caches the token.
        public static async Task<string> GetTokenAsync(ulong discordUserId)
        {
            if (_tokenCache.ContainsKey(discordUserId))
                return _tokenCache[discordUserId];

            var payload = JsonConvert.SerializeObject(new { userName = discordUserId.ToString() });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await Service.PostAsync(URL + "/api/auth/login", content);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var loginRes = JsonConvert.DeserializeObject<LoginRes>(body);
            _tokenCache[discordUserId] = loginRes!.Token;
            return loginRes.Token;
        }

        // Authorized HTTP helpers — each call auto-logs in the user if needed
        public static async Task<HttpResponseMessage> GetAuthorizedAsync(string url, ulong discordUserId)
        {
            var token = await GetTokenAsync(discordUserId);
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Add("X-Session-Id", discordUserId.ToString());
            req.Headers.Add("X-Platform-Id", YuukaConstants.PLATFORM_ID);
            return await Service.SendAsync(req);
        }

        public static async Task<HttpResponseMessage> PostAuthorizedAsync(string url, ulong discordUserId, HttpContent body)
        {
            var token = await GetTokenAsync(discordUserId);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = body };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Add("X-Session-Id", discordUserId.ToString());
            req.Headers.Add("X-Platform-Id", YuukaConstants.PLATFORM_ID);
            return await Service.SendAsync(req);
        }

        public static async Task<HttpResponseMessage> PutAuthorizedAsync(string url, ulong discordUserId, HttpContent body)
        {
            var token = await GetTokenAsync(discordUserId);
            var req = new HttpRequestMessage(HttpMethod.Put, url) { Content = body };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Add("X-Session-Id", discordUserId.ToString());
            req.Headers.Add("X-Platform-Id", YuukaConstants.PLATFORM_ID);
            return await Service.SendAsync(req);
        }

        public static async Task Main(string[] args)
        {
            var configJson = await ConfigJSON.ReadConfig();

            // Discord client main configuration (for 2 hand shake protocol)
            var config = new DiscordConfiguration()
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            // Setup for dev server
            URL = configJson.URL;
            GuildID = configJson.GuildID;
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
                CookieContainer = new CookieContainer()
            };
            Console.WriteLine(GuildID);
            // Pass the handler to httpclient(from you are calling api)
            Service = new HttpClient(clientHandler);

            Client = new DiscordClient(config);
            Interactivity = Client.UseInteractivity(new InteractivityConfiguration());

            var slash = Client.UseSlashCommands();
            //Client.DeleteGuildApplicationCommandAsync();
            
            slash.RegisterCommands<BillCommand>(GuildID);
            slash.RegisterCommands<WalletCommand>(GuildID);
            slash.RegisterCommands<AuthCommand>(GuildID);

            // Starts connecting (2 hand shake protocol)
            await Client.ConnectAsync();
            
            await Task.Delay(-1);
            //await slash.RefreshCommands();
        }
    }
}
