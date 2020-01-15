using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckPointPartner.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CheckPointPartner.WebUI.Model;
using CheckPointPartner.WebUI.Data;

namespace CheckPointPartner.WebUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UsersContext _usersContext;

        public DashboardController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, UsersContext usersContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersContext = usersContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckInListAsync(int? p_iIdPartner, string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                CheckInCollection checkIn = new CheckInCollection(_usersContext);
                checkIn.List(p_iIdPartner, sKeyword, iSkip, iLength);

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = checkIn.totalRecord,
                    recordsTotal = checkIn.totalRecord,
                    data = checkIn
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
