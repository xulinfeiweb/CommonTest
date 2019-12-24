using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// 压缩处理数据帮助类
    /// </summary>
    public class CompressionUtilHelper
    {
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void Compress(Stream source, Stream dest)
        {
            using (GZipStream zipStream = new GZipStream(dest, CompressionMode.Compress, true))
            {
                byte[] buf = new byte[1024];
                int len;
                while ((len = source.Read(buf, 0, buf.Length)) > 0)
                {
                    zipStream.Write(buf, 0, len);
                }
            }
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void Decompress(Stream source, Stream dest)
        {
            using (GZipStream zipStream = new GZipStream(source, CompressionMode.Decompress, true))
            {
                byte[] buf = new byte[1024];
                int len;
                while ((len = zipStream.Read(buf, 0, buf.Length)) > 0)
                {
                    dest.Write(buf, 0, len);
                }
            }
        }

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="Source"></param>
        public static byte[] Compress(byte[] Source)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress);

            gZipStream.Write(Source, 0, Source.Length);
            gZipStream.Close();

            return stream.ToArray();
        }

        ///// <summary>
        ///// 解压数据
        ///// </summary>
        ///// <param name="Source"></param>
        public static byte[] Decompress(byte[] Source)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(new MemoryStream(Source), CompressionMode.Decompress);

            byte[] b = new byte[4096];
            int count = 0;
            while (true)
            {
                int n = gZipStream.Read(b, 0, b.Length);

                if (n > 0)
                {
                    stream.Write(b, 0, n);
                    count += n;
                }
                else
                {
                    gZipStream.Close();
                    break;
                }
            }

            return stream.ToArray().Take(count).ToArray();
        }
    }
}
