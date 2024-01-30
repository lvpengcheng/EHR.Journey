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
    /// 其他工具-签署文件验签
    /// </summary>
    [RemoteService("/documents/verifySignature", "POST")]
   public class VerifySignatureRequest : BaseReqeust<VerifySignatureResponse>
    {
        [JsonIgnore]
        public FileItem file { get; set; }
        public PdfInfo pdfInfo { get; set; }
        public class PdfInfo
        {
            public string fileHash { get; set; }
        }
    }
}
