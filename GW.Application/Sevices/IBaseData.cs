using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Http;
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
        public Task<string> PutFileAsync(IFormFile file);
        public SettingDto ConvertStringToSettings(string setting);

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

        #region PutFile
        public async Task<string> PutFileAsync(IFormFile file)
        {
            try
            {
                //string name = fota.File.FileName;
                string name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                //imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(fileName.FileName);
                var filePath = Path.Combine("C:\\", Constants.GW_FOTA_DIRECTORY, name);
                var directory = "C:\\" + Constants.GW_FOTA_DIRECTORY;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(fileStream);
                }
                return directory+"\\"+name;
            }catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region ConvertObjToString
        public SettingDto ConvertStringToSettings(string setting)
        {
            SettingDto settingDto = new();
            var all = setting.Split('#');
            var items = all.FirstOrDefault()?.Split('*');
            foreach (var item in items)
            {
                var code = item.Split('=');
                if (string.IsNullOrEmpty(code[1].Trim())) continue;
                switch (code[0].Trim())
                {
                    case "100":
                        if (Enum.TryParse<DeviceType>(code[1].Trim(), ignoreCase: true, out var type))
                        {
                            // success
                            settingDto.Type = type;
                        }
                        break;
                    case "101":
                        if (Enum.TryParse<ProductCategory>(code[1].Trim(), ignoreCase: true, out var cat))
                        {
                            // success
                            settingDto.ProductCategory = cat;
                        }
                        break;
                    case "102":
                        settingDto.BatchNumber = code[1].Trim();
                        break;
                    case "103":
                        settingDto.SerialNumber = code[1].Trim();
                        break;
                    case "104":
                        if (DateTime.TryParse(code[1].Trim(), out DateTime date))
                        {
                            settingDto.ProductionDate = date;
                        }
                        break;
                    case "105":
                        if (DateTime.TryParse(code[1].Trim(), out DateTime last))
                        {
                            settingDto.LastUpdate = last;
                        }
                        break;
                    case "106":
                        settingDto.HardwareVersion = code[1].Trim();
                        break;
                    case "107":
                        settingDto.MAC = code[1].Trim();
                        break;
                    case "108":
                        settingDto.IMEI = code[1].Trim();
                        break;
                    case "109":
                        settingDto.FkOwnerId = int.Parse(code[1].Trim());
                        break;
                    case "110":
                        settingDto.OwnerName = code[1].Trim();
                        break;
                    case "111":
                        settingDto.FkESPId = int.Parse(code[1].Trim());
                        break;
                    case "112":
                        settingDto.ESPVersion = code[1].Trim();
                        break;
                    case "113":
                        settingDto.FkSTMId = int.Parse(code[1].Trim());
                        break;
                    case "114":
                        settingDto.STMVersion = code[1].Trim();
                        break;
                    case "115":
                        settingDto.FkHoltekId = int.Parse(code[1].Trim());
                        break;
                    case "116":
                        settingDto.HoltekVersion = code[1].Trim();
                        break;
                }
            }
            return settingDto;
        }
        #endregion
    }
}
