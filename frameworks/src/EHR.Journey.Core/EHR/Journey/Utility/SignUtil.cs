using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.Utility
{
    /// <summary>
    /// @author zhangyongliang
    /// @version 3.0.1
    /// @date 2020/01/25
    /// 签名计算
    /// </summary>
    public class SignUtil
    {
        /// <summary>
        ///  生成签名
        /// </summary>
        /// <param name="input"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static String GetSign(IEnumerable<KeyValuePair<string, string>> dic, string timestamp, string appkey)
        {
            string sign = null;
            dic = dic.Where(r => string.IsNullOrEmpty(r.Value) == false).OrderBy(x => x.Key, new OrdinalComparer()).ToDictionary(x => x.Key, y => y.Value);
            var content = string.Join("&", dic.Select(r => r.Key + "=" + r.Value));
            string signText = CryptTool.sha256(content).ToLower();
            byte[] secretSign = CryptTool.HMACSHA256Byte(timestamp, appkey);
            sign = CryptTool.HMACSHA256Str(signText.ToLower(), secretSign);
            return sign;
        }
    }
}
