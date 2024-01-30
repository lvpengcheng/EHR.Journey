using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Oauth2
{
    /// <summary>
    /// 授权管理-静默签取消授权
    /// </summary>
    [RemoteService("/oauth2/cancelAuthSignAuth", "POST")]
    public class CancelAuthSignAuthRequest : BaseReqeust<CancelAuthSignAuthResponse>
    {
        public string unionId { get; set; }
    }
}
