using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CheckPointPartner.WebUI.Model;
using RD.Lib;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using CheckPointPartner.WebUI.Data;
using Microsoft.AspNetCore.Authorization;
using CheckPointPartner.WebUI.Services;

namespace CheckPointPartner.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : RDController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsersContext _usersContext;
        private readonly IAppUser _appUser;

        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAppUser appUser, UsersContext usersContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersContext = usersContext;
            _appUser = appUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UsersListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                IUsersCollection aspNetUsers = new IUsersCollection(_usersContext);
                aspNetUsers.List(sKeyword, iSkip, iLength);

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = aspNetUsers.totalRecord,
                    recordsTotal = aspNetUsers.totalRecord,
                    data = aspNetUsers
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IActionResult GetUser(string p_iAspNetUsersId)
        {
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                IUsers user = _usersContext.iUsers.Where(o => o.Id == p_iAspNetUsersId).FirstOrDefault();
                bool bIsHaveDetail = _usersContext.UserDetails.Any(o => o.AspNetUserId == p_iAspNetUsersId);
                user.UserDetail = bIsHaveDetail 
                                ? _usersContext.UserDetailWithPartners.Where(o => o.AspNetUserId == p_iAspNetUsersId).FirstOrDefault()
                                : new UserDetailWithPartner();
                string sJsonData = JsonConvert.SerializeObject(user);

                response.ReferenceNumber = ReferenceNumber();
                response.DataTable = new RDDatatable();
                response.JsonData = sJsonData;
                response.IsSuccess = !string.IsNullOrEmpty(sJsonData);
                response.StatusCode = response.IsSuccess ? 200 : 500;
                response.Message = response.IsSuccess ? String.Empty : "Data Not Found";

                List<string> lstRolesId = new List<string>();
                List<IUserRoles> userRoles = _usersContext.iUserRoles.Where(o => o.UserId == p_iAspNetUsersId).ToList();
                foreach (IUserRoles userRole in userRoles)
                {
                    lstRolesId.Add(userRole.RoleId);
                }
                List<IRoles> roles = _usersContext.iRoles.Where(o => lstRolesId.Contains(o.Id)).ToList();

                HttpContext.Session.SetString("sessMasRoles", JsonConvert.SerializeObject(roles));
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(response);
        }

        public IActionResult RolesList(string p_iId, int draw)
        {
            List<IRoles> masRoleAccess = new List<IRoles>();
            try
            {
                string sessMasRoles = HttpContext.Session.GetString("sessMasRoles");
                if (!string.IsNullOrEmpty(sessMasRoles))
                {
                    masRoleAccess = JsonConvert.DeserializeObject<List<IRoles>>(sessMasRoles);
                }
                else
                {
                    List<string> iRoles = new List<string>();
                    foreach (IUserRoles userRoles in _usersContext.iUserRoles.Where(o => o.UserId == p_iId))
                    {
                        iRoles.Add(userRoles.RoleId);
                    }
                    masRoleAccess = _usersContext.iRoles.Where(o => iRoles.Contains(o.Id)).ToList();
                    HttpContext.Session.SetString("sessMasRoles", JsonConvert.SerializeObject(masRoleAccess));
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = masRoleAccess.Count,
                    recordsTotal = masRoleAccess.Count,
                    data = masRoleAccess
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult AddRoleAccess(string p_iRoleId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            List<IRoles> iRoles = new List<IRoles>();

            try
            {
                string sessMasRoles = HttpContext.Session.GetString("sessMasRoles");
                if (!string.IsNullOrEmpty(sessMasRoles)) iRoles = JsonConvert.DeserializeObject<List<IRoles>>(sessMasRoles);

                if (!iRoles.Any(o => o.Id == p_iRoleId))
                {
                    IRoles roles = _usersContext.iRoles.Where(o => o.Id == p_iRoleId).FirstOrDefault();
                    iRoles.Add(roles);

                    HttpContext.Session.SetString("sessMasRoles", JsonConvert.SerializeObject(iRoles));
                    bIsSuccess = true;
                }
                else
                {
                    sMessage = "The Role Access you try to add, is already exist for this user.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new
            {
                IsSuccess = bIsSuccess,
                Message = sMessage
            });
        }

        [HttpPost]
        public ActionResult DeleteRoleAccess(string p_iRoleId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            List<IRoles> iRoles = new List<IRoles>();

            try
            {
                string sessMasRoles = HttpContext.Session.GetString("sessMasRoles");
                if (!string.IsNullOrEmpty(sessMasRoles)) iRoles = JsonConvert.DeserializeObject<List<IRoles>>(sessMasRoles);

                if (iRoles.Any(o => o.Id == p_iRoleId))
                {
                    IRoles roles = iRoles.Where(o => o.Id == p_iRoleId).FirstOrDefault();
                    iRoles.Remove(roles);

                    HttpContext.Session.SetString("sessMasRoles", JsonConvert.SerializeObject(iRoles));
                    bIsSuccess = true;
                }
                else
                {
                    sMessage = "The Role Access you try to delete, is not exist";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new
            {
                IsSuccess = bIsSuccess,
                Message = sMessage
            });
        }

        [HttpPost]
        public ActionResult Save(IUsers p_oUser)
        {
            bool bIsExist = false;
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            List<IRoles> iRoles = new List<IRoles>();

            RDCustomResponse response = new RDCustomResponse();
            try
            {
                bIsExist = _usersContext.iUsers.Any(o => o.Id == p_oUser.Id || o.UserName == p_oUser.UserName);
                IUsers oUser = bIsExist ? _usersContext.iUsers.Where(o => o.Id == p_oUser.Id || o.UserName == p_oUser.Email).FirstOrDefault() : new IUsers();

                oUser.Id = bIsExist ? p_oUser.Id : NewGuid().ToString();
                oUser.UserName = p_oUser.UserName;
                oUser.NormalizedUserName = p_oUser.UserName.ToUpper();
                oUser.Email = p_oUser.Email;
                oUser.NormalizedEmail = p_oUser.Email.ToUpper();
                oUser.PhoneNumber = p_oUser.PhoneNumber;
                oUser.PasswordHash = bIsExist ? oUser.PasswordHash : HashPassword(p_oUser.UserName.Split("@")[0]);
                oUser.AccessFailedCount = !bIsExist ? 0 : p_oUser.AccessFailedCount;
                oUser.ConcurrencyStamp = Guid.NewGuid().ToString();
                oUser.EmailConfirmed = true;
                oUser.LockoutEnabled = false;
                oUser.LockoutEnd = null;
                oUser.PhoneNumberConfirmed = false;
                oUser.SecurityStamp = Guid.NewGuid().ToString();
                if (!bIsExist) _usersContext.iUsers.Add(oUser);
                else _usersContext.iUsers.Update(oUser);

                bool bIsHaveDetail = _usersContext.UserDetails.Any(o => o.UserDetailId == p_oUser.UserDetail.UserDetailId);
                UserDetail userDetail = new UserDetail();
                userDetail.UserDetailId = bIsHaveDetail ? p_oUser.UserDetail.UserDetailId : NewGuid();
                userDetail.AspNetUserId = oUser.Id;
                userDetail.Name = p_oUser.UserDetail.Name;
                userDetail.PhoneNumber = p_oUser.UserDetail.PhoneNumber;
                userDetail.UserImage = HttpContext.Request.Form.Files.Any(o => o.Name == "fileUserPhoto") ? "usr_" + p_oUser.Id : null;
                userDetail.PartnerId = p_oUser.UserDetail.PartnerId;
                userDetail.PartnerAddress = p_oUser.UserDetail.PartnerAddress;
                userDetail.PartnerImage = HttpContext.Request.Form.Files.Any(o => o.Name == "filePartnerPhoto") ? "ptr_" + p_oUser.Id : null;
                userDetail.IsActive = bIsHaveDetail ? p_oUser.UserDetail.IsActive : true;
                userDetail.IsDeleted = bIsHaveDetail ? p_oUser.UserDetail.IsDeleted : false;
                userDetail.CreateDate = bIsHaveDetail ? p_oUser.UserDetail.CreateDate : DateTime.Now;
                userDetail.CreateByUserId = bIsHaveDetail ? p_oUser.UserDetail.CreateByUserId : _appUser.CurrentUser().Id;
                userDetail.UpdateDate = DateTime.Now;
                userDetail.UpdateByUserId = _appUser.CurrentUser().Id;
                if (!bIsHaveDetail) _usersContext.UserDetails.Add(userDetail);
                else _usersContext.UserDetails.Update(userDetail);

                string sessMasRoles = HttpContext.Session.GetString("sessMasRoles");
                _usersContext.iUserRoles.RemoveRange(_usersContext.iUserRoles.Where(o => o.UserId == p_oUser.Id));
                if (!string.IsNullOrEmpty(sessMasRoles)) iRoles = JsonConvert.DeserializeObject<List<IRoles>>(sessMasRoles);
                foreach (IRoles oRole in iRoles)
                {
                    IUserRoles oUserRole = new IUserRoles();
                    oUserRole.RoleId = oRole.Id;
                    oUserRole.UserId = oUser.Id;

                    _usersContext.iUserRoles.Add(oUserRole);
                }
                _usersContext.SaveChanges();

                bIsSuccess = true;
                sMessage = bIsSuccess ? "Data Saved" : "Failed to Save Data";
            }
            catch (Exception ex)
            {
                sMessage = ex.Message.ToString();
            }

            response.IsSuccess = bIsSuccess;
            response.Message = sMessage;
            response.StatusCode = response.IsSuccess ? 200 : 500;

            return Json(response);
        }

        [HttpDelete]
        public ActionResult DeleteAsync(string p_iAspNetUsersId)
        {
            bool bIsExist = false;
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            List<IRoles> iRoles = new List<IRoles>();

            RDCustomResponse response = new RDCustomResponse();
            try
            {
                bIsExist = _usersContext.iUsers.Any(o => o.Id == p_iAspNetUsersId);
                if (bIsExist)
                {
                    IUsers users = new IUsers();
                    users = _usersContext.iUsers.Where(o => o.Id == p_iAspNetUsersId).FirstOrDefault();
                    users.EmailConfirmed = false;
                    _usersContext.iUsers.Update(users);
                    _usersContext.SaveChanges();
                    bIsSuccess = true;
                }

                sMessage = bIsSuccess ? "Data Deleted" : "Failed to Delete Data";
            }
            catch (Exception ex)
            {
                sMessage = ex.Message.ToString();
            }

            response.IsSuccess = bIsSuccess;
            response.Message = sMessage;
            response.StatusCode = response.IsSuccess ? 200 : 500;

            return Json(response);
        }

    }
}