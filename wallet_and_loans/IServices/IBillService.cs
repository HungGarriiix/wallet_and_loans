using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.ItemDTO;

namespace wallet_and_loans_api.IServices
{
    public interface IBillService
    {
        IEnumerable<BillResponseDTO> GetBills();
        BillDetailsResponseDTO GetBill(int id);
        BillResponseDTO CreateBill(CreateBillDTO dto);
        AddItemToBillDTO AddItemToBill(int billId, BillItemDTO item);
        BillResponseDTO UpdateBill(int id, UpdateBillDTO dto);
    }
}
