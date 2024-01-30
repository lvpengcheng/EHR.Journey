using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
   public class GetAccessObjectInfoResponse
    {
        public string companyUnionId { get; set; }

        public string companyName { get; set; }
        public string managerUnionId { get; set; }

        public string managerName { get; set; }
    }
}
