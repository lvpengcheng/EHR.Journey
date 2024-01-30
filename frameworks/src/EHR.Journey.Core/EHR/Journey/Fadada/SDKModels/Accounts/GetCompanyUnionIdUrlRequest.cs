using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 获取企业实名绑定地址
    /// </summary>
    [RemoteService("/accounts/getCompanyUnionIdUrl", "POST")]
    public class GetCompanyUnionIdUrlRequest : BaseReqeust<GetCompanyUnionIdUrlResponse>
    {

        public Bank bank { get; set; }
        public string clientId { get; set; }
        public string redirectUrl { get; set; }
        public string allowModify { get; set; }
        public Company company { get; set; }
        public string authScheme { get; set; }
        public string authScope { get; set; }
        public Applicant applicant { get; set; }
        public Notice notice { get; set; }

        public class Bank
        {
            public string bankProvinceName { get; set; }
            public string bankCardNo { get; set; }
            public string bankName { get; set; }
            public string bankCityName { get; set; }
            public string bandBranchName { get; set; }
        }

        public class Company
        {
            public string legalName { get; set; }
            public string organizationType { get; set; }
            public string authorizationFileBase64 { get; set; }
            public string companyName { get; set; }
            public string creditImageBase64 { get; set; }
            public string creditNo { get; set; }
        }

        public class Applicant
        {
            public string unionId { get; set; }
            public string applicantType { get; set; }
        }

        public class Notice
        {
            public int notifyWay { get; set; }
            public string notifyAddress { get; set; }
        }
    }
}
