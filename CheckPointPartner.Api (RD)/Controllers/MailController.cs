using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AbsenSupir.WebApp.Configuration;
using CheckPointPartner.Api_RD.Data;
using CheckPointPartner.Api_RD.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RD.Lib;

namespace CheckPointPartner.Api_RD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private SmtpClient _smtpClient;
        private readonly AppSettings _appSettings;
        private readonly UsersContext _usersContext;
        string sMailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\template\\email\\");

        public MailController(SmtpClient smtpClient, IConfiguration configuration, UsersContext userContext)
        {
            _smtpClient = smtpClient;
            _usersContext = userContext;
            _appSettings = configuration.Get<AppSettings>();
        }

        [Route("TestMail")]
        [HttpPost]
        public async Task<IActionResult> TestMail(string p_sTemplateCode)
        {
            string sSubject = "GiTTerns Mail Sender";
            string sBody = "This message has no body.";

            try
            {
                #region Get Template
                sSubject = _appSettings.Email.Notification.Subject;
                if (Directory.Exists(sMailTemplatePath))
                {
                    StringBuilder sbBody = new StringBuilder();
                    sbBody.Append(RDFile.ReadStringFromTxtFile(Path.Combine(sMailTemplatePath, _appSettings.Email.Notification.MailBodyTemplateFileName)));

                    sBody = sbBody.ToString();
                }
                #endregion

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_appSettings.Email.Notification.EmailFrom);
                mail.Subject = sSubject;
                mail.Body = sBody;
                mail.IsBodyHtml = true;

                #region Assign Mail Retriever

                foreach (IUserRoles userRoles in _usersContext.iUserRoles.Where(o => o.RoleId == _usersContext.iRoles.Where(p => p.Name == "Administrator").FirstOrDefault().Id))
                {
                    mail.To.Add(_usersContext.iUsers.Where(o => o.Id == userRoles.UserId).FirstOrDefault().Email);
                }

                #endregion

                await _smtpClient.SendMailAsync(mail);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Ok("OK");

        }
    }
}