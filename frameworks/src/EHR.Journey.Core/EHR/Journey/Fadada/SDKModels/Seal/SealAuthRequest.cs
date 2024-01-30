using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
    /// <summary>
    /// 企业印章授权
    /// </summary>
    [RemoteService("/seal/sealAuth", "POST")]
    public class SealAuthRequest : BaseReqeust<SealAuthResponse>
    {
        public SealInfo sealInfo { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public Owner owner { get; set; }
        public class SealInfo
        {
            public string sealId { get; set; }
        }
        public class EmployeeInfo
        {
            public string unionId { get; set; }
            public string companyUnionId { get; set; }
        }
        public class Owner
        {
            public string unionId { get; set; }
        }
    }
}
