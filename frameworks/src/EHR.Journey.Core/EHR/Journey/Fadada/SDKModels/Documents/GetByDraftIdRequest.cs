using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 草稿文件下载
    /// </summary>
    [RemoteService("/documents/getByDraftId", "POST")]
    public class GetByDraftIdRequest : BaseReqeust<GetByDraftIdResponse>
    {
        public string draftId { get; set; }
        public string draftFileId { get; set; }
    }
}
