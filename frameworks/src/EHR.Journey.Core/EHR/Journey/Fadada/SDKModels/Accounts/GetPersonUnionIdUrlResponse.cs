using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    public class GetPersonUnionIdUrlResponse
    {
        public string nextUrl { get; set; }

        public string clientId { get; set; }

        public miniProgramConfigClass miniProgramConfig { get; set; }

        public class miniProgramConfigClass
        {
            public string appId { get; set; }
            public string path { get; set; }
        }

    }
}
