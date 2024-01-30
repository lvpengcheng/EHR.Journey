using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
    /// <summary>
    /// 获取签署任务发起地址
    /// </summary>
    [RemoteService("/signtasks/getSentUrl", "POST")]
    public class GetSentUrlRequest : BaseReqeust<GetSentUrlResponse>
    {
        public string taskId { get; set; }
    }
}