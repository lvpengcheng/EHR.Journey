using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Organization
{
    /// <summary>
    /// 组织架构-变更企业管理员
    /// </summary>
    [RemoteService("/org/group/getChangeCompanyMajorUrl", "POST")]
    public class GetChangeCompanyMajorUrlRequest : BaseReqeust<GetChangeCompanyMajorUrlResponse>
    {
        public string unionId { get; set; }
    }
}
