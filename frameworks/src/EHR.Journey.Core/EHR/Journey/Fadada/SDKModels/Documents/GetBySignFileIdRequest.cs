using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 签署文件下载
    /// </summary>
    [RemoteService("/documents/getBySignFileId", "POST")]
   public class GetBySignFileIdRequest : BaseReqeust<GetBySignFileIdResponse>
    {
        public string taskId { get; set; }
        public string signFileId { get; set; }
    }
}
