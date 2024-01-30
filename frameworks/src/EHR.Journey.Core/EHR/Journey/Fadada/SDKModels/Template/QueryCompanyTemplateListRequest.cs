using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
    /// <summary>
    /// 模板列表查询
    /// </summary>
    [RemoteService("/template/queryCompanyTemplateList", "POST")]
    public class QueryCompanyTemplateListRequest : BaseReqeust<QueryCompanyTemplateListResponse>
    {
        public QueryInfo queryInfo { get; set; }
        public class QueryInfo
        {
            public int templateStatus { get; set; }
            public int currentPageNo { get; set; }
            public int pageSize { get; set; }
            public string keyword { get; set; }
        }
    }
}
