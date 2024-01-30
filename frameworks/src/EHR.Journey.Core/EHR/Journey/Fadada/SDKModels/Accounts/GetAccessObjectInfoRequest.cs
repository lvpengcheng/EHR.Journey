using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 获取接入方实名绑定信息
    /// </summary>
    [RemoteService("/accounts/getAccessObjectInfo", "POST")]
    public class GetAccessObjectInfoRequest : BaseReqeust<GetAccessObjectInfoResponse>
    {

    }
}
