using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 其他工具-根据uuid下载文件base64
    /// </summary>
    [RemoteService("/accounts/getFileBase64", "POST")]
    public class GetFileBase64Request : BaseReqeust<GetFileBase64Response>
    {
        public string uuid { get; set; }
    }
}
