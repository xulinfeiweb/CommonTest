using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PDFHelper
    {
        public PDFHelper(string pdfInputPath, string imageOutputPath, string ift, int def, out string pathName)
        {
            PDFFile pdfFile = PDFFile.Open(pdfInputPath);
            pathName = imageOutputPath + "\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "\\";
            Assembly asm = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/dll/System.Drawing.dll");
            Type t = asm.GetType("System.Drawing.Bitmap");//获取类名，必须 命名空间+类名  
            object o = Activator.CreateInstance(t);
            for (int i = 0; i < pdfFile.PageCount; i++)
            {
                o = pdfFile.GetPageImage(i, 56 * (int)def);
                //创建目标路径
                Directory.CreateDirectory(pathName);
                o.Save(pathName + i.ToString() + "." + ift.ToString(), ift);
                o.Dispose();
            }
        }
        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }
        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        /// <param name="ift">设置所需图片格式</param>
        public static void ConvertPDF2Image(string pdfInputPath, string imageOutputPath, ImageFormat ift, Definition def, out string pathName)
        {
            PDFFile pdfFile = PDFFile.Open(pdfInputPath);
            pathName = imageOutputPath + "\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "\\";

            for (int i = 0; i < pdfFile.PageCount; i++)
            {
                Bitmap pageImage = pdfFile.GetPageImage(i, 56 * (int)def);
                //创建目标路径
                Directory.CreateDirectory(pathName);
                pageImage.Save(pathName + i.ToString() + "." + ift.ToString(), ift);
                pageImage.Dispose();
            }
            pdfFile.Dispose();
        }
    }
}
