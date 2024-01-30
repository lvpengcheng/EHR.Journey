
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace FDD.OpenAPI.Utility
{
    /// <summary>
    /// @author zhangyongliang
    /// @version 3.1.0
    /// @date 2020/01/26
    /// </summary>
    public class HttpUrl
    {
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = HttpUtility.UrlEncode(str);
            }
            return str;
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = HttpUtility.UrlDecode(str);
            }
            return str;
        }

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="keys">参数名称</param>
        /// <param name="values">参数值</param>
        /// <returns>String</returns>
        public static string BuildRequestUrl(string url, string keys, params string[] values)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keys))
            {
                string[] keyArr = keys.Split(',');
                int pageLength = url.IndexOf('?');
                NameValueCollection oldUrlParams = null;
                if (pageLength >= 0)
                {
                    sb.Append(url.Substring(0, pageLength));
                    oldUrlParams = HttpUtility.ParseQueryString(url.Substring(pageLength));
                }
                else
                {
                    sb.Append(url);
                    oldUrlParams = new NameValueCollection();
                }
                for (int i = 0; i < keyArr.Length; i++)
                {
                    oldUrlParams[keyArr[i]] = values[i];
                }
                sb.Append("?");
                foreach (string p in oldUrlParams)
                {
                    var s = p + "=" + UrlEncode(oldUrlParams[p]) + "&";
                    sb.Append(s);
                }
                sb.Remove(sb.Length - 1, 1);
                url = sb.ToString();
            }
            return url;
        }


        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数映射</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                int pageLength = url.IndexOf('?');
                StringBuilder sb = new StringBuilder();
                NameValueCollection oldUrlParams = null;
                if (pageLength >= 0)
                {
                    sb.Append(url.Substring(0, pageLength));
                    oldUrlParams = HttpUtility.ParseQueryString(url.Substring(pageLength));
                }
                else
                {
                    sb.Append(url);
                    oldUrlParams = new NameValueCollection();
                }
                foreach (var p in parameters)
                {
                    oldUrlParams[p.Key] = p.Value;
                }
                sb.Append("?");
                foreach (string p in oldUrlParams)
                {
                    var s = p + "=" + UrlEncode(oldUrlParams[p]) + "&";
                    sb.Append(s);
                }
                sb.Remove(sb.Length - 1, 1);
                url = sb.ToString();
            }
            return url;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null || parameters.Count() == 0)
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            bool hasParam = false;

            foreach (KeyValuePair<string, string> kv in parameters)
            {
                string name = kv.Key;
                string value = kv.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))
                {
                    if (hasParam)
                    {
                        query.Append("&");
                    }
                    query.Append(name + "=" + UrlEncode(value));
                    hasParam = true;
                }
            }
            return query.ToString();
        }


        /// <summary>
        /// 解析查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParamQuery(string query)
        {
            Dictionary<string, string> ps = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(query))
            {
                var arr = query.Split('&');
                foreach (var a in arr)
                {
                    var keys = a.Split('=');
                    var key = keys[0];
                    string value = null;
                    if (keys.Length > 1) value = keys[1];
                    ps.Add(key, value);
                }
            }
            return ps;
        }
    }
}
