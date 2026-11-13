using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans_components.Logics
{
    public class LoginProfile
    {
        public LoginProfile() { }

        public LoginPlatformEnum Platform { get; set; }
        public string Id { get; set; }
        public string ProfileName { get; set; }
    }
}
