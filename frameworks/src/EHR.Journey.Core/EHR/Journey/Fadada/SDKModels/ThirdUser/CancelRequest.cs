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
    [RemoteService("/thirdUser/cancel", "POST")]
    public class CancelRequest : BaseReqeust<CancelResponse>
    {
        public string unionId { get; set; }
    }
}
