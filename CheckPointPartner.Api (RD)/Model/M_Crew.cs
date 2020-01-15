using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckPointPartner.Api_RD.Model
{

    public enum CrewType
    {
        [Description("PENGEMUDI")]
        PENGEMUDI = 1,  
        [Description("KENEK")]
        KENEK,
    }

    public enum CrewStatus
    { 
        [Description("NONAKTIF")]
        NONAKTIF = 1,
        [Description("AKTIF")]
        AKTIF = 2,
        [Description("SUSPEND")]
        SUSPEND = 3,
        [Description("RESIGN")]
        RESIGN = 4,
        [Description("BLACKLIST")]
        BLACKLIST = 5,
    }

    [Table("VM_Crew")]
    public class M_Crew
    {
        [NotMapped]
        public Int32? TimTerId { get; set; }
        [NotMapped]
        public bool? HaveFaceRecognizerData { get; set; }
        [NotMapped]
        public string FaceRecognizerFileName { get; set; }
        [NotMapped]
        public Int32 FaceRecognizerDataVersion { get; set; }

        [NotMapped]
        public string KodeUangJalan { get; set; }
        [NotMapped]
        public bool? IsAlreadyCheckIn { get; set; }
        [NotMapped]
        public Int32 IdUangJalan { get; set; }
        [NotMapped]
        public Int32 IdUangJalanCheckPoint { get; set; }
        [NotMapped]
        public string Base64Photo { get; set; }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string KtpNo { get; set; }
        public string KtpName { get; set; }
        public string KtpAddress { get; set; }
        public string KtpVillage { get; set; }
        public string KtpSubdistrict { get; set; }
        public string KtpCity { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public string ResignReason { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
        public string TagNo { get; set; }
        public Guid PhotoId { get; set; }
        public DateTime InputOn { get; set; }
        public string InputBy { get; set; }
        public DateTime LastUpdateOn { get; set; }
        public string LastUpdateBy { get; set; }
        public Guid CompanyId { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int? StatusSopir { get; set; }
    }
}
