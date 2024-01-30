using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 其他工具-根据关键字查询坐标
    /// </summary>
    [RemoteService("/documents/lookUpCoordinates", "POST")]
    public class LookUpCoordinatesRequest : BaseReqeust<LookUpCoordinatesResponse>
    {
        public QueryInfo queryInfo { get; set; }
        public class QueryInfo
        {
            public string fileId { get; set; }
            public string keyword { get; set; }
            public int pageNumber { get; set; }
            public int keywordStrategy { get; set; }
        }
    }
}
