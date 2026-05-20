using wallet_and_loans_api.Model.DTO.UserDTO;

namespace wallet_and_loans_api.IServices
{
    public interface IUserService
    {
        bool CheckUserAvailability();
        UserResponseDTO GetUser();
    }
}
