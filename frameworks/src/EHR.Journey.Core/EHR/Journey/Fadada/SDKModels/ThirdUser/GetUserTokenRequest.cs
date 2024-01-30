using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ThirdUser
{
    /// <summary>
    /// 授权管理-关闭法大大电子签服务
    /// </summary>
    [RemoteService("/thirdUser/getUserToken", "POST")]
    public class GetUserTokenRequest : BaseReqeust<GetUserTokenResponse>
    {
        public string grantCode { get; set; }
    }
}
