using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
    /// <summary>
    /// 查询签署任务详情
    /// </summary>
    [RemoteService("/signtasks/getTaskDetailByTaskId", "POST")]
   public class GetTaskDetailByTaskIdRequest : BaseReqeust<GetTaskDetailByTaskIdResponse>
    {
        public string taskId { get; set; }
        public string unionId { get; set; }
    }
}
