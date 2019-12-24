using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class PDFHelper
    {
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
        /// <summary>
        /// 批量将图片转换为PDF文档的方法
        /// </summary>
        /// <param name="files">图片路径</param>
        /// <param name="newpdf">PDF生成路径</param>
        public static void ConvertJPG2PDF(string[] files, string newpdf)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
            if (files.Length > 0)
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(newpdf, FileMode.Create, FileAccess.ReadWrite));
                document.Open();
                iTextSharp.text.Image image;
                for (int i = 0; i < files.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(files[i])) break;
                    image = iTextSharp.text.Image.GetInstance(files[i]);
                    if (image.Height > iTextSharp.text.PageSize.A4.Height - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 25, iTextSharp.text.PageSize.A4.Height - 25);
                    }
                    else if (image.Width > iTextSharp.text.PageSize.A4.Width - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 25, iTextSharp.text.PageSize.A4.Height - 25);
                    }
                    image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                    //image.SetDpi(72, 72);
                    document.NewPage();
                    document.Add(image);
                }
            }
            document.Close();
        }
        /// <summary>
        /// 删除批量图片
        /// </summary>
        /// <param name="files">图片路径</param>
        /// <param name="imgtype">文件扩展名部分的字符串</param>
        public static void DeleteFile(string[] files, string imgtype)
        {
            if (files.Length > 0)
            {
                //删除图片
                foreach (var file in files)
                {
                    FileInfo f = new FileInfo(file);
                    if (f.Extension == "." + imgtype)
                        f.Delete();
                }
            }
        }
    }
}
