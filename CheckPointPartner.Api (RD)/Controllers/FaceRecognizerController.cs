using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AbsenSupir.WebApp.Data;
using CheckPointPartner.Api_RD.Model;
using AbsenSupir.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;

namespace AbsenSupir.WebApp.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    public class FaceRecognizerController : RDController
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IGlobalHttpClient _globalHttpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string sUserEmail = string.Empty;
        string sTrackDatFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\file\\track");
        string sCrewPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\crew");
        string sCrewPhotoThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\crew\\thumbs");

        public FaceRecognizerController(ApplicationDbContext context, IGlobalHttpClient globalHttpClient, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = context;
            _globalHttpClient = globalHttpClient;
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null) sUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("CrewDetail")]
        [HttpGet]
        public async Task<IActionResult> GetCrewDetail(string p_sCrewTagNo)
        {
            HttpClient httpClient = await _globalHttpClient.GetClient();
            try
            {
                if (_dbContext.M_Crews.Any(o=>o.TagNo == p_sCrewTagNo))
                {
                    M_Crew oCrew = _dbContext.M_Crews.Where(o => o.TagNo == p_sCrewTagNo).FirstOrDefault();
                    byte[] myByte = await httpClient.GetByteArrayAsync("http://system.timurterang.com:5998/api/image/" + oCrew.PhotoId);
                    myByte = myByte == null ? await httpClient.GetByteArrayAsync("http://system.timurterang.com:5998/api/image/" + "00000000-0000-0000-0000-000000000000") : myByte;
                    oCrew.Base64Photo = Convert.ToBase64String(myByte);

                    oCrew.BirthDate = oCrew.BirthDate.ToUniversalTime();
                    if(oCrew.JoinDate != null) oCrew.JoinDate = Convert.ToDateTime(oCrew.JoinDate).ToUniversalTime();
                    if (oCrew.ResignDate != null) oCrew.ResignDate = Convert.ToDateTime(oCrew.ResignDate).ToUniversalTime();
                    oCrew.InputOn = oCrew.InputOn.ToUniversalTime();
                    oCrew.LastUpdateOn = oCrew.LastUpdateOn.ToUniversalTime();

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
                        default:
                            break;

                    }

                    FaceRecognizerData oFaceRecognizerData = _dbContext.FaceRecognizerDatas.Where(o => o.TagNumber == p_sCrewTagNo).FirstOrDefault();
                    oCrew.HaveFaceRecognizerData = oFaceRecognizerData != null;
                    oCrew.FaceRecognizerFileName = oFaceRecognizerData != null ? oFaceRecognizerData.FileName : "";
                    oCrew.FaceRecognizerDataVersion = oFaceRecognizerData != null ? oFaceRecognizerData.Version : 0;

                    return Json(oCrew);
                }

                return BadRequest("Data " + p_sCrewTagNo + " not Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [Route("SaveFaceData")]
        [HttpPost]
        public async Task<IActionResult> SaveFaceDataAsync(IFormFile file)
        {
            FaceRecognizerData p_oFaceRecognizerData = new JavaScriptSerializer().Deserialize<FaceRecognizerData>(Request.Form.Where(o => o.Key == "p_oFaceRecognizerData").FirstOrDefault().Value);
            FaceRecognizerData oFaceRecognizerData = _dbContext.FaceRecognizerDatas.Where(o => o.TagNumber == p_oFaceRecognizerData.TagNumber).FirstOrDefault();

            try
            {

                if (oFaceRecognizerData == null)
                {
                    oFaceRecognizerData = new FaceRecognizerData();

                    oFaceRecognizerData.CrewId = p_oFaceRecognizerData.CrewId;
                    oFaceRecognizerData.TagNumber = p_oFaceRecognizerData.TagNumber;
                    oFaceRecognizerData.Name = p_oFaceRecognizerData.Name;
                    oFaceRecognizerData.FileName = p_oFaceRecognizerData.FileName;

                    oFaceRecognizerData.Version = 1;
                    oFaceRecognizerData.IOn = oFaceRecognizerData.UOn = DateTime.Now.ToLocalTime();
                    oFaceRecognizerData.IBy = oFaceRecognizerData.UBy = CurrentUserId(HttpContext).ToString();

                    _dbContext.FaceRecognizerDatas.Add(oFaceRecognizerData);
                }
                else
                {
                    oFaceRecognizerData.FileName = p_oFaceRecognizerData.FileName;

                    oFaceRecognizerData.Version += 1;
                    oFaceRecognizerData.IOn = oFaceRecognizerData.IOn.ToLocalTime();
                    oFaceRecognizerData.UOn = DateTime.Now.ToLocalTime();
                    oFaceRecognizerData.UBy = CurrentUserId(HttpContext).ToString();

                    _dbContext.FaceRecognizerDatas.Update(oFaceRecognizerData);
                }

                if (file != null || file.Length > 0)
                {
                    if (!Directory.Exists(sTrackDatFilePath)) Directory.CreateDirectory(sTrackDatFilePath);
                    var datFilePath = Path.Combine(sTrackDatFilePath, file.FileName);
                    using (var stream = new FileStream(datFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _dbContext.SaveChanges();

                return Ok(oFaceRecognizerData);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                p_oFaceRecognizerData = null;
                oFaceRecognizerData = null;
            }
        }

        [Route("RecordedFaceList")]
        [HttpGet]
        public IActionResult GetFaceDataList(string p_sKeyword)
        {
            try
            {
                p_sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;
                List<FaceRecognizerData> lstFaceRecognizer = _dbContext.FaceRecognizerDatas.Where(o=>o.Name.Contains(p_sKeyword) || o.TagNumber.Contains(p_sKeyword)).ToList();
                foreach (FaceRecognizerData oData in lstFaceRecognizer)
                {
                    String sThumbsName = @"\\thumb-" + oData.TagNumber + ".jpeg";
                    if (System.IO.File.Exists(sCrewPhotoThumbsFilePath + sThumbsName))
                    {
                        oData.Base64Thumbnails = Convert.ToBase64String(RDImageHelper.ImageToByteArray(sCrewPhotoThumbsFilePath + sThumbsName));
                    }

                    oData.IOn = oData.IOn.ToLocalTime();
                    oData.UOn = oData.IOn.ToLocalTime();
                }

                return Json(lstFaceRecognizer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
            }
        }
    }
}