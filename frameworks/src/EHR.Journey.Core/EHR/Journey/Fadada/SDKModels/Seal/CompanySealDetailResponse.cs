using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
   public class CompanySealDetailResponse
    {
        public string sealId { get; set; }
        public string sealName { get; set; }
        public string status { get; set; }
        public string sysFlag { get; set; }
        public string base64 { get; set; }
        public SealHolders sealHolders { get; set; }
        public class SealHolders
        {
            public string unionId { get; set; }
        }
    }
}
