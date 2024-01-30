using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
   public class GetCompanyInfoResponse
    {
        public string unionId { get; set; }
        public CompanyInfo companyInfo { get; set; }
        public Applicant applicant { get; set; }
        public BankInfo bankInfo { get; set; }
        public class CompanyInfo
        {
            public string companyName { get; set; }
            public string organizationType { get; set; }
            public string certType { get; set; }
            public string organizationCard { get; set; }
            public string legalName { get; set; }
            public string legal { get; set; }
            public int verifyType { get; set; }
            public string authenticateStatus { get; set; }
            public string hasAgent { get; set; }
            public string auditorTime { get; set; }
            public string companyEmail { get; set; }
        }
        public class Applicant
        {
            public string applicantName { get; set; }
            public string applicantMobile { get; set; }
        }
        public class BankInfo
        {
            public string bankName { get; set; }
            public string bankDetailName { get; set; }
            public string bankCardNo { get; set; }
            public string status { get; set; }
            public string enterTime { get; set; }
        }
    }
}
