using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("VM_Kenek")]
    public class M_Kenek
    {
        public int Id { get; set; }
        public string KodeId { get; set; }
        public string Name { get; set; }
        public string NoHP { get; set; }
        public string Status { get; set; }
        public Guid GlobalId { get; set; }

        public System.DateTime IOn { get; set; }
        public string IBy { get; set; }
        public System.DateTime UOn { get; set; }
        public string UBy { get; set; }
    }
}
