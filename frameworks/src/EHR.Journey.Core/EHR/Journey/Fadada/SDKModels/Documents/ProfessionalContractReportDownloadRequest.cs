using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
    /// <summary>
    /// 存证管理-技术报告下载
    /// </summary>
    [RemoteService("/documents/professionalContractReportDownload", "POST")]
    public class ProfessionalContractReportDownloadRequest : BaseReqeust<ProfessionalContractReportDownloadResponse>
    {
        public ContractInfo contractInfo { get; set; }
        public class ContractInfo
        {
            public string taskId { get; set; }
        }
    }
}
