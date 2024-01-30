using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
   public class TemplatInitResponse
    {
        public string templateId { get; set; }
        public List<Roles> roles { get; set; }
        public class Roles
        {
            public string roleId { get; set; }
            public string roleName { get; set; }
            public int roleType { get; set; }
            public int rolePermission { get; set; }
            public int signSort { get; set; }
            public int fillSort { get; set; }
        }
    }
}
