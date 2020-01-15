using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.WebUI.Model
{
    [Table("VAspNetUserRoles")]
    public partial class IUserRoles
    {
        [Key]
        public string UserId { get; set; }
        public string RoleId { get; set; }

        [JsonIgnore]
        public IRoles Role { get; set; }
        [JsonIgnore]
        public IUsers User { get; set; }
    }
}
