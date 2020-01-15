using CheckPointPartner.WebUI.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckPointPartner.WebUI.Services
{
    public interface IAppUser
    {
        IUsers CurrentUser();
    }
}
