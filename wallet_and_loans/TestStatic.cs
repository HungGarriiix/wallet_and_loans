using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api
{
    public static class TestStatic
    {
        public static List<Bill> Bills { get; set; } = new List<Bill>();

        // Test with multiple users
        public static List<User> Users { get; set; } = new List<User>();

        public static int UserCounter { get; set; } = 0;
        public static int WalletCounter { get; set; } = 0;
        public static int BillCounter { get; set; } = 0;
    }
}
