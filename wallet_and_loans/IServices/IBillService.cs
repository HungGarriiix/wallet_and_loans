using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IServices
{
    public interface IBillService
    {
        IEnumerable<BillResponseDTO> GetBills();
        Bill GetBillByID(int id);
        BillResponseDTO CreateBill(CreateBillDTO dto);
    }
}
