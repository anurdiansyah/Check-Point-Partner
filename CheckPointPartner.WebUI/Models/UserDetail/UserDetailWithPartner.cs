using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPointPartner.WebUI.Model
{
    [Table("V_UserDetail")]
    public class UserDetailWithPartner
    { 
        public Guid UserDetailId { get; set; }
        public string AspNetUserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string UserImage { get; set; }
        public Int32? PartnerId { get; set; }
        public string PartnerAddress { get; set; }
        public string PartnerImage { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

        public string PartnerName { get; set; }
        public string PartnerEmail { get; set; }
    }
}
