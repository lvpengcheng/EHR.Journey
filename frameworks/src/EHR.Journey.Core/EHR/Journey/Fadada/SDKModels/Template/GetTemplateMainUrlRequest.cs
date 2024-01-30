using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 获取模板维护地址
    /// </summary>
    [RemoteService("/template/getTemplateMainUrl", "POST")]
    public class GetTemplateMainUrlRequest : BaseReqeust<GetTemplateMainUrlResponse>
    {
        public TemplateInfo templateInfo { get; set; }
        public class TemplateInfo
        {
            public string templateId { get; set; }
        }
    }
}
