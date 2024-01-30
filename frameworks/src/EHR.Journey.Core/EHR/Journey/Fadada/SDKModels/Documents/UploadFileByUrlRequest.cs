using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 其他工具-根据链接上传原始文件
    /// </summary>
    [RemoteService("/documents/uploadFileByUrl", "POST")]
    public class UploadFileByUrlRequest : BaseReqeust<UploadFileByUrlResponse>
    {
        public string fileName { get; set; }
        public string fileType { get; set; }
        public string fileUrl { get; set; }
    }
}
