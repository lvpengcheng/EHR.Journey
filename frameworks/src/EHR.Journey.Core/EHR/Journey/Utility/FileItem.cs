using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.Utility
{
    /// <summary>
    /// @author zhangyongliang
    /// @version 3.1.0
    /// @date 2020/01/27
    /// 处理文件数据
    /// </summary>
    public class FileItem
    {
        public string FileName { get; set; }

        public Stream Stream { get; set; }

        private byte[] content;

        public FileItem()
        {

        }
        /// <summary>
        /// 基于本地文件的构造器。
        /// </summary>
        /// <param name="fileInfo">本地文件</param>
        public FileItem(FileInfo fileInfo) : this(fileInfo.Name, fileInfo.OpenRead())
        {

        }

        /// <summary>
        /// 基于本地文件全路径的构造器。
        /// </summary>
        /// <param name="filePath">本地文件全路径</param>
        public FileItem(string filePath) : this(new FileInfo(filePath))
        {
        }

        /// <summary>
        /// 基于文件名和字节流的构造器。
        /// </summary>
        /// <param name="fileName">文件名称（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        public FileItem(string fileName, byte[] content)
        {
            this.FileName = fileName;
            this.content = content;
        }

        public FileItem(string filename, Stream stream)
        {
            this.FileName = filename;
            this.Stream = stream;
        }

        public byte[] GetContent()
        {
            if (this.content == null)
            {
                if (Stream.CanSeek) Stream.Seek(0, SeekOrigin.Begin);
                this.content = new byte[Stream.Length];
                Stream.Read(content, 0, content.Length);
            }

            return this.content;
        }

    }
}
