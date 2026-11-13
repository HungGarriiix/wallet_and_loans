using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace wallet_and_loans_api.Common.Attributes
{
    public class UserAuthorizationAttribute: TypeFilterAttribute
    {
        public UserAuthorizationAttribute() : base(typeof(UserAuthorizationFilter))
        {
        }
    }
}
