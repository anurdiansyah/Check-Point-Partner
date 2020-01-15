using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AbsenSupir.WebApp.Data;
using CheckPointPartner.Api_RD.Model;
using CheckPointPartner.Api_RD.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using RD.Lib;

namespace AbsenSupir.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public AuthController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
                              IConfiguration configuration, IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
            _configuration = configuration;
            _dbContext = context;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                //model.Password = RDEncryption.DecodeAndDecrypt(model.Password);
                RDToken _token = await _userService.GetTokenAsync(model.Email, model.Password);
                if (_token == null) return Unauthorized();

                bool bIsRestaurant = _dbContext.M_RumahMakans.Any(o => o.PartnerEmail == model.Email);
                M_RumahMakan oMyRestaurant = bIsRestaurant ? _dbContext.M_RumahMakans.Where(o => o.PartnerEmail == model.Email).FirstOrDefault() : new M_RumahMakan();
                return Ok(new
                {
                    id = oMyRestaurant.PartnerId,
                    oMyRestaurant.PartnerEmail,
                    oMyRestaurant.PartnerName,
                    token = _token.Token,
                    expiration = _token.ExpiredDate.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"),
                });

            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        ErrorMessage = ex.ToString()
                    });
            }
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] LoginViewModel model)
        {
            try
            {
                bool IsSuccess = await _userService.ChangePasswordAsync(model.Email, model.Password, model.NewPassword);
                if (!IsSuccess) return BadRequest(new { ErrorMessage = "Failed to change password" });
                
                return Ok(new { IsSuccess });

            }
            catch (Exception ex)
            {
                return BadRequest( new { ErrorMessage = ex.ToString() });
            }
        }

    }
}
