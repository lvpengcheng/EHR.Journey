using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
   public class GetSignUrlResponse
    {
        public List<SignUrls> signUrls { get; set; }
        public MiniProgramConfig miniProgramConfig { get; set; }
        public class SignUrls
        {
            public string signUrl { get; set; }
        }
        public class MiniProgramConfig
        {
            public string appId { get; set; }
            public string path { get; set; }
        }
    }
}
