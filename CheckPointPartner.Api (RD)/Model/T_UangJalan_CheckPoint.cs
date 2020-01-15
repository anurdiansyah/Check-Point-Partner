using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("VT_UangJalan_CheckPoint")]
    public class T_UangJalan_CheckPoint
    {
        public int Id { get; set; }
        public int UangJalanId { get; set; }
        public int RumahMakanId { get; set; }
        public bool AbsenSopir { get; set; }
        public bool AbsenKenek { get; set; }
        public System.DateTime IOn { get; set; }
        public string IBy { get; set; }
        public System.DateTime UOn { get; set; }
        public string UBy { get; set; }     
    }
}
