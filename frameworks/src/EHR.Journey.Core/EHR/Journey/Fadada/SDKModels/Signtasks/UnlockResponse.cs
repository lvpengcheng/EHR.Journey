using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
   public class UnlockResponse
    {
        public List<Signers> signers { get; set; }
        public class Signers
        {
            public Signer signer { get; set; }
            public ExternalSigner externalSigner { get; set; }
            public int locks { get; set; }
        }
        public class Signer
        {
            public Signatory signatory { get; set; }
            public Corp corp { get; set; }
        }
        public class ExternalSigner
        {
            public string mobile { get; set; }
            public string personName { get; set; }
            public ExternalCorp externalCorp { get; set; }
        }
        public class ExternalCorp
        {
            public string corpName { get; set; }
        }
        public class Signatory
        {
            public string signerId { get; set; }
        }
        public class Corp
        {
            public string corpId { get; set; }
        }
    }
}
