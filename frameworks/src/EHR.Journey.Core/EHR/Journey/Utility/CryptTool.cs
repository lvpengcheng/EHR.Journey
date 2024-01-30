using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.Utility
{
    /// <summary>
    /// @author zhangyongliang
    /// @version 3.1.0
    /// @date 2020/01/26
    /// 加密算法
    /// </summary>
    public class CryptTool
    {
        public static string Base64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        /// <summary>
        /// 实现MD5消息摘要
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string md5(string str)
        {
            var strRes = Encoding.UTF8.GetBytes(str);
            using (var provider = new MD5CryptoServiceProvider())
            {
                strRes = provider.ComputeHash(strRes);
                var enText = new StringBuilder();
                foreach (byte iByte in strRes)
                {
                    enText.AppendFormat("{0:X2}", iByte);
                }
                return enText.ToString();
            }
        }
        /// <summary>
        /// 实现SHA-1消息摘要
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            var strRes = Encoding.UTF8.GetBytes(str);
            using (var provider = new SHA1CryptoServiceProvider())
            {
                strRes = provider.ComputeHash(strRes);
                var enText = new StringBuilder();
                foreach (byte iByte in strRes)
                {
                    enText.AppendFormat("{0:X2}", iByte);
                }
                return enText.ToString();
            }
        }
        /// <summary>
        /// 现SHA-256消息摘要
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string sha256(string str)
        {
            var strRes = Encoding.UTF8.GetBytes(str);
            using (var provider = new SHA256CryptoServiceProvider())
            {
                strRes = provider.ComputeHash(strRes);
                var enText = new StringBuilder();
                foreach (byte iByte in strRes)
                {
                    enText.AppendFormat("{0:X2}", iByte);
                }
                return enText.ToString();
            }
        }
        /// <summary>
        /// HMAC SHA256 (Base64)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string CreateSign(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        /// <summary>
        /// HMAC SHA256 (64位原始)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string HMACSHA256Str(string message, byte[] secret)
        {
            //secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            //byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(secret))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashmessage.Length; i++)
                {
                    builder.Append(hashmessage[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// HMAC SHA256 (64位原始)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static byte[] HMACSHA256Byte(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return hashmessage;
            }
        }

        /// <summary> 
        /// 计算文件的哈希值 
        /// </summary> 
        /// <param name="fileName">要计算哈希值的文件名和路径</param> 
        /// <param name="algName">算法:sha1,md5</param> 
        /// <returns>哈希值16进制字符串</returns> 
        public static string HashFile(string fileName, string algName)
        {
            if (!System.IO.File.Exists(fileName))
                return string.Empty;

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            fs.Dispose();
            return ByteArrayToHexString(hashBytes);
        }

        public static string HashDataString(Stream stream, string algName)
        {
            byte[] hashBytes = HashData(stream, algName);
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary> 
        /// 计算哈希值 
        /// </summary> 
        /// <param name="stream">要计算哈希值的 Stream</param> 
        /// <param name="algName">算法:sha1,md5</param> 
        /// <returns>哈希值字节数组</returns> 
        public static byte[] HashData(Stream stream, string algName)
        {
            HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "sha256", true) == 0)
            {
                algorithm = SHA256.Create();
            }
            else if (string.Compare(algName, "sm3", true) == 0)
            {
                algorithm = SM3.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName 只能使用 sha256 或 md5");
                }
                algorithm = MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary> 
        /// 字节数组转换为16进制表示的字符串 
        /// </summary> 
        public static string ByteArrayToHexString(byte[] buf)
        {
            string returnStr = "";
            if (buf != null)
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    returnStr += buf[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public static byte[] FileToStream(string fileName)
        {
            //文件下载地址
            if (fileName.Contains("https"))
            {
                using (WebClient client = new WebClient())
                {
                    //把下载后的文件转化为 byte[]
                    byte[] bytesDownload = client.DownloadData(fileName);
                    // 把 byte[] 转换成 Stream
                    //Stream streamDownload = new MemoryStream(bytesDownload);
                    return bytesDownload;
                }
            }
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            //Stream stream = new MemoryStream(bytes);
            return bytes;
        }
    }
}
