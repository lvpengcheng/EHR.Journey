using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels
{
   public class AccessTokenResponse
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string accessToken { get; set; }

        /// <summary>
        /// 有效截⽌时间（毫秒）
        /// </summary>
        public long expiresIn { get; set; }

        /// <summary>
        /// 有效截⽌时间(yyyy-MM-dd HH:mm:ss.sss)
        /// </summary>
        public DateTime expiresTime { get; set; }
    }
}
