using Newtonsoft.Json;

namespace yuuka_chan.Types.Response.Auth
{
    public class LoginRes
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }
}
