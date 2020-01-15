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

namespace CheckPointPartner.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReportController : RDController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsersContext _usersContext;

        public ReportController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, UsersContext usersContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersContext = usersContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckInReportListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                CheckInReportCollection checkInReportCollection = new CheckInReportCollection(_usersContext);
                checkInReportCollection.List(sKeyword, iSkip, iLength);

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = checkInReportCollection.totalRecord,
                    recordsTotal = checkInReportCollection.totalRecord,
                    data = checkInReportCollection
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IActionResult GetCheckInDetail(int p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                CheckInReport checkInReport = _usersContext.CheckInReports.Where(o => o.IdUangJalan == p_iId).FirstOrDefault();

                response.JsonData = JsonConvert.SerializeObject(checkInReport);
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(response);
        }

    }
}