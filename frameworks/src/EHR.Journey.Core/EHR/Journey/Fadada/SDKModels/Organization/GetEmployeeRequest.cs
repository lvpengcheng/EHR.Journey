using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Organization
{
    /// <summary>
    /// 组织架构-员工列表查询
    /// </summary>
    [RemoteService("/org/group/getEmployee", "POST")]
    public class GetEmployeeRequest : BaseReqeust<GetEmployeeResponse>
    {
        public string company { get; set; }
        public string offset { get; set; }
        public string size { get; set; }
    }
}
