using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Dtos
{
    public class ConfigurationPecUsersDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int pecTypeId { get; set; }
    }
}