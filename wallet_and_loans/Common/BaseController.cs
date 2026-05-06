using wallet_and_loans_api.Common.Session;

namespace wallet_and_loans_api.Common
{
    public class BaseController
    {
        protected readonly ISessionDataProvider _sessionDataProvider;

        public BaseController(ISessionDataProvider sessionDataProvider)
        {
            _sessionDataProvider = sessionDataProvider;
        }
    }
}
