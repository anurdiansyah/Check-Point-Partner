using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("VAspNetRoles")]
    public partial class IRoles
    {
        public IRoles()
        {
            RoleClaims = new HashSet<IRoleClaims>();
            UserRoles = new HashSet<IUserRoles>();
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        [JsonIgnore]
        public ICollection<IRoleClaims> RoleClaims { get; set; }
        [JsonIgnore]
        public ICollection<IUserRoles> UserRoles { get; set; }
    }
}
