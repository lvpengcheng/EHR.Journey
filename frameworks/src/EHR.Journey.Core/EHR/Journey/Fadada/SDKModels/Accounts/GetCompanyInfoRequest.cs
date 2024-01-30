using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 企业实名信息查询
    /// </summary>
    [RemoteService("/accounts/getCompanyInfo", "POST")]
    public class GetCompanyInfoRequest : BaseReqeust<GetCompanyInfoResponse>
    {
        public string unionId { get; set; }
    }
}
