using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
    /// <summary>
    /// 企业印章详情查询
    /// </summary>
    [RemoteService("/seal/companySealDetail", "POST")]
    public class CompanySealDetailRequest : BaseReqeust<CompanySealDetailResponse>
    {
        public SealInfo sealInfo { get; set; }
        public Owner owner { get; set; }
        public class SealInfo
        {
            public int sealId { get; set; }
        }
        public class Owner
        {
            public string unionId { get; set; }
        }
    }
}
