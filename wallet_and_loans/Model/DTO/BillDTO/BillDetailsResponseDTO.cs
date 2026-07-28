using wallet_and_loans_api.Model.DTO.ItemDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class BillDetailsResponseDTO
    {
        public BillDetailsResponseDTO()
        {

        }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Wallet WalletUsedID { get; set; }
        public string Owner { get; set; }
        public float Total { get; set; }
        public List<BillItemResponseDTO> Items { get; set; }
    }
}
