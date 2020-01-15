using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsenSupir.WebApp.Configuration
{
    public class Smtp
    {
        public string Host { get; set; }
        public Int32 Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
    }
}
