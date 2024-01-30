using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Organization
{
    /// <summary>
    /// 组织架构-移除分子公司
    /// </summary>
    [RemoteService("/org/group/removeSubCompany", "POST")]
   public class RemoveSubCompanyRequest : BaseReqeust<RemoveSubCompanyResponse>
    {
        public string parentCompany { get; set; }
        public string subCompany { get; set; }
    }
}
