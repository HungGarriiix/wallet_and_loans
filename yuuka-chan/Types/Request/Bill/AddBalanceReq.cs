using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace yuuka_chan.Types.Request.Bill
{
    public class AddBalanceReq
    {
        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("walletUsedID")]
        public long WalletUsedID { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
