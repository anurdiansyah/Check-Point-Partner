using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("VAspNetRoleClaims")]
    public partial class IRoleClaims
    {
        [Key]
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        [JsonIgnore]
        public IRoles Role { get; set; }
    }
}
