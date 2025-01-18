using System.Xml;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class BillService: IBillService
    {
        private readonly IWalletService _walletService;

        public BillService(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public List<Bill> Bills { get; set; } = new List<Bill>();

        public IEnumerable<Bill> GetBills()
        {
            return Bills;
        }

        public Bill GetBillByID(int id)
        {
            return Bills[id];
        }

        public void CreateBill(CreateBillDTO dto, out Bill result)
        {
            Wallet wallet = _walletService.GetWalletByID(dto.WalletUsedID);
            Bill bill = new Bill(
                Bills.Count + 1,
                dto.DateCreated,
                dto.Description,
                wallet,
                TestStatic.UserTest);
            Bills.Add(bill);
            result = bill;
        }
    }
}
