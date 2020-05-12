using Api.Dtos;
using Api.Models;
using Api.ServiceRol;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Controllers.Api
{
    [RoutePrefix("api/Upu")]
    public class UpuController : ApiController
    {
        private static Entities _context;

        public UpuController()
        {
            _context = new Entities();
        }

        [Route("UPU")]
        [HttpGet]
        public static string UPU(Guid guidUser)
        {
            //MULTIPLE USERS
            var u = _context.Users.Where(a => a.guidUser == guidUser);
            if (u.Count() == 0)
                return null;

            var Users = _context.Users.Where(a => a.guidUser == guidUser).SingleOrDefault(a => a.parentId == 0);
            if (Users == null)
                return null;
            ROLServiceSoapClient service = new ROLServiceSoapClient();
            if (Users.areaTestUser)
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePosteAreaTest;
                service.ClientCredentials.UserName.Password = Users.pwdPosteAreaTest;
            }
            else
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePoste;
                service.ClientCredentials.UserName.Password = Users.pwdPoste;
            }
            var n = service.RecuperaNomiNazioniUPU(CountryLanguage.Italiano);
            var l = n.ListaNomi;
            return JsonConvert.SerializeObject(l);
        }
    }
}
