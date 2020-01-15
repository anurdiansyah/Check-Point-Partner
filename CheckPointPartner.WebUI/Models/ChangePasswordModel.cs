using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPointPartner.WebUI.Models
{
    public class ChangePasswordModel
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
