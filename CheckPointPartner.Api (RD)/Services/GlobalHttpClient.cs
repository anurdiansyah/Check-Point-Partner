using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace AbsenSupir.WebApp.Services
{
    public class GlobalHttpClient : IGlobalHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient = new HttpClient();
        public IConfiguration Configuration { get; }

        public GlobalHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

        [Obsolete]
        public async Task<HttpClient> GetClient()
        {
            //discover endpoints from metadata
            DiscoveryClient discoveryClient = new DiscoveryClient(Configuration["WebApi:Is4Url"]);
            discoveryClient.Policy.RequireHttps = false;
            var discoveryResponse = await discoveryClient.GetAsync();

            if (discoveryResponse.IsError)
            {
                Console.WriteLine(discoveryResponse.Error);
            }

            // request token
            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "absenwebapp", "b0e456ab6e4117934f747dd07093055b");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("globaldataapi");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            _httpClient.SetBearerToken(tokenResponse.AccessToken);
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _httpClient.BaseAddress = new Uri(Configuration["WebApi:GlobalApiUrl"]);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return this._httpClient;
        }
    }
}
