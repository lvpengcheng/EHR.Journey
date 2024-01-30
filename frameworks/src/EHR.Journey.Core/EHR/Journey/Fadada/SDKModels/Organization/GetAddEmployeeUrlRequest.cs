using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Organization
{
    /// <summary>
    /// 组织架构-邀请员工
    /// </summary>
    [RemoteService("/org/group/getAddEmployeeUrl", "POST")]
    public class GetAddEmployeeUrlRequest : BaseReqeust<GetAddEmployeeUrlResponse>
    {
        public string company { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public string redirectUrl { get; set; }
        public class EmployeeInfo
        {
            public string unionId { get; set; }
        }
    }
}
