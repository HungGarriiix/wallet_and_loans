using wallet_and_loans_api.Model.DTO.ItemDTO;

namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class AddItemToBillDTO
    {
        public BillItemResponseDTO Item { get; set; }
        public double ExpectedBalance {  get; set; }
        public List<BillItemResponseDTO> BillItems { get; set; }
    }
}
