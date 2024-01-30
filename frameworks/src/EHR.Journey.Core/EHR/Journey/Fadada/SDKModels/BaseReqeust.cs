using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels
{
   public class BaseReqeust<T> where T : class, new()
    {
        public BaseResponse<T> GetResponse()
        {
            return new BaseResponse<T>();
        }
    }
}
