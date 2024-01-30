using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Organization
{
    /// <summary>
    /// 组织架构-公司列表查询
    /// </summary>
    [RemoteService("/org/group/getChildCompanyList", "POST")]
    public class GetChildCompanyListRequest : BaseReqeust<GetChildCompanyListResponse>
    {
        public string company { get; set; }
    }
}
