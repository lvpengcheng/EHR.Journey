using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels
{
   public class BaseResponse<T> where T : class, new()
    {
        public string RequestId { get; set; }

        public int code { get; set; }

        public string msg { get; set; }

        public T data { get; set; }
    }
}
