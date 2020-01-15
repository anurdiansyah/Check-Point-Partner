using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using CheckPointPartner.Api_RD.Model;
using AbsenSupir.WebApp.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AbsenSupir.WebApp.Controllers
{
    /* [Authorize]*/
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private SmtpClient smtpClient;
        private string sUserEmail = string.Empty;

        public ValuesController(IGlobalHttpClient globalHttpClient, IHttpContextAccessor httpContextAccessor, SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok( new { API = "OK" });
        }
    }
}
