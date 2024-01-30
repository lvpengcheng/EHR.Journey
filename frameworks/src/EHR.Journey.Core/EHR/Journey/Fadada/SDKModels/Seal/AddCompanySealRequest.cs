using FDD.OpenAPI.Attributes;
using FDD.OpenAPI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
    /// <summary>
    /// 新增企业印章
    /// </summary>
    [RemoteService("/seal/addCompanySeal", "POST")]
    public class AddCompanySealRequest : BaseReqeust<AddCompanySealResponse>
    {
        [JsonIgnore]
        public FileItem image { get; set; }
        public SealInfo sealInfo { get; set; }
        public Owner owner { get; set; }
        public class SealInfo
        {
            public string imageHash { get; set; }
            public string sealName { get; set; }
        }
        public class Owner
        {
            public string unionId { get; set; }
        }
    }
}
