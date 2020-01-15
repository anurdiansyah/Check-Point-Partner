using AbsenSupir.WebApp.Configuration;
using CheckPointPartner.Api_RD.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CheckPointPartner.Api_RD.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IOptions<AppSettings> appSettings, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _appSettings = configuration.Get<AppSettings>();
        }

        public async Task<RDToken> GetTokenAsync(string username, string password)
        {
            RDToken rdToken = new RDToken();

            ApplicationUser _user = await _userManager.FindByNameAsync(username);
            var userRoles = await _userManager.GetRolesAsync(_user);
            string roles = string.Empty;
            for (int i = 0; i < userRoles.Count; i++)
            {
                roles += userRoles[i];
                if (i != userRoles.Count - 1) roles += ", ";
            }

            if (_user != null && await _userManager.CheckPasswordAsync(_user, RDEncryption.DecodeAndDecrypt(password)))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.SigningKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                        new Claim(ClaimTypes.Name, _user.UserName.ToString()),
                        new Claim(ClaimTypes.Email, _user.Email),
                        new Claim(ClaimTypes.Role, roles)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(_appSettings.Jwt.ExpiryInMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var _token = tokenHandler.CreateToken(tokenDescriptor);

                rdToken.RefId = _user.Id;
                rdToken.Token = tokenHandler.WriteToken(_token);
                rdToken.ExpiredDate = _token.ValidTo;


                return rdToken;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ChangePasswordAsync(string username, string password, string newpassword)
        {
            bool bIsSuccess = false;

            ApplicationUser _user = await _userManager.FindByNameAsync(username);
            if (_user != null && await _userManager.CheckPasswordAsync(_user, RDEncryption.DecodeAndDecrypt(password)))
            {
                _user.PasswordHash = _userManager.PasswordHasher.HashPassword(_user, newpassword);
                var result = await _userManager.UpdateAsync(_user);
                bIsSuccess = result.Succeeded;
            }

            return bIsSuccess;
        }
    }
}
