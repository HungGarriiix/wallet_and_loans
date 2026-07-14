using System.Xml;
using wallet_and_loans_api.Common;
using wallet_and_loans_api.Common.Session;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.ItemDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class BillService: BaseService, IBillService
    {
        private readonly IBillBLO _billBLO;
        private readonly IUserBLO _userBLO;

        public BillService(IBillBLO billBLO, IUserBLO userBLO, ISessionDataProvider sessionDataProvider)
            : base(sessionDataProvider)
        {
            _billBLO = billBLO;
            _userBLO = userBLO;
        }

        public IEnumerable<BillResponseDTO> GetBills()
        {
            string userId = _sessionDataProvider.UserId;
            if (string.IsNullOrEmpty(userId)) throw new Exception("Unauthorized: User session not found.");
            
            User user = _userBLO.GetUserById(Convert.ToInt32(userId));
            if (user == null) throw new Exception("User not found.");

            IEnumerable<Bill> bills = _billBLO.GetBills(user);
            return BundleBillsIntoList(bills);
        }

        public BillDetailsResponseDTO GetBill(int id)
        {
            Bill bill = _billBLO.GetBillByID(id);
            BillDetailsResponseDTO billDetails = new BillDetailsResponseDTO
            {
                ID = bill.ID,
                Date = bill.Date,
                Description = bill.Description,
                WalletUsedID = bill.WalletUsed,
                Owner = bill.Owner.Username,
                Items = bill.Items.Select(item => new BillItemResponseDTO()
                    {
                        Name = item.Name,
                        Quantity = item.Quantity,
                        SinglePrice = item.SinglePrice,
                        TotalPrice = item.TotalPrice,
                    }
                ).ToList()
            };
            return billDetails;
        }

        public BillResponseDTO CreateBill(CreateBillDTO dto)
        {
            string userId = _sessionDataProvider.UserId;
            if (string.IsNullOrEmpty(userId)) throw new Exception("Unauthorized: User session not found.");

            User user = _userBLO.GetUserById(Convert.ToInt32(userId));
            if (user == null) throw new Exception("User not found.");

            Bill bill = _billBLO.CreateBill(dto, user);
            return new BillResponseDTO(bill);
        }

        public AddItemToBillDTO AddItemToBill(int billId, BillItemDTO item)
        {
            Bill bill = _billBLO.GetBillByID(billId);
            float expectedBalance = 0f;
            BillItem billItem = new BillItem(item.Name, item.Quantity, item.TotalPrice);

            _billBLO.AddItemToBill(bill, billItem, ref expectedBalance);

            return new AddItemToBillDTO
            {
                Item = new BillItemResponseDTO
                {
                    Name = billItem.Name,
                    Quantity = billItem.Quantity,
                    SinglePrice = billItem.SinglePrice,
                    TotalPrice = billItem.TotalPrice
                },
                ExpectedBalance = expectedBalance,
                BillItems = BundleBillItemsIntoList(bill.Items)
            };
        }

        private List<BillResponseDTO> BundleBillsIntoList(IEnumerable<Bill> bills)
        {
            List<BillResponseDTO> billsList = new List<BillResponseDTO>();
            foreach(Bill bill in bills)
                billsList.Add(new BillResponseDTO(bill));
            return billsList;
        }

        private List<BillItemResponseDTO> BundleBillItemsIntoList(IEnumerable<BillItem> billItems)
        {
            List<BillItemResponseDTO> billItemList = new List<BillItemResponseDTO>();
            foreach (BillItem billItem in billItems)
                billItemList.Add(new BillItemResponseDTO
                {
                    Name = billItem.Name,
                    Quantity = billItem.Quantity,
                    SinglePrice = billItem.SinglePrice,
                    TotalPrice = billItem.TotalPrice
                });
            return billItemList;
        }
    }
}
