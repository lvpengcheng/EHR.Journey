using FDD.OpenAPI.Attributes;
using FDD.OpenAPI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 模板文件上传
    /// </summary>
    [RemoteService("/template/uploadCompanyTemplateFile", "POST")]
    public class UploadCompanyTemplateFileRequest : BaseReqeust<UploadCompanyTemplateFileResponse>
    {

        [JsonIgnore]
        public FileItem file { get; set; }
        public TemplateInfo templateInfo { get; set; }
        public class TemplateInfo
        {
            public string fileHash { get; set; }
            public string fileType { get; set; }
            public string templateId { get; set; }
        }
    }
}
