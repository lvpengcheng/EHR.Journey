using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 模板详情信息查询
    /// </summary>
    [RemoteService("/template/getTemplateDetail", "POST")]
    public class GetTemplateDetailRequest : BaseReqeust<GetTemplateDetailResponse>
    {
        public string templateId { get; set; }
    }
}
