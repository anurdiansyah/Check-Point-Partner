using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.WebUI.Model
{
    [Table("VAspNetUserLogins")]
    public partial class IUserLogins
    {
        public string LoginProvider { get; set; }
        [Key]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public IUsers User { get; set; }
    }
}
