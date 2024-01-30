using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ReviseTask
{
    /// <summary>
    /// 定稿任务填充
    /// </summary>
    [RemoteService("/reviseTask/saveFillValues", "POST")]
    public class SaveFillValuesRequest : BaseReqeust<SaveFillValuesResponse>
    {
        public string taskId { get; set; }
        public List<RoleFillValues> roleFillValues { get; set; }
        public class RoleFillValues
        {
            public string roleName { get; set; }
            public List<FillValues> fillValues { get; set; }
        }
        public class FillValues
        {
            public string fileId { get; set; }
            public string values { get; set; }
        }
    }
}
