using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan
{
    public class ConfigJSON
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
        [JsonProperty("prefix")]
        public string Prefix { get; set; } = string.Empty;
        [JsonProperty("url")]
        public string URL { get; set; } = string.Empty;
        [JsonProperty("guildId")]
        public ulong GuildID { get; set; }

        public static async Task<ConfigJSON> ReadConfig()
        {
            ConfigJSON configJson = new ConfigJSON();
            string? configPath = FindExistingFile("config.json", Path.Combine("yuuka-chan", "config.json"));
            if (configPath != null)
            {
                var json = await File.ReadAllTextAsync(configPath);
                configJson = JsonConvert.DeserializeObject<ConfigJSON>(json) ?? new ConfigJSON();
            }

            string? envPath = FindExistingFile(".env", Path.Combine("yuuka-chan", ".env"));
            var envMap = LoadDotEnv(envPath);
            configJson.Token = GetConfigValue(new[] { "TOKEN" }, envMap, configJson.Token);
            configJson.Prefix = GetConfigValue(new[] { "PREFIX" }, envMap, configJson.Prefix);
            configJson.URL = GetConfigValue(new[] { "URL" }, envMap, configJson.URL);

            string guildId = GetConfigValue(new[] { "GUILD_ID", "GUILDID" }, envMap, configJson.GuildID.ToString());
            if (!string.IsNullOrWhiteSpace(guildId))
                configJson.GuildID = ulong.Parse(guildId);

            // Validation
            if (string.IsNullOrWhiteSpace(configJson.Token))
                throw new Exception("Missing bot token. Set TOKEN in .env or token in config.json.");
            if (string.IsNullOrWhiteSpace(configJson.URL))
                throw new Exception("Missing API URL. Set URL in .env or url in config.json.");
            if (configJson.GuildID == 0)
                throw new Exception("Missing guild id. Set GUILD_ID in .env or guildId in config.json.");

            return configJson;
        }

        private static string? FindExistingFile(params string[] candidates)
        {
            foreach (var candidate in candidates)
            {
                if (File.Exists(candidate))
                    return candidate;
            }

            return null;
        }

        private static Dictionary<string, string> LoadDotEnv(string? path)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return result;

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                var separator = line.IndexOf('=');
                if (separator <= 0)
                    continue;

                var key = line.Substring(0, separator).Trim();
                var value = line.Substring(separator + 1).Trim().Trim('"');
                result[key] = value;
            }

            return result;
        }

        private static string GetConfigValue(string[] keys, Dictionary<string, string> envMap, string fallback)
        {
            foreach (var key in keys)
            {
                var fromProcess = Environment.GetEnvironmentVariable(key);
                if (!string.IsNullOrWhiteSpace(fromProcess))
                    return fromProcess;

                if (envMap.TryGetValue(key, out var fromDotEnv) && !string.IsNullOrWhiteSpace(fromDotEnv))
                    return fromDotEnv;
            }

            return fallback;
        }
    }
}
