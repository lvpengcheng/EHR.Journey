using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Seal
{
   public class CompanySealListResponse
    {
        public List<Seals> seals { get; set; }
        public class Seals
        {
            public string sealId { get; set; }
            public string sealName { get; set; }
            public string status { get; set; }
            public string sysFlag { get; set; }
        }
    }
}
