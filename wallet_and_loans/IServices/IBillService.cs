using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.ItemDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IServices
{
    public interface IBillService
    {
        IEnumerable<BillResponseDTO> GetBills();
        BillResponseDTO GetBill(int id);
        BillResponseDTO CreateBill(CreateBillDTO dto);
        AddItemToBillDTO AddItemToBill(int billId, BillItemDTO item);
    }
}
