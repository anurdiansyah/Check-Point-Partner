using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("T_CheckPoint")]
    public class T_CheckPoint
    {
        public Int32 Id { get; set; }

        public Int32 AbsenMakanSupirId { get; set; }

        public decimal Total { get; set; }

        public Int32 PaymentStatus { get; set; }

    }
}
