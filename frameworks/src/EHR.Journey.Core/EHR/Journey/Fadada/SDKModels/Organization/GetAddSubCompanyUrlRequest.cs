using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Organization
{
    /// <summary>
    /// 组织架构-添加分子公司
    /// </summary>
    [RemoteService("/org/group/getAddSubCompanyUrl", "POST")]
    public class GetAddSubCompanyUrlRequest : BaseReqeust<GetAddSubCompanyUrlResponse>
    {
        public string parentCompany { get; set; }
        public string subCompany { get; set; }
        public string redirectUrl { get; set; }
    }
}
