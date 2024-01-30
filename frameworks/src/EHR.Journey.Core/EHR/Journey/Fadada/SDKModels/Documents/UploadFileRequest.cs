using FDD.OpenAPI.Attributes;
using FDD.OpenAPI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 原始文件上传
    /// </summary>
    [RemoteService("/documents/uploadFile", "POST")]
    public class UploadFileRequest : BaseReqeust<UploadFileResponse>
    {
        public int fileType { get; set; }

        public string fileContentHash { get; set; }

        [JsonIgnore]
        public FileItem fileContent { get; set; }

    }
}
