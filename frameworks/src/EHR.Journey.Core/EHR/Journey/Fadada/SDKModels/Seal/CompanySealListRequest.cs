using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
    /// <summary>
    /// 企业印章列表查询
    /// </summary>
    [RemoteService("/seal/companySealList", "POST")]
    public class CompanySealListRequest : BaseReqeust<CompanySealListResponse>
    {
        public SealInfo sealInfo { get; set; }
        public Owner owner { get; set; }
        public class SealInfo
        {
            public int loadUnPass { get; set; }
        }
        public class Owner
        {
            public string unionId { get; set; }
        }
    }
}
