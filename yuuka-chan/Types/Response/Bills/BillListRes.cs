using Newtonsoft.Json;

namespace yuuka_chan.Types.Response.Bills
{
    public class BillListRes
    {
        [JsonProperty("message")]
        public string? Message { get; private set; }
        [JsonProperty("bills")]
        public BillRes[] Bills { get; private set; } = [];
    }
}
