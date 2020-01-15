using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AbsenSupir.Api.Data;
using AbsenSupir.WebApp.Configuration;
using AbsenSupir.WebApp.Data;
using CheckPointPartner.Api_RD.Model;
using AbsenSupir.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using CheckPointPartner.Api_RD.Data;

namespace AbsenSupir.WebApp.Controllers
{
    [Route("[controller]")]
    //[Authorize(Roles = "Merchant")]
    public class CheckInController : RDController
    {

        private SmtpClient _smtpClient;
        private readonly ApplicationDbContext _dbContext;
        private readonly UsersContext _usersContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGlobalHttpClient _globalHttpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        private string sUserEmail = string.Empty;
        string sCrewPhotoThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\crew\\thumbs");
        string sTrackDatFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\file\\track");
        string sCheckInImagePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\");
        string sDefaultPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\default\\");
        string sMailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\template\\email\\");

        public CheckInController(ApplicationDbContext context, UsersContext userContext, UserManager<ApplicationUser> userManager,
                                 IGlobalHttpClient globalHttpClient, IHttpContextAccessor httpContextAccessor,
                                 SmtpClient smtpClient, IConfiguration configuration)
        {
            _dbContext = context;
            _usersContext = userContext;
            _userManager = userManager;
            _globalHttpClient = globalHttpClient;
            _httpContextAccessor = httpContextAccessor;
            _smtpClient = smtpClient;
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email) != null) sUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            _appSettings = configuration.Get<AppSettings>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("DoCheckIn")]
        public async Task<IActionResult> CheckInAsync([FromBody] DoCheckInModel p_DoCheckInModel)
        {
            T_AbsenMakanSopir oAbsenMakan = new T_AbsenMakanSopir();
            bool bIsSuccess = false;

            try
            {
                oAbsenMakan.PengemudiId = _dbContext.M_Pengemudis.Where(o => o.GlobalId == p_DoCheckInModel.GlobalId).FirstOrDefault()?.Id;
                oAbsenMakan.KenekId = _dbContext.M_Keneks.Where(o => o.GlobalId == p_DoCheckInModel.GlobalId).FirstOrDefault()?.Id;
                oAbsenMakan.FotoId = Guid.NewGuid().ToString();
                oAbsenMakan.UangJalanId = p_DoCheckInModel.UangJalanId;
                oAbsenMakan.RumahMakanId = p_DoCheckInModel.RumahMakanId;
                oAbsenMakan.Trandate = DateTime.UtcNow.ToLocalTime();
                oAbsenMakan.IBy = sUserEmail;
                oAbsenMakan.IOn = DateTime.Now.ToLocalTime();
                oAbsenMakan.UBy = sUserEmail;
                oAbsenMakan.UOn = DateTime.Now.ToLocalTime();
                oAbsenMakan.DefaultHarga = 20000;

                if (_dbContext.T_AbsenMakanSopirs.Where(o => o.UangJalanId == p_DoCheckInModel.UangJalanId && o.PengemudiId == oAbsenMakan.PengemudiId && o.KenekId == oAbsenMakan.KenekId).Count() == 0)
                {
                    String sFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                    RDImageHelper.SaveImageFromBase64(p_DoCheckInModel.Base64Photos, sFilePath, oAbsenMakan.FotoId + ".jpeg");
                    _dbContext.T_AbsenMakanSopirs.Add(oAbsenMakan);
                    if (_dbContext.SaveChanges() > 0)
                    {
                        T_CheckPoint oCheckPoints = new T_CheckPoint();
                        oCheckPoints.AbsenMakanSupirId = oAbsenMakan.Id;
                        oCheckPoints.Total = oAbsenMakan.DefaultHarga;
                        oCheckPoints.PaymentStatus = (int)Enumeration.PaidStatus.UnPaid;
                        _dbContext.CheckPoints.Add(oCheckPoints);
                        if (_dbContext.SaveChanges() > 0)
                        {
                            T_UangJalan_CheckPoint oCheckPoint = _dbContext.T_UangJalan_CheckPoints.Where(o => o.Id == p_DoCheckInModel.UangJalanCheckPointId).FirstOrDefault();
                            if (oCheckPoint != null)
                            {
                                if (p_DoCheckInModel.CrewTypeId == (int)CrewType.PENGEMUDI)
                                {
                                    oCheckPoint.AbsenSopir = true;
                                }
                                else
                                {
                                    oCheckPoint.AbsenKenek = true;
                                }
                                oCheckPoint.UBy = sUserEmail;
                                oCheckPoint.UOn = DateTime.Now.ToLocalTime();
                            }
                            _dbContext.SaveChanges();
                        }
                    };

                    p_DoCheckInModel.ResponseMessage = p_DoCheckInModel.CrewTypeId == (int)CrewType.PENGEMUDI
                                                        ? "\"Check In Pengemudi Berhasil\""
                                                        : "\"Check In Kenek Berhasil\"";
                }
                else
                {
                    p_DoCheckInModel.ResponseMessage = p_DoCheckInModel.CrewTypeId == (int)CrewType.PENGEMUDI
                                                        ? "\"Checkin Gagal\nPengemudi sudah melakukan Check In sebelumnya\""
                                                        : "\"Checkin Gagal\nKenek sudah melakukan Check In sebelumnya\"";
                }

                bIsSuccess = true;
                return Json(p_DoCheckInModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (bIsSuccess)
                {
                    await sendNotificationAsync(oAbsenMakan);
                }
            }
        }

        [HttpGet]
        [Route("GetFaceData")]
        public async Task<IActionResult> GetFaceDataAsync(string p_sCrewTagNo)
        {
            FaceRecognizerData oFaceRecognizerData;
            try
            {
                oFaceRecognizerData = _dbContext.FaceRecognizerDatas.Where(o => o.TagNumber == p_sCrewTagNo).FirstOrDefault();

                if (oFaceRecognizerData == null)
                {
                    return BadRequest("Data rekaman wajah untuk crew dengan nomor tag " + p_sCrewTagNo + " tidak dapat ditemukan");
                }
                else
                {
                    var memory = new MemoryStream();
                    var datFilePath = Path.Combine(sTrackDatFilePath, oFaceRecognizerData.FileName);
                    using (var stream = new FileStream(datFilePath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return File(memory, "application/octet-stream", Path.GetFileName(datFilePath));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                oFaceRecognizerData = null;
            }
        }

        [HttpGet]
        [Route("MyCheckInList")]
        public IActionResult MyCheckInList(string p_sDateFrom, string p_sDateTo)
        {
            try
            {
                DateTime dtFrom = new DateTime(Convert.ToInt32(p_sDateFrom.Split('-')[2]), Convert.ToInt32(p_sDateFrom.Split('-')[1]), Convert.ToInt32(p_sDateFrom.Split('-')[0]));
                DateTime dtTo = new DateTime(Convert.ToInt32(p_sDateTo.Split('-')[2]), Convert.ToInt32(p_sDateTo.Split('-')[1]), Convert.ToInt32(p_sDateTo.Split('-')[0]));

                var lstAbsenMakanSupirWithDetail = _dbContext.T_AbsenMakanSopirWithDetails.Where(o => o.IBy == sUserEmail
                                                                      && (o.IOn > Convert.ToDateTime(dtFrom) || o.IOn == Convert.ToDateTime(dtFrom))
                                                                      && (o.IOn < Convert.ToDateTime(dtTo).AddDays(1) || o.IOn == Convert.ToDateTime(dtTo).AddDays(1)));
                foreach (var item in lstAbsenMakanSupirWithDetail)
                {
                    String sImageName = item.FotoId + ".jpeg";
                    if (System.IO.File.Exists(sCheckInImagePhotoPath + sImageName))
                        item.Base64Thumbnails = Convert.ToBase64String(RDImageHelper.ImageToByteArray(sCheckInImagePhotoPath + sImageName));
                    else
                        item.Base64Thumbnails = Convert.ToBase64String(RDImageHelper.ImageToByteArray(sDefaultPhotoPath + @"\\no_photo.png"));

                    item.Trandate = item.Trandate.ToUniversalTime();
                    item.IOn = item.IOn.ToUniversalTime();
                    item.UOn = item.UOn.ToUniversalTime();
                }
                return Json(lstAbsenMakanSupirWithDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new {
                        userMessage = "Oops...",
                        internalMessage = ex.ToString(),
                        errorCode = "200"
                    });
            }
            finally
            {
            }
        }

        [HttpGet]
        [Route("CrewDetail")]
        public async Task<IActionResult> GetCrewDetail(string p_sCrewTagNo)
        {
            HttpClient httpClient = await _globalHttpClient.GetClient();
            try
            {
                if (_dbContext.M_Crews.Any(o => o.TagNo == p_sCrewTagNo))
                {
                    M_Crew oCrew = _dbContext.M_Crews.Where(o => o.TagNo == p_sCrewTagNo).FirstOrDefault();
                    AllowedCrewToCheckIn allowedCrewToCheckIn = _dbContext.AllowedCrewToCheckIn.Where(o => o.EmailRumahMakan == sUserEmail && o.CrewTagNo == p_sCrewTagNo).FirstOrDefault();
                    
                    byte[] myByte = await httpClient.GetByteArrayAsync("http://system.timurterang.com:5998/api/image/" + oCrew.PhotoId);
                    myByte = myByte == null ? await httpClient.GetByteArrayAsync("http://system.timurterang.com:5998/api/image/" + "00000000-0000-0000-0000-000000000000") : myByte;

                    oCrew.TimTerId = allowedCrewToCheckIn != null
                                                ? oCrew.Type == CrewType.PENGEMUDI.ToString()
                                                    ? allowedCrewToCheckIn.PengemudiId
                                                    : allowedCrewToCheckIn.KenekId
                                                : -99;
                    oCrew.IdUangJalan = allowedCrewToCheckIn == null ? -99 : allowedCrewToCheckIn.IdUangJalan;
                    oCrew.IdUangJalanCheckPoint = allowedCrewToCheckIn == null ? -99 : allowedCrewToCheckIn.IdUangJalanCheckPoint;
                    oCrew.Base64Photo = Convert.ToBase64String(myByte);
                    oCrew.KodeUangJalan = allowedCrewToCheckIn?.KodeUangJalan;
                    oCrew.IsAlreadyCheckIn = allowedCrewToCheckIn?.CrewIsCheckedIn;

                    switch (oCrew.Status)
                    {
                        case "NONAKTIF":
                            oCrew.Status = Convert.ToString((int)CrewStatus.NONAKTIF);
                            break;
                        case "AKTIF":
                            oCrew.Status = Convert.ToString((int)CrewStatus.AKTIF);
                            break;
                        case "SUSPEND":
                            oCrew.Status = Convert.ToString((int)CrewStatus.SUSPEND);
                            break;
                        case "RESIGN":
                            oCrew.Status = Convert.ToString((int)CrewStatus.RESIGN);
                            break;
                        case "BLACKLIST":
                            oCrew.Status = Convert.ToString((int)CrewStatus.BLACKLIST);
                            break;
                        default:
                            break;

                    }

                    switch (oCrew.Type)
                    {
                        case "SOPIR":
                            oCrew.Type = Convert.ToString((int)CrewType.PENGEMUDI);
                            break;
                        case "KENEK":
                            oCrew.Type = Convert.ToString((int)CrewType.KENEK);
                            break;

                    }

                    oCrew.BirthDate = oCrew.BirthDate.ToUniversalTime();
                    if (oCrew.JoinDate != null) oCrew.JoinDate = Convert.ToDateTime(oCrew.JoinDate).ToUniversalTime();
                    if (oCrew.ResignDate != null) oCrew.ResignDate = Convert.ToDateTime(oCrew.ResignDate).ToUniversalTime();
                    oCrew.InputOn = oCrew.InputOn.ToUniversalTime();
                    oCrew.LastUpdateOn = oCrew.LastUpdateOn.ToUniversalTime();

                    return Json(oCrew);
                }

                return BadRequest("Data " + p_sCrewTagNo + " not Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                httpClient = null;
            }
        }

        private async Task sendNotificationAsync(T_AbsenMakanSopir t_AbsenMakanSopir)
        {
            string sSubject = "GiTTerns Mail Sender.";
            string sBody = "This message has no body.";

            try
            {
                foreach (IUserRoles userRoles in _usersContext.iUserRoles.Where(o => o.RoleId == _usersContext.iRoles.Where(p => p.Name == "Administrator").FirstOrDefault().Id))
                {
                    MailMessage mail = new MailMessage();
                    sSubject = _appSettings.Email.Notification.Subject;

                    mail.To.Add(_usersContext.iUsers.Where(o => o.Id == userRoles.UserId).FirstOrDefault().Email);
                    mail.From = new MailAddress(_appSettings.Email.Notification.EmailFrom);
                    mail.Subject = sSubject;
                    if (Directory.Exists(sMailTemplatePath))
                    {
                        StringBuilder sbBody = new StringBuilder();
                        sbBody.Append(RDFile.ReadStringFromTxtFile(Path.Combine(sMailTemplatePath, _appSettings.Email.Notification.MailBodyTemplateFileName)));

                        sbBody.Replace("[%USER_NAME%]", _usersContext.iUsers.Where(o => o.Id == userRoles.UserId).FirstOrDefault().UserName);
                        sbBody.Replace("[%PARTNER_NAME%]", _dbContext.M_RumahMakans.Where(o => o.PartnerId == t_AbsenMakanSopir.RumahMakanId).FirstOrDefault().PartnerName);
                        sbBody.Replace("[%CREW_NAME%]", t_AbsenMakanSopir.PengemudiId == null
                                                      ? _dbContext.M_Keneks.Where(o => o.Id == t_AbsenMakanSopir.KenekId).FirstOrDefault().Name
                                                      : _dbContext.M_Pengemudis.Where(o => o.Id == t_AbsenMakanSopir.PengemudiId).FirstOrDefault().Name);
                        sbBody.Replace("[%JABATAN%]", t_AbsenMakanSopir.PengemudiId == null ? "Asisten Pengemudi" : "Pengemudi");
                        sbBody.Replace("[%CHECK_IN_LOC%]", _dbContext.M_RumahMakans.Where(o => o.PartnerId == t_AbsenMakanSopir.RumahMakanId).FirstOrDefault().PartnerName);
                        sbBody.Replace("[%CHECK_IN_DATE%]", t_AbsenMakanSopir.Trandate.ToString("dd MMMM yyyy - HH:mm:ss"));
                        sbBody.Replace("[%HARGA%]", t_AbsenMakanSopir.DefaultHarga.ToString("N2"));

                        sBody = sbBody.ToString();
                    }
                    mail.Body = sBody;
                    mail.IsBodyHtml = true;

                    await _smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}