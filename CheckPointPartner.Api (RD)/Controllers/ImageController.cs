using Microsoft.AspNetCore.Mvc;
using RD.Lib;
using System;
using System.IO;

namespace CheckPointPartner.Api_RD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : RDController
    {
        string sImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\");
        string sDefaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\default\\");

        string sCrewPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\crew\\");
        [HttpGet]
        [Route("Crew")]
        public IActionResult GetCrew(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sCrewPhotoPath + sImageName))
            {
                return PhysicalFile(sCrewPhotoPath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_photo.png", "image/png");
            }
        }

        [Route("CheckInImage")]
        public IActionResult GetCheckInImage(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sImagePath + sImageName))
            {
                return PhysicalFile(sImagePath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_photo.png", "image/png");
            }
        }
    }
}