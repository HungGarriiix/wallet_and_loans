namespace wallet_and_loans_api.Common.Session
{
    public class SessionGet
    {
        private static ISessionDataProvider _sessionDataProvider;
        public SessionGet() 
        {
            
        }

        private static ISessionDataProvider Instance
        {
            get => _sessionDataProvider;
            set
            {
                if (_sessionDataProvider == null)
                {
                    _sessionDataProvider = new SessionDataProvider(new HttpContextAccessor());
                }
            }
        }

        //public string UserId
        //{
        //    get => Instance.UserId;
        //}

    }
}
