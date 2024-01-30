using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 模板文件删除
    /// </summary>
    [RemoteService("/template/delCompanyTemplateFile", "POST")]
   public class DelCompanyTemplateFileRequest : BaseReqeust<DelCompanyTemplateFileResponse>
    {
        public TemplateInfo templateInfo { get; set; }
        public class TemplateInfo
        {
            public string templateId { get; set; }
            public string fileId { get; set; }
            public string deleteWay { get; set; }
        }
    }
}
