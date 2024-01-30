using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 模板文件下载
    /// </summary>
    [RemoteService("/template/downloadCompanyTemplateFile", "POST")]
    public class DownloadCompanyTemplateFileRequest : BaseReqeust<DownloadCompanyTemplateFileResponse>
    {
        public TemplateInfo templateInfo { get; set; }
        public class TemplateInfo
        {
            public string templateId { get; set; }
        }
    }
}
