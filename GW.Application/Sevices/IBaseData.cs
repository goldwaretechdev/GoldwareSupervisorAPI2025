using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GW.Application.Sevices
{
    public interface IBaseData
    {
        public string HashMaker(string plain);
        public string GenerateToken(Guid userId,string role);
        public Guid GetUserId(string token);
        public string GetUserRole(string token);
    }


    public class BaseData:IBaseData
    {
        private readonly IConfiguration _configuration;


        public BaseData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region HashMaker
        public string HashMaker(string plain)
        {
            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(plain, 13);
            return hash;
        }
        #endregion

        #region GenerateToken
        public string GenerateToken(Guid userId, string role)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // SignalR default lookup
                 new Claim("Identifier",userId.ToString()),               
                  new Claim(ClaimTypes.Role,role)
            };

            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region GetUserId
        public Guid GetUserId(string token)
        {
            var strToken = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.ReadJwtToken(strToken);
            var tokenS = jwtSecurityToken as JwtSecurityToken;
            return Guid.Parse(tokenS.Claims.FirstOrDefault(claim => claim.Type == "Identifier").Value);
        }
        #endregion

        #region GetUserRole
        public string GetUserRole(string token)
        {
            var strToken = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.ReadJwtToken(strToken);
            var tokenS = jwtSecurityToken as JwtSecurityToken;
            return tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
        }
        #endregion
    }
}
