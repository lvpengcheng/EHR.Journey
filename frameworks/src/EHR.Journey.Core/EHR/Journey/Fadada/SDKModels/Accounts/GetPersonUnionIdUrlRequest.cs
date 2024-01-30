using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
    /// <summary>
    /// 获取个人实名绑定地址
    /// </summary>
    [RemoteService("/accounts/getPersonUnionIdUrl", "POST")]
    public class GetPersonUnionIdUrlRequest : BaseReqeust<GetPersonUnionIdUrlResponse>
    {
        public string clientId { get; set; }
        public string redirectUrl { get; set; }
        public string allowModify { get; set; }
        public string authScheme { get; set; }
        public string isMiniProgram { get; set; }
        public string authScope { get; set; }
        public Person person { get; set; }

        public Notice notice { get; set; }

        public class Person
        {
            public string backIdCardImgBase64 { get; set; }
            public string idCardImgBase64 { get; set; }
            public string identNo { get; set; }
            public string bankCardNo { get; set; }
            public string identType { get; set; }
            public string idPhotoOptional { get; set; }
            public string name { get; set; }
            public string mobile { get; set; }
        }

        public class Notice
        {
            public int notifyWay { get; set; }
            public string notifyAddress { get; set; }
        }
    }
}
