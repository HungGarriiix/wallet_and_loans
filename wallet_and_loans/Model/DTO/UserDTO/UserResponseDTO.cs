using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Model.DTO.UserDTO
{
    public class UserResponseDTO
    {
        public UserResponseDTO(User user)
        {
            ID = user.ID;
            Username = user.Username;
            DisplayName = user.LoginProfiles.FirstOrDefault()?.ProfileName ?? string.Empty;
        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }
}