using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.WebUi.Configuration
{
    public class Oidc
    {
        public string AuthUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public bool SaveToken { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }
}
