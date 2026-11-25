namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class BillListResponseDTO
    {
        // set when there is something the user should be told, e.g. the list is empty
        public string? Message { get; set; }
        public IEnumerable<BillResponseDTO> Bills { get; set; } = new List<BillResponseDTO>();
    }
}
