using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan
{
    public struct ConfigJSON
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        [JsonProperty("url")]
        public string URL { get; private set; }
        [JsonProperty("guildId")]
        public ulong GuildID { get; private set; }
    }
}
