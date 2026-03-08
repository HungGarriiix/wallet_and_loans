using wallet_and_loans_api.Model.DTO.AuthDTO;

namespace wallet_and_loans_api.IServices
{
    public interface IAuthService
    {
        LoginResponseDTO Login(LoginRequestDTO dto);
    }
}
