using System.Net.Http;
using System.Threading.Tasks;

namespace AbsenSupir.WebApp.Services
{
    public interface IGlobalHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
