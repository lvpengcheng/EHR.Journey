using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ReviseTask
{
    /// <summary>
    /// 定稿任务撤销
    /// </summary>
    [RemoteService("/reviseTask/cancelReviseTask", "POST")]
    public class CancelReviseTaskRequest : BaseReqeust<CancelReviseTaskResponse>
    {
        public string taskId { get; set; }
    }
}
