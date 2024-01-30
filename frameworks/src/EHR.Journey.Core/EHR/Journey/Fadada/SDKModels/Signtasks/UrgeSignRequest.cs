using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
    /// <summary>
    /// 签署任务撤销
    /// </summary>
    [RemoteService("/signtasks/urgeSign", "POST")]
    public class UrgeSignRequest : BaseReqeust<UrgeSignResponse>
    {
        public string taskId { get; set; }
    }
}
