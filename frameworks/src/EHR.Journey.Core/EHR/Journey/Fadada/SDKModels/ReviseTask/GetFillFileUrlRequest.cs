using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ReviseTask
{
    /// <summary>
    /// 创建模板定稿任务
    /// </summary>
    [RemoteService("/reviseTask/getFillFileUrl", "POST")]
   public class GetFillFileUrlRequest : BaseReqeust<GetFillFileUrlResponse>
    {
        public string taskId { get; set; }
        public string roleName { get; set; }
    }
}
