using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 存证管理-公证处保全报告下载
    /// </summary>
    [RemoteService("/documents/downloadEvidenceReport", "POST")]
    public class DownloadEvidenceReportRequest : BaseReqeust<DownloadEvidenceReportResponse>
    {
        public QueryInfo queryInfo { get; set; }
        public class QueryInfo
        {
            public string type { get; set; }
            public string unionId { get; set; }
            public string taskId { get; set; }
            public List<UnionIds> unionIds { get; set; }
            public class UnionIds
            {
            }
        }
    }
}
