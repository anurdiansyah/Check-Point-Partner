using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.WebUI.Model
{
    [Table("VAspNetUserClaims")]
    public partial class IUserClaims
    {
        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public IUsers User { get; set; }
    }
}
