using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ThirdUser
{
    /// <summary>
    /// 授权管理-获取法大大电子签服务开通地址
    /// </summary>
    [RemoteService("/thirdUser/getOpenServerUrl", "POST")]
   public class GetOpenServerUrlRequest : BaseReqeust<GetOpenServerUrlResponse>
    {
        public string unionId { get; set; }
        public string redirectUrl { get; set; }
    }
}
