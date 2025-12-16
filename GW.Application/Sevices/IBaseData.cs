using GW.Core.Models.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Sevices
{
    public interface IBaseData
    {
        public string HashMaker(string plain);
        public string GenerateToken(Guid userId,string role);

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

    }
}
