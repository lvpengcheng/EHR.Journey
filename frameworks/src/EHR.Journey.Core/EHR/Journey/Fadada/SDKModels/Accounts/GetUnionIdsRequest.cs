using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 其他工具-根据clientId获取unionId
    /// </summary>
    [RemoteService("/accounts/getUnionIds", "POST")]
    public class GetUnionIdsRequest : BaseReqeust<GetUnionIdsResponse>
    {
        public string clientId { get; set; }
        public int identityType { get; set; }
    }
}
