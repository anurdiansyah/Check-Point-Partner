using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckPointPartner.WebUI
{
    [Table("AllowedCrewToCheckIn")]
    public partial class CheckIn
    {

        //Uang Jalan
        [Key]
        public Int64 RowNumber { get; set; }
        public Int32 IdUangJalan { get; set; }
        public string KodeUangJalan { get; set; }
        public Int32 IdUangJalanCheckPoint { get; set; }
        public string StatusUangJalan { get; set; }
        public Int32? PengemudiId { get; set; }
        public Int32? KenekId { get; set; }

        //Rumah Makan
        public Int32 IdRumahMakan { get; set; }
        public string NamaRumahMakan { get; set; }
        public string EmailRumahMakan { get; set; }

        //Crew to Check In
        //public Guid CrewGlobalId { get; set; }
        public string CrewPosition { get; set; }
        public string StatusCrew { get; set; }
        public bool CrewIsCheckedIn { get; set; }
        public string CrewTagNo { get; set; }
        public string CrewName { get; set; }
        public Guid CrewPhotoId { get; set; }
        public string Base64Photo { get; set; }
        

    }
}
