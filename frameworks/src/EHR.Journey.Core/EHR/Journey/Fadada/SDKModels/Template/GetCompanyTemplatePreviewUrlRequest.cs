using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 获取模板文件预览地址
    /// </summary>
    [RemoteService("/template/getCompanyTemplatePreviewUrl", "POST")]
    public class GetCompanyTemplatePreviewUrlRequest : BaseReqeust<GetCompanyTemplatePreviewUrlResponse>
    {
        public TemplateInfo templateInfo { get; set; }
        public class TemplateInfo
        {
            public string templateId { get; set; }
        }
    }
}
