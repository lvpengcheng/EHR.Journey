using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Oauth2
{
    /// <summary>
    /// 授权管理-获取静默签授权地址
    /// </summary>
    [RemoteService("/oauth2/getAutoSignAuthUrl", "POST")]
    public class GetAutoSignAuthUrlRequest : BaseReqeust<GetAutoSignAuthUrlResponse>
    {
        public string unionId { get; set; }
        public string redirectUrl { get; set; }
    }
}
