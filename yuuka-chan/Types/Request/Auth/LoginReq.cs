using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan.Types.Request.Auth
{
    public class LoginReq
    {
        public LoginReq(string username)
        {
            UserName = username;
        }

        public LoginReq() { }

        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
