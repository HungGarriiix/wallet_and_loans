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
            User user = _userBLO.GetCurrentUser();
            IEnumerable<Bill> bills = _billBLO.GetBills(user);
            return BundleBillsIntoList(bills);
        }

        public BillDetailsResponseDTO GetBill(int id)
        {
            Bill bill = _billBLO.GetBillByID(id, _userBLO.GetCurrentUser());
            BillDetailsResponseDTO billDetails = new BillDetailsResponseDTO
            {
                ID = bill.ID,
                Date = bill.Date,
                Description = bill.Description,
                WalletUsedID = bill.WalletUsed,
                Owner = bill.Owner.Username,
                Total = bill.Total,
                Type = bill.Type,
                TypeName = bill.Type.ToString(),
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
            Bill bill = _billBLO.CreateBill(dto, _userBLO.GetCurrentUser());
            return new BillResponseDTO(bill);
        }

        public AddItemToBillDTO AddItemToBill(int billId, BillItemDTO item)
        {
            Bill bill = _billBLO.GetBillByID(billId, _userBLO.GetCurrentUser());
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

        public BillResponseDTO UpdateBill(int id, UpdateBillDTO dto)
        {
            Bill result = new Bill();

            _billBLO.UpdateBill(id, dto, _userBLO.GetCurrentUser(), ref result);
            return new BillResponseDTO(result);
        }

        public BillDetailsResponseDTO DeleteAllBillItem(int id)
        {
            Bill bill = _billBLO.DeleteAllBillItems(id, _userBLO.GetCurrentUser());
            BillDetailsResponseDTO billDetail = new BillDetailsResponseDTO()
            {
                ID = bill.ID,
                Date = bill.Date,
                Description = bill.Description,
                WalletUsedID = bill.WalletUsed,
                Owner = bill.Owner.Username,
                Total = bill.Total,
                Type = bill.Type,
                TypeName = bill.Type.ToString(),
                Items = null //for now
            };

            return billDetail;
        }

        public AddBalanceResponseDTO AddBalance(AddBalanceDTO dto)
        {
            float expectedBalance = 0f;
            Bill bill = _billBLO.AddBalance(dto, _userBLO.GetCurrentUser(), ref expectedBalance);

            return new AddBalanceResponseDTO
            {
                Bill = new BillDetailsResponseDTO
                {
                    ID = bill.ID,
                    Date = bill.Date,
                    Description = bill.Description,
                    WalletUsedID = bill.WalletUsed,
                    Owner = bill.Owner.Username,
                    Total = bill.Total,
                    Type = bill.Type,
                    TypeName = bill.Type.ToString()
                },
                ExpectedBalance = expectedBalance
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
