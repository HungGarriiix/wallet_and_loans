using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Services;

namespace wallet_and_loans_api
{
    public static class APIPreparer
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IWalletService, WalletService>();
        }
    }
}
