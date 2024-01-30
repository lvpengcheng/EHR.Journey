using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.Attributes
{
    public class RemoteServiceAttribute : Attribute
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求方式(POST GET PUT DEL)
        /// </summary>
        public string Method { get; set; }

        public RemoteServiceAttribute(string Url, string Method)
        {
            this.Url = Url;
            this.Method = Method;
        }
    }
}
