using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.WebUI.Model
{
    [Table("VAspNetUsers")]
    public partial class IUsers
    {
        public IUsers()
        {
            UserClaims = new HashSet<IUserClaims>();
            UserLogins = new HashSet<IUserLogins>();
            UserRoles = new HashSet<IUserRoles>();
            UserTokens = new HashSet<IUserTokens>();
            UserDetail = new UserDetailWithPartner();
        }
        
        [Key]
        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public ICollection<IUserClaims> UserClaims { get; set; }
        [JsonIgnore]
        public ICollection<IUserLogins> UserLogins { get; set; }
        [JsonIgnore]
        public ICollection<IUserRoles> UserRoles { get; set; }
        [JsonIgnore]
        public ICollection<IUserTokens> UserTokens { get; set; }

        [NotMapped]
        public UserDetailWithPartner UserDetail { get; set; }

    }
}
