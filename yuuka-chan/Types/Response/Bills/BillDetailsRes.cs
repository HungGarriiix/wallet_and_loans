using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using yuuka_chan.Types.Response.Items;
using yuuka_chan.Types.Response.Wallets;

namespace yuuka_chan.Types.Response.Bills
{
    public class BillDetailsRes
    {
        [JsonProperty("id")]
        public int ID { get; private set; }
        [JsonProperty("date")]
        public DateTime Date { get; private set; }
        [JsonProperty("description")]
        public string Description { get; private set; }
        [JsonProperty("walletUsedID")]
        public WalletSumRes WalletUsedID { get; private set; }
        [JsonProperty("owner")]
        public string Owner { get; private set; }
        [JsonProperty("items")]
        public List<BillItemRes> Items { get; private set; }
    }
}
