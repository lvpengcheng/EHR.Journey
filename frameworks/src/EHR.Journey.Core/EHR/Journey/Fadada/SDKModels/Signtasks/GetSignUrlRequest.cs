using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
    /// <summary>
    /// 获取签署链接
    /// </summary>
    [RemoteService("/signtasks/getSignUrl", "POST")]
   public class GetSignUrlRequest : BaseReqeust<GetSignUrlResponse>
    {
        public string taskId { get; set; }
        public string unionId { get; set; }
        public string redirectUrl { get; set; }
        public int miniProgramSign { get; set; }
    }
}
