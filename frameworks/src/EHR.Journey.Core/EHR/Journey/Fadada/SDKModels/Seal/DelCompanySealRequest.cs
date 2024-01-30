using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
    /// <summary>
    /// 删除企业印章
    /// </summary>
    [RemoteService("/seal/delCompanySeal", "POST")]
   public class DelCompanySealRequest : BaseReqeust<DelCompanySealResponse>
    {
        public SealInfo sealInfo { get; set; }
        public Owner owner { get; set; }
        public class SealInfo
        {
            public string sealId { get; set; }
        }
        public class Owner
        {
            public string unionId { get; set; }
        }
    }
}
