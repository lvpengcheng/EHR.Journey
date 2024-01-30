using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using FDD.OpenAPI.Attributes;
using FDD.OpenAPI.SDKModels;
using FDD.OpenAPI.Utility;
using Newtonsoft.Json;

namespace FDD.OpenAPI
{
    /// <summary>
    /// @author zhangyongliang
    /// @version 3.1.0
    /// @date 2020/01/25
    /// 说明：开放版客户端
    /// </summary>
    public class OpenClient
    {
        public string ServerUrl { get; private set; }

        public string AppId { get; private set; }

        public string AppKey { get; private set; }
        public OpenClient(string serverUrl, string appId, string appKey)
        {
            this.ServerUrl = serverUrl;
            this.AppId = appId;
            this.AppKey = appKey;
        }
        public BaseResponse<T> Execute<T>(BaseReqeust<T> req) where T : class, new()
        {
            var reqStr = JsonConvert.SerializeObject(req, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var urlAttribute = GetUrlAttribute(req);
            var url = ServerUrl + urlAttribute.Url;
            BaseResponse<T> rspModel = null;
            try
            {
                var body = new Dictionary<string, string>() { { "bizContent", reqStr } };
                var files = GetRequestFiles(req);
                var client = HttpWebHelp.CreateDefault(url);
                var token = GetToken();
                SetRequestHeaders(client, token.accessToken, body);
                client.Method = urlAttribute.Method;
                client.ContentType = "multipart/form-data";
                client.SubmitFormData(body, files);
                var rsp = client.GetResponseIgnoreServerError();
                string rspStr = null;
                if (typeof(FileItem).IsAssignableFrom(typeof(T)))
                {
                    if (rsp.ContentType.StartsWith("application/json"))
                    {
                        rspStr = rsp.GetResponseString();
                        rspModel = JsonConvert.DeserializeObject<BaseResponse<T>>(rspStr);
                        rspModel.RequestId = rsp.Headers["X-FDD-Request-Id"];
                    }
                    else
                    {
                        var file = new T() as FileItem;
                        file.Stream = new MemoryStream();  //此处可根据request配置，直接传递filestream，直接就下载到本地存储，可以不用先下载到内存。
                        rsp.GetResponseFileItem(file);
                        rspModel = new BaseResponse<T>()
                        {
                            code = 100000,
                            msg = "文件下载成功！",
                            data = file as T
                        };
                    }
                }
                else
                {
                    rspStr = rsp.GetResponseString();
                    rspModel = JsonConvert.DeserializeObject<BaseResponse<T>>(rspStr);
                    rspModel.RequestId = rsp.Headers["X-FDD-Request-Id"];
                }
            }
            catch (Exception e)
            {
                rspModel = new BaseResponse<T>()
                {
                    code = -1,
                    msg = "Client：" + e.Message
                };
            }
            return rspModel;
        }

        private void SetRequestHeaders(HttpWebRequest client, string token, Dictionary<string, string> body)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); //时间戳         
            IDictionary<string, string> headerParames = new Dictionary<string, string>();//参数摘要计算得map
            headerParames.Add("X-FDD-Api-App-Id", AppId);
            headerParames.Add("X-FDD-Api-Sign-Type", "HMAC-SHA256");
            headerParames.Add("X-FDD-Api-Timestamp", timestamp);
            headerParames.Add("X-FDD-Api-Nonce", Guid.NewGuid().ToString("N"));
            headerParames.Add("X-FDD-Api-Token", token);
            headerParames.Add("X-FDD-Api-Sign", Utility.SignUtil.GetSign(headerParames.Union(body), timestamp, AppKey));

            foreach (var p in headerParames)
            {
                client.Headers[p.Key] = p.Value;
            }
        }

        static ConcurrentDictionary<string, AccessTokenResponse> tokens = new ConcurrentDictionary<string, AccessTokenResponse>();
        public AccessTokenResponse GetToken()
        {
            AccessTokenResponse token;
            if (tokens.TryGetValue(AppId, out token) == false || token == null)
            {
                token = GetTokenFromServer();
                tokens[AppId] = token;
            }
            else if ((token.expiresTime - DateTime.Now).TotalMinutes < 3)
            {
                token = GetTokenFromServer();
                tokens[AppId] = token;
            }
            return token;
        }

        public AccessTokenResponse GetTokenFromServer()
        {
            string nonce = Guid.NewGuid().ToString("N");  //随机数            
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); //时间戳            

            IDictionary<string, string> headerParames = new Dictionary<string, string>();//参数摘要计算得map
            headerParames.Add("X-FDD-Api-App-Id", AppId);
            headerParames.Add("X-FDD-Api-Sign-Type", "HMAC-SHA256");
            headerParames.Add("X-FDD-Api-Timestamp", timestamp);
            headerParames.Add("X-FDD-Api-Nonce", nonce);
            headerParames.Add("X-FDD-Api-Grant-Type", "client_credential");
            headerParames.Add("X-FDD-Api-Sign", Utility.SignUtil.GetSign(headerParames, timestamp, AppKey));

            var client = HttpWebHelp.CreateDefault(ServerUrl + "/oauth2/accessToken");
            foreach (var p in headerParames)
            {
                client.Headers[p.Key] = p.Value;
            }
            client.Method = "POST";
            client.ContentType = "application/x-www-form-data";
            var rspResponse = client.GetResponseIgnoreServerError();
            var rspStr = rspResponse.GetResponseString();
            var rspToken = JsonConvert.DeserializeObject<BaseResponse<AccessTokenResponse>>(rspStr);
            rspToken.RequestId = rspResponse.Headers["X-FDD-Request-Id"];
            return rspToken.data;
        }

        private RemoteServiceAttribute GetUrlAttribute<T>(BaseReqeust<T> req) where T : class, new()
        {
            var urlAttribute = req.GetType().GetCustomAttributes(false).First(r => r is RemoteServiceAttribute) as RemoteServiceAttribute;
            return urlAttribute;
        }

        private IDictionary<string, FileItem> GetRequestFiles<T>(BaseReqeust<T> req) where T : class, new()
        {
            IDictionary<string, FileItem> files = null;
            var filePropertys = req.GetType().GetProperties().Where(r => r.PropertyType == typeof(FileItem));
            if (filePropertys.Count() > 0)
            {
                files = new Dictionary<string, FileItem>();
                foreach (var property in filePropertys)
                {
                    var file = property.GetValue(req, null) as FileItem;
                    files.Add(property.Name, file);
                }
            }
            return files;
        }
    }
}
