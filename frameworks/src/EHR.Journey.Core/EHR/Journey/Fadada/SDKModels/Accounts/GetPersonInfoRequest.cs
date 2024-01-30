using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 个人实名信息查询
    /// </summary>
    [RemoteService("/accounts/getPersonInfo", "POST")]
    public class GetPersonInfoRequest : BaseReqeust<GetPersonInfoResponse>
    {
        public string unionId { get; set; }
    }
}
