using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Accounts
{
   public class GetPersonInfoResponse
    {
        public string unionId { get; set; }
        public string status { get; set; }
        public Person person { get; set; }
        public ImageInfo imageInfo { get; set; }
        public class Person
        {
            public string name { get; set; }
            public string idCard { get; set; }
            public string mobile { get; set; }
            public string certType { get; set; }
            public string bankCardNo { get; set; }
            public int verifyType { get; set; }
            public string auditorTime { get; set; }
            public string liveRate { get; set; }
            public string similarity { get; set; }
        }
        public class ImageInfo
        {
            public string headPhotoPath { get; set; }
            public string backgroundIdCardPath { get; set; }
            public string photoUuid { get; set; }
            public string gesturesPhotoPath { get; set; }
        }
    }
}
