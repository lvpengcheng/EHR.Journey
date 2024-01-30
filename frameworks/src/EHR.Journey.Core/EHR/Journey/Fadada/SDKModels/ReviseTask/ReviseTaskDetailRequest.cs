using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ReviseTask
{
    /// <summary>
    /// 获取定稿任务详情
    /// </summary>
    [RemoteService("/reviseTask/reviseTaskDetail", "POST")]
    public class ReviseTaskDetailRequest : BaseReqeust<ReviseTaskDetailResponse>
    {
        public string taskId { get; set; }
    }
}
