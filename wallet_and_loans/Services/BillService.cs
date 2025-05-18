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

        public IEnumerable<BillResponseDTO> GetBills()
        {
            return BundleBillsIntoList(TestStatic.Bills);
        }

        public Bill GetBillByID(int id)
        {
            return TestStatic.Bills[id];
        }

        public BillResponseDTO CreateBill(CreateBillDTO dto)
        {
            Wallet wallet = _walletService.GetWalletByID(dto.WalletUsedID);
            Bill bill = new Bill(
                TestStatic.Bills.Count + 1,
                dto.DateCreated,
                dto.Description,
                wallet,
                TestStatic.UserTest);
            TestStatic.Bills.Add(bill);
            return new BillResponseDTO(bill);
        }

        private List<BillResponseDTO> BundleBillsIntoList(List<Bill> bills)
        {
            List<BillResponseDTO> billsList = new List<BillResponseDTO>();
            foreach(Bill bill in bills)
                billsList.Add(new BillResponseDTO(bill));
            return billsList;
        }
    }
}
