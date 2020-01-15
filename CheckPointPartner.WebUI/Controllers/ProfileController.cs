using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CheckPointPartner.WebUI.Model;
using CheckPointPartner.WebUI.Data;
using CheckPointPartner.WebUI.Models;
using CheckPointPartner.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;

namespace CheckPointPartner.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IAppUser _appUser;
        private readonly UsersContext _usersContext;

        string sUserPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Assets\\dist\\img\\user\\");
        string sUserPhotoThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Assets\\dist\\img\\user\\thumbs\\");

        #endregion

        #region Constructor

        public ProfileController(IConfiguration configuration, IAppUser appUser, UsersContext usersContext)
        {
            _usersContext = usersContext;
            _configuration = configuration;
            _appUser = appUser;

            if (!Directory.Exists(sUserPhotoPath)) Directory.CreateDirectory(sUserPhotoPath);
            if (!Directory.Exists(sUserPhotoThumbsFilePath)) Directory.CreateDirectory(sUserPhotoThumbsFilePath);
        }

        #endregion

        #region Method

        public IActionResult Index()
        {
            IUsers user = new IUsers();
            user = _appUser.CurrentUser(); ;

            return View(user);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel p_oModel)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                if (p_oModel.NewPassword != p_oModel.ConfirmNewPassword)
                {
                    response.IsSuccess = true;
                    response.Message = "Password baru tidak sama..!";
                    return Json(response);
                }

                IUsers user = _appUser.CurrentUser();
                user.PasswordHash = HashPassword(p_oModel.NewPassword);

                _usersContext.iUsers.Update(user);
                _usersContext.SaveChanges();

                response.Message = "Password berhasil dirubah";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public IActionResult UpdateProfile(IUsers p_oUsers)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                IUsers user = _appUser.CurrentUser();
                bool bIsHaveDetail = _usersContext.UserDetails.Any(o => o.UserDetailId == p_oUsers.UserDetail.UserDetailId);

                user.UserName = p_oUsers.UserName;
                user.NormalizedUserName = p_oUsers.UserName.ToUpper();
                user.Email = p_oUsers.Email;
                user.NormalizedEmail = p_oUsers.Email.ToUpper();
                user.PhoneNumber = p_oUsers.PhoneNumber;

                UserDetail userDetail = new UserDetail();
                userDetail.UserDetailId = bIsHaveDetail ? p_oUsers.UserDetail.UserDetailId : NewGuid();
                userDetail.AspNetUserId = p_oUsers.Id;
                userDetail.Name = p_oUsers.UserDetail.Name;
                userDetail.PhoneNumber = p_oUsers.UserDetail.PhoneNumber;
                userDetail.UserImage = HttpContext.Request.Form.Files.Any(o => o.Name == "fileUserPhoto") ? "usr_" + user.Id : userDetail.UserImage;
                userDetail.PartnerId = p_oUsers.UserDetail.PartnerId;
                userDetail.PartnerAddress = p_oUsers.UserDetail.PartnerAddress;
                userDetail.PartnerImage = HttpContext.Request.Form.Files.Any(o => o.Name == "filePartnerPhoto") ? "ptr_" + user.Id : userDetail.PartnerImage;
                userDetail.IsActive = bIsHaveDetail ? p_oUsers.UserDetail.IsActive : true;
                userDetail.IsDeleted = bIsHaveDetail ? p_oUsers.UserDetail.IsDeleted : false;
                userDetail.CreateDate = bIsHaveDetail ? p_oUsers.UserDetail.CreateDate : DateTime.Now;
                userDetail.CreateByUserId = bIsHaveDetail ? p_oUsers.UserDetail.CreateByUserId : user.Id;
                userDetail.UpdateDate = DateTime.Now;
                userDetail.UpdateByUserId = user.Id;

                List<IFormFile> formFiles = new List<IFormFile>();
                foreach (IFormFile oFile in HttpContext.Request.Form.Files)
                {
                    string sFileName = oFile.Name.ToLower().Contains("partner")
                                        ? user.UserDetail.PartnerImage + ".jpeg"
                                        : user.UserDetail.UserImage + ".jpeg";
                    using (var fileStream = new FileStream(Path.Combine(sUserPhotoPath, sFileName), FileMode.Create))
                    {
                        oFile.CopyTo(fileStream);
                        RDImageHelper.CompressAndSaveImage(Path.Combine(sUserPhotoPath, sFileName), Path.Combine(sUserPhotoThumbsFilePath, sFileName), 80, 64, 0, 256);
                    }
                }

                _usersContext.iUsers.Update(user);
                if (bIsHaveDetail) _usersContext.UserDetails.Update(userDetail);
                else _usersContext.UserDetails.Add(userDetail);
                _usersContext.SaveChanges();

                response.Message = "Profil berhasil diperbaharui";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion

    }
}