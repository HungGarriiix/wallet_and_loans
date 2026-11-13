using wallet_and_loans_api.Common.Session;

namespace wallet_and_loans_api.Common
{
    public class BaseService
    {
        protected readonly ISessionDataProvider _sessionDataProvider;

        public BaseService(ISessionDataProvider sessionDataProvider)
        {
            _sessionDataProvider = sessionDataProvider;
        }
    }
}
