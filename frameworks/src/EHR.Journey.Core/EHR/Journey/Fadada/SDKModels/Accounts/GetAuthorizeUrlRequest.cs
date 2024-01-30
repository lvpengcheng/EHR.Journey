using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 获取用户实名信息授权地址
    /// </summary>
    [RemoteService("/oauth2/getAuthorizeUrl", "POST")]
    public class GetAuthorizeUrlRequest : BaseReqeust<GetAuthorizeUrlResponse>
    {
        public string unionId { get; set; }
        public string redirectUrl { get; set; }
        public string scope { get; set; }
    }
}
