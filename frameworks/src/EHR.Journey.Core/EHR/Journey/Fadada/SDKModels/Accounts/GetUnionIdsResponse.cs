using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
   public class GetUnionIdsResponse
    {
        public List<UnionIdInfos> unionIdInfos { get; set; }
        public class UnionIdInfos
        {
            public string unionId { get; set; }
            public int identityType { get; set; }
            public string name { get; set; }
        }
    }
}
