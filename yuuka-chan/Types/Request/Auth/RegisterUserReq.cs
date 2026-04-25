using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan.Types.Request.Auth
{
    public class RegisterUserReq
    {
        public string UserId { get; set; }
        public int PlatformId { get; set; }
        public string DisplayName { get; set; }
    }
}
