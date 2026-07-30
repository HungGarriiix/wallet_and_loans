using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
