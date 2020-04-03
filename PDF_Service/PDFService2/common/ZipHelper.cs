using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class ZipsHelper
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationZipFilePath"></param>
        /// <param name="level"></param>
        /// <param name="searchPattern"></param>
        public static void CreateZip(string sourceFilePath, string destinationZipFilePath, int level = 6, string searchPattern = "")
        {
            ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
            zipStream.SetLevel(level);  // 压缩级别 0-9
            CreateZipFiles(sourceFilePath, zipStream, searchPattern, sourceFilePath);
            zipStream.Finish();
            zipStream.Close();
        }
        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream, string searchPattern, string staticFile)
        {
            Crc32 crc = new Crc32();
            IList<string> filesArray = null;
            if (string.IsNullOrWhiteSpace(searchPattern))
                filesArray = Directory.GetFileSystemEntries(sourceFilePath).Where(p => p.EndsWith(".pdf") || p.EndsWith(".doc")).ToList();
            else
                filesArray = Directory.GetFileSystemEntries(sourceFilePath, searchPattern);
            foreach (string file in filesArray)
            {
                //如果当前是文件夹，递归
                if (Directory.Exists(file))
                {
                    CreateZipFiles(file, zipStream, searchPattern, staticFile);
                }
                else
                {
                    //如果是文件，开始压缩
                    FileStream fileStream = File.OpenRead(file);

                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    string tempFile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);

                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
