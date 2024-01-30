using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
   public class VerifySignatureResponse
    {
        public List<SignatureInfos> signatureInfos { get; set; }
        public class SignatureInfos
        {
            public string signer { get; set; }
            public string signedonTime { get; set; }
            public string authority { get; set; }
            public int timestampFlag { get; set; }
            public string timestampTime { get; set; }
            public int timestampVerifyFlag { get; set; }
            public int integrityFlag { get; set; }
        }
    }
}
