using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace yuuka_chan.Types.Request.Bill
{
    public class UpdateBillReq
    {
        public UpdateBillReq(string description, DateTime date, long walletID)
        {
            Description = description;
            Date = date;
            WalletUsedId = walletID;
        }

        public UpdateBillReq() { }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("walletUsedId")]
        public long WalletUsedId { get; set; }
    }
}
