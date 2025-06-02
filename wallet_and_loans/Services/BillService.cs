using System.Xml;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class BillService: IBillService
    {
        private readonly IBillBLO _billBLO;

        public BillService(IBillBLO billBLO)
        {
            _billBLO = billBLO;
        }

        public IEnumerable<BillResponseDTO> GetBills()
        {
            IEnumerable<Bill> bills = _billBLO.GetBills();
            return BundleBillsIntoList(bills);
        }

        public BillResponseDTO GetBill(int id)
        {
            Bill bill = _billBLO.GetBillByID(id);
            return new BillResponseDTO(bill);
        }

        public BillResponseDTO CreateBill(CreateBillDTO dto)
        {
            Bill bill = _billBLO.CreateBill(dto);
            return new BillResponseDTO(bill);
        }

        private List<BillResponseDTO> BundleBillsIntoList(IEnumerable<Bill> bills)
        {
            List<BillResponseDTO> billsList = new List<BillResponseDTO>();
            foreach(Bill bill in bills)
                billsList.Add(new BillResponseDTO(bill));
            return billsList;
        }
    }
}
