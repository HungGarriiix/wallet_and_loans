namespace wallet_and_loans_api.Common.Session
{
    public class SessionDataProvider : ISessionDataProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionDataProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // for JWT
        //public string UserId => _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value ?? string.Empty;

        // for session data
        public string UserId
        {
            get => _httpContextAccessor.HttpContext.Items["UserId"].ToString() ?? string.Empty;
            set => _httpContextAccessor.HttpContext.Items["UserId"] = value;
        }
    }
}
