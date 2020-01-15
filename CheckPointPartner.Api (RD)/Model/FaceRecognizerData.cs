using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckPointPartner.Api_RD.Model
{
    [Table("FaceRecognizerData")]
    public class FaceRecognizerData
    {
        [NotMapped]
        public string Base64Thumbnails { get; set; }
        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FaceId { get; set; }
        public string CrewId { get; set; }
        public string TagNumber { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }

        public int Version { get; set; }
        public System.DateTime IOn { get; set; }
        public string IBy { get; set; }
        public System.DateTime UOn { get; set; }
        public string UBy { get; set; }
    }
}
