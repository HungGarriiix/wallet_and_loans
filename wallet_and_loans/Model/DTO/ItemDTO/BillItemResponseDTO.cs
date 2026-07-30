namespace wallet_and_loans_api.Model.DTO.ItemDTO
{
    public class BillItemResponseDTO
    {
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public float? SinglePrice { get; set; }
        public float? TotalPrice { get; set; }
    }
}
