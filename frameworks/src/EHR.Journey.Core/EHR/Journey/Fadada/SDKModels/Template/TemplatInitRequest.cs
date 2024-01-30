using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 创建模板
    /// </summary>
    [RemoteService("/template/templateInit", "POST")]
   public class TemplatInitRequest : BaseReqeust<TemplatInitResponse>
    {
        public TemplateInfo templateInfo { get; set; }
        public class TemplateInfo
        {
            public string templateName { get; set; }
            public string fileSource { get; set; }
            public string templateRemark { get; set; }
            public List<Roles> roles { get; set; }
        }
        public class Roles
        {
            public string roleName { get; set; }
            public int roleType { get; set; }
            public int rolePermission { get; set; }
            public int signSort { get; set; }
            public int fillSort { get; set; }
        }
    }
}
