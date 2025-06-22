using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan.Types.Response.Wallets
{
    public class WalletSumRes
    {
        [JsonProperty("id")]
        public int ID { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
    }
}
