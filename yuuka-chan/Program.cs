using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using yuuka_chan.Command;
using yuuka_chan.Common;
using yuuka_chan.Types.Response.Auth;

namespace yuuka_chan
{
    public class Program
    {
        public static DiscordClient Client { get; set; }
        public static InteractivityExtension Interactivity { get; private set; }
        public static SlashCommandsExtension Commands { get; private set; }
        public static string URL { get; private set; }
        public static ulong GuildID { get; private set; }
        public static HttpClient Service { get; private set; } = new HttpClient();

        private static readonly Dictionary<ulong, string> _tokenCache = new();

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

        public static async Task<HttpResponseMessage> PatchAuthorizedAsync(string url, ulong discordUserId, HttpContent body)
        {
            var token = await GetTokenAsync(discordUserId);
            var req = new HttpRequestMessage(HttpMethod.Patch, url) { Content = body };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Add("X-Session-Id", discordUserId.ToString());
            req.Headers.Add("X-Platform-Id", YuukaConstants.PLATFORM_ID);
            return await Service.SendAsync(req);
        }

        public static async Task Main(string[] args)
        {
            var configJson = await LoadConfigAsync();

            var config = new DiscordConfiguration()
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            URL = configJson.URL;
            GuildID = configJson.GuildID;
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
                CookieContainer = new CookieContainer()
            };
            Service = new HttpClient(clientHandler);

            Client = new DiscordClient(config);
            Interactivity = Client.UseInteractivity(new InteractivityConfiguration());

            var slash = Client.UseSlashCommands();
            slash.RegisterCommands<BillCommand>(GuildID);
            slash.RegisterCommands<WalletCommand>(GuildID);
            slash.RegisterCommands<AuthCommand>(GuildID);
            slash.RegisterCommands<NonAuthCommand>(GuildID);

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task<ConfigJSON> LoadConfigAsync()
        {
            string configPath = "config.json";
            if (!File.Exists(configPath))
            {
                configPath = Path.Combine("yuuka-chan", "config.json");
            }

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("Could not find config.json");
            }

            var json = await File.ReadAllTextAsync(configPath);
            return JsonConvert.DeserializeObject<ConfigJSON>(json) ?? throw new Exception("Failed to deserialize config.json");
        }
    }
}
