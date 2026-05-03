using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.BLO
{
    public class AuthBLO : IAuthBLO
    {
        private readonly IConfiguration _config;
        private readonly IUserBLO _userBLO;

        public AuthBLO(IConfiguration config, IUserBLO userBLO)
        {
            _config = config;
            _userBLO = userBLO;
        }

        public string GenerateToken(string userId)
        {
            var jwtConfig = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userId),
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtConfig["ExpiresMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User GetUserInfo(string userId, string platform)
        {
            if (!string.IsNullOrEmpty(platform))
            {
                //int platformInt = int.Parse(platform);
                return _userBLO.GetUserByContact(userId, (LoginPlatformEnum)Enum.Parse(typeof(LoginPlatformEnum), platform));
            }
            return _userBLO.GetUserById(int.Parse(userId));
        }
    }
}
