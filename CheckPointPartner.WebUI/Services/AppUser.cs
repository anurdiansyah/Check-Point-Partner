using CheckPointPartner.WebUI.Model;
using CheckPointPartner.WebUI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckPointPartner.WebUI.Services
{
    public class AppUser : RDController, IAppUser
    {
        private readonly UsersContext _usersContext;
        public IUsers _iUser = new IUsers();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppUser(UsersContext usersContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _usersContext = usersContext;
        }

        public IUsers CurrentUser()
        {
            _iUser = _usersContext.iUsers.Where(o => o.Id == _httpContextAccessor.HttpContext.User.GetUserId()).FirstOrDefault();
            if (_usersContext.UserDetails.Any(o => o.AspNetUserId == _iUser.Id))
            {
                _iUser.UserDetail = _usersContext.UserDetailWithPartners.Where(o => o.AspNetUserId == _iUser.Id).FirstOrDefault();
            }
            return _iUser;
        }
    }
}
