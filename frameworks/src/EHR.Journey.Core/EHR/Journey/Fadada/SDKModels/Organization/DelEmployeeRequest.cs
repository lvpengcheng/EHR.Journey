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
    [RemoteService("/org/group/delEmployee", "POST")]
    public class DelEmployeeRequest : BaseReqeust<DelEmployeeResponse>
    {
        public string company { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public class EmployeeInfo
        {
            public string unionId { get; set; }
        }
    }
}
