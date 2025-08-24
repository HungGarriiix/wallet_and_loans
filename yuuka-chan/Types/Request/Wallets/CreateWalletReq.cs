using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan.Types.Request.Wallets
{
    public class CreateWalletReq
    {
        public CreateWalletReq(string name, float balance) 
        {
            Name = name;
            Balance = balance;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("balance")]
        public float Balance { get; set; }
    }
}
