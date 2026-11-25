using Newtonsoft.Json;

namespace yuuka_chan.Types.Response.Wallets
{
    public class WalletListRes
    {
        [JsonProperty("message")]
        public string? Message { get; private set; }
        [JsonProperty("wallets")]
        public WalletRes[] Wallets { get; private set; } = [];
    }
}
