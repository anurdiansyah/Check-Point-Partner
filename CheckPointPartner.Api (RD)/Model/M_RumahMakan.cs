using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("VM_RumahMakan")]
    public partial class M_RumahMakan
    {
        [Key]
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerEmail { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
