using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsenSupir.WebApp.Configuration
{
    public class Notification
    {
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string MailBodyTemplateFileName { get; set; }
    }
}
