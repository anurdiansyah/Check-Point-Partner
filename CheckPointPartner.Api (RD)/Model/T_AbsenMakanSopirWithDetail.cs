using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("VT_AbsenMakanSopirWithDetail")]
    public class T_AbsenMakanSopirWithDetail
    {
        [NotMapped]
        public string Base64Thumbnails { get; set; }

        public string NamaPengemudi { get; set; }
        public string TagPengemudi { get; set; }
        public string NamaKenek { get; set; }
        public string TagKenek { get; set; }

        public Int32 Id { get; set; }
        public Int32? PengemudiId { get; set; }
        public Int32? KenekId { get; set; }
        public Int32 RumahMakanId { get; set; }
        public Int32 UangJalanId { get; set; }
        public DateTime Trandate { get; set; }
        public string FotoId { get; set; }
        public decimal DefaultHarga { get; set; }

        public System.DateTime IOn { get; set; }
        public string IBy { get; set; }
        public System.DateTime UOn { get; set; }
        public string UBy { get; set; }


        public Int32 PaymentStatus { get; set; }
    }
}
