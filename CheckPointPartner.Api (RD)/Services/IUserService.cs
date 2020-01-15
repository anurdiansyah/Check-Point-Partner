using RD.Lib;
using System.Threading.Tasks;

namespace CheckPointPartner.Api_RD.Services
{
    public interface IUserService
    {
        Task<RDToken> GetTokenAsync(string username, string password);
        Task<bool> ChangePasswordAsync(string username, string password, string newpassword);
    }
}
