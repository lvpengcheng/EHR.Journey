using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
    /// <summary>
    /// 引用模板创建签署任务
    /// </summary>
    [RemoteService("/signtasks/createTaskByDraftId", "POST")]
   public class CreateTaskByDraftIdRequest : BaseReqeust<CreateTaskByDraftIdResponse>
    {
        public string draftId { get; set; }
        public string taskSubject { get; set; }
        public string status { get; set; }
        public int sort { get; set; }
        public Sender sender { get; set; }
        public List<Signers> signers { get; set; }
        public List<Ccs> ccs { get; set; }
        public int autoArchive { get; set; }
        public class Sender
        {
            public string unionId { get; set; }
        }
        public class Ccs
        {
            public string personName { get; set; }
            public string companyName { get; set; }
            public Notice notice { get; set; }
        }
        public class Signers
        {
            public string templateRoleName { get; set; }
            public Signer signer { get; set; }
            public ExternalSigner externalSigner { get; set; }
            public int signOrder { get; set; }
            public int locks { get; set; }
        }
        public class ExternalSigner
        {
            public string mobile { get; set; }
            public string personName { get; set; }
            public ExternalCorp externalCorp { get; set; }
        }
        public class Signer
        {
            public Signatory signatory { get; set; }
            public Corp corp { get; set; }
            public SignAction signAction { get; set; }
            public Notice notice { get; set; }
        }
        public class ExternalCorp
        {
            public string corpName { get; set; }
        }
        public class Signatory
        {
            public string signerId { get; set; }
            public Seal seal { get; set; }
        }
        public class Corp
        {
            public string corpId { get; set; }
            public Seal seal { get; set; }
        }
        public class SignAction
        {
            public string signWay { get; set; }
            public string signIntendWay { get; set; }
        }
        public class Notice
        {
            public string notifyWay { get; set; }
            public string notifyAddress { get; set; }
        }
        public class Seal
        {
            public string sealId{ get; set; }
        }
    }
}
