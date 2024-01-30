using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Cache;
using System.Net.Security;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace FDD.OpenAPI.Utility
{
    /// <summary>
    /// @author zhangyongliang
    /// @version 3.1.0
    /// @date 2020/01/26
    /// </summary>
    public static class HttpWebHelp
    {
        public static Encoding DefaultEncode = Encoding.UTF8;

        public static int ReadBuffSize = 4096;

        public static HttpWebRequest CreateDefault(string url)
        {
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                SetHttpsSecurityProtocol();
            }
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.SetDefault();
            return req;
        }

        public static void SetHttpsSecurityProtocol()
        {
            //HTTPS无视证书验证
            var callback = new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => { return true; });
            ServicePointManager.ServerCertificateValidationCallback = callback;
            ServicePointManager.SecurityProtocol= SecurityProtocolType.Tls |
                    SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls| SecurityProtocolType.Tls11|SecurityProtocolType.Tls12|SecurityProtocolType.Tls13 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
        }

        public static HttpWebRequest SetDefault(this HttpWebRequest req)
        {
            req.Proxy = WebRequest.GetSystemWebProxy();
            req.Timeout = 20000;
            req.ReadWriteTimeout = 60000;
            req.KeepAlive = false;
            req.AllowAutoRedirect = true;
            req.MaximumAutomaticRedirections = 32;
            req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            req.Accept = "*/*";
            req.Headers["Accept-Encoding"] = "gzip,deflate";
            return req;
        }

        /// <summary>
        /// 获取RequestStream并自动压缩
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Stream GetRequestStreamAutoCompress(this HttpWebRequest req)
        {
            var ContentEncoding = req.Headers["Content-Encoding"];
            var stream = req.GetRequestStream();
            if ("gzip".Equals(ContentEncoding, StringComparison.OrdinalIgnoreCase))
            {
                stream = new GZipStream(stream, CompressionMode.Compress);
            }
            else if ("deflate".Equals(ContentEncoding, StringComparison.OrdinalIgnoreCase))
            {
                stream = new DeflateStream(stream, CompressionMode.Compress);
            }
            return stream;
        }

        public static HttpWebRequest SubmitFormData(this HttpWebRequest req, object postParams, IDictionary<string, FileItem> files)
        {
            return SubmitFormData(req, postParams, files, DefaultEncode);
        }

        /// <returns></returns>
        public static HttpWebRequest SubmitFormData(this HttpWebRequest req, object postParams, IDictionary<string, FileItem> files, Encoding encode)
        {
            if (postParams == null && files == null) return req;
            using (System.IO.Stream reqStream = GetRequestStreamAutoCompress(req))
            {
                if (req.ContentType.StartsWith("multipart/form-data"))
                {
                    string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线            
                    req.ContentType = "multipart/form-data;charset=" + encode.WebName + ";boundary=" + boundary;
                    byte[] itemBoundaryBytes = encode.GetBytes("\r\n--" + boundary + "\r\n");
                    byte[] endBoundaryBytes = encode.GetBytes("\r\n--" + boundary + "--\r\n");

                    #region 写入字段
                    if (postParams != null)
                    {
                        if (postParams is IDictionary<string, string>)
                        {
                            // 组装文本请求参数
                            string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
                            foreach (var kv in postParams as Dictionary<string, string>)
                            {
                                string textEntry = string.Format(textTemplate, kv.Key, kv.Value);
                                byte[] itemBytes = encode.GetBytes(textEntry);
                                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                                reqStream.Write(itemBytes, 0, itemBytes.Length);
                            }
                        }
                        else if (postParams is string)
                        {
                            // 组装文本请求参数
                            string txt = "Content-Disposition:form-data;" + postParams as string;
                            byte[] itemBytes = encode.GetBytes(txt);
                            reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                            reqStream.Write(itemBytes, 0, itemBytes.Length);
                        }
                        else
                        {
                            // 组装文本请求参数
                            string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
                            foreach (var kv in getParames(postParams))
                            {
                                string textEntry = string.Format(textTemplate, kv.Key, kv.Value);
                                byte[] itemBytes = encode.GetBytes(textEntry);
                                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                                reqStream.Write(itemBytes, 0, itemBytes.Length);
                            }
                        }
                    }
                    #endregion

                    #region 写入文件
                    if (files != null)
                    {
                        // 组装文件请求参数
                        string fileTemplate = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
                        foreach (var kv in files)
                        {
                            string key = kv.Key;
                            FileItem fileItem = kv.Value;
                            string fileEntry = string.Format(fileTemplate, key, fileItem.FileName, "");
                            byte[] itemBytes = encode.GetBytes(fileEntry);
                            reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                            reqStream.Write(itemBytes, 0, itemBytes.Length);
                            byte[] fileBytes = fileItem.GetContent();
                            reqStream.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }
                    #endregion

                    reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                }
                else
                {
                    byte[] datas;
                    if (postParams is IDictionary<string, string>)
                    {
                        datas = encode.GetBytes(HttpUrl.BuildQuery(postParams as Dictionary<string, string>));
                    }
                    else if (postParams is string)
                    {
                        datas = encode.GetBytes(postParams as string);
                    }
                    else if (postParams is byte[])
                    {
                        datas = postParams as byte[];
                    }
                    else
                    {
                        datas = encode.GetBytes(HttpUrl.BuildQuery(getParames(postParams)));
                    }
                    reqStream.Write(datas, 0, datas.Length);
                }
            }
            return req;
        }

        /// <summary>
        /// 获取Response，忽略HttpCode异常
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        public static HttpWebResponse GetResponseIgnoreServerError(this HttpWebRequest req)
        {
            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException we)
            {
                rsp = (HttpWebResponse)we.Response;
                if (rsp == null) throw we;
            }
            catch
            {
                throw;
            }
            return rsp;
        }

        /// <summary>
        /// 获取ResponseStream并自动解码
        /// </summary>
        /// <param name="rsp"></param>
        /// <returns></returns>
        public static Stream GetResponseStreamAutoDecompress(this HttpWebResponse rsp)
        {
            var stream = rsp.GetResponseStream();
            if ("gzip".Equals(rsp.ContentEncoding, StringComparison.OrdinalIgnoreCase))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            else if ("deflate".Equals(rsp.ContentEncoding, StringComparison.OrdinalIgnoreCase))
            {
                stream = new DeflateStream(stream, CompressionMode.Decompress);
            }
            return stream;
        }

        /// <summary>
        /// 获取返回数据
        /// </summary>
        /// <param name="rsp"></param>
        /// <returns></returns>
        public static string GetResponseString(this HttpWebResponse rsp)
        {
            return GetResponseString(rsp, GetResponseEncoding(rsp));
        }

        /// <summary>
        /// 获取返回数据
        /// </summary>
        public static string GetResponseString(this HttpWebResponse rsp, Encoding encode)
        {
            using (var stream = GetResponseStreamAutoDecompress(rsp))
            {
                using (var reader = new StreamReader(stream, encode))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 获取返回的文件
        /// </summary>
        public static void GetResponseFile(this HttpWebResponse rsp, Stream savestream)
        {
            using (var rspstream = GetResponseStreamAutoDecompress(rsp))
            {
                var buff = new byte[ReadBuffSize];
                var readcount = 0;
                do
                {
                    readcount = rspstream.Read(buff, 0, ReadBuffSize);
                    savestream.Write(buff, 0, readcount);
                } while (readcount > 0);
            }
        }

        public static FileItem GetResponseFileItem(this HttpWebResponse rsp)
        {
            FileItem fileItem = new FileItem("", new MemoryStream());
            GetResponseFileItem(rsp, fileItem);
            return fileItem;
        }

        public static void GetResponseFileItem(this HttpWebResponse rsp, FileItem fileItem)
        {
            using (var rspstream = GetResponseStreamAutoDecompress(rsp))
            {
                string fileName = rsp.Headers["Content-Disposition"];
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = rsp.ResponseUri.Segments[rsp.ResponseUri.Segments.Length - 1];
                }
                else
                {
                    fileName = fileName.Remove(0, fileName.IndexOf("filename=") + 9);
                }
                fileItem.FileName = fileName;
                var buff = new byte[ReadBuffSize];
                var readcount = 0;
                do
                {
                    readcount = rspstream.Read(buff, 0, ReadBuffSize);
                    fileItem.Stream.Write(buff, 0, readcount);
                } while (readcount > 0);
            }
        }

        public static byte[] GetResponseFile(this HttpWebResponse rsp)
        {
            using (var rspstream = GetResponseStreamAutoDecompress(rsp))
            {
                using (var savestream = new MemoryStream(Convert.ToInt32(rsp.ContentLength)))
                {
                    var buff = new byte[ReadBuffSize];
                    var readcount = 0;
                    do
                    {
                        readcount = rspstream.Read(buff, 0, ReadBuffSize);
                        savestream.Write(buff, 0, readcount);
                    } while (readcount > 0);
                    return savestream.GetBuffer();
                }
            }
        }


        public static Encoding GetResponseEncoding(this HttpWebResponse rsp)
        {
            string charset = rsp.CharacterSet;
            if (string.IsNullOrEmpty(charset))
            {
                return DefaultEncode;
            }
            else
            {
                return Encoding.GetEncoding(charset);
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> getParames(object param)
        {
            foreach (var p in param.GetType().GetProperties())
            {
                var value = p.GetValue(param, null);
                if (value != null) yield return new KeyValuePair<string, string>(p.Name, value.ToString());
            }
        }
    }
}
