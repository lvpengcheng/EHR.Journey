using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
    /// <summary>
    /// 获取签署文件预览地址
    /// </summary>
    [RemoteService("/signtasks/getSignPreviewUrl", "POST")]
   public class GetSignPreviewUrlRequest : BaseReqeust<GetSignPreviewUrlResponse>
    {
        public string taskId { get; set; }
    }
}
