using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.WebUI.Model
{
    [Table("VM_RumahMakan")]
    public class Partner
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerEmail { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
