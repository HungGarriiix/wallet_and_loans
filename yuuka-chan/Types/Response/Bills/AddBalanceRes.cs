using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace yuuka_chan.Types.Response.Bills
{
    public class AddBalanceRes
    {
        [JsonProperty("bill")]
        public BillDetailsRes Bill { get; set; }

        [JsonProperty("expectedBalance")]
        public double ExpectedBalance { get; set; }
    }
}
