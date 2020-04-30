using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出完整路径(包括文件名)</param>
        /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        private static void ConvertPdf2Image(string pdfInputPath, string imageOutputPath,
             int startPageNum, int endPageNum, ImageFormat imageFormat, int definition)
        {

            PDFFile pdfFile = PDFFile.Open(pdfInputPath);

            if (startPageNum <= 0)
            {
                startPageNum = 1;
            }

            if (endPageNum > pdfFile.PageCount)
            {
                endPageNum = pdfFile.PageCount;
            }

            if (startPageNum > endPageNum)
            {
                int tempPageNum = startPageNum;
                startPageNum = endPageNum;
                endPageNum = startPageNum;
            }

            var bitMap = new Bitmap[endPageNum];

            for (int i = startPageNum; i <= endPageNum; i++)
            {
                Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * definition);
                Bitmap newPageImage = new Bitmap(pageImage.Width / 4, pageImage.Height / 4);

                Graphics g = Graphics.FromImage(newPageImage);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //重新画图的时候Y轴减去130，高度也减去130  这样水印就看不到了
                g.DrawImage(pageImage, new Rectangle(0, 0, pageImage.Width / 4, pageImage.Height / 4),
                    new Rectangle(0, 130, pageImage.Width, pageImage.Height - 130), GraphicsUnit.Pixel);

                bitMap[i - 1] = newPageImage;
                g.Dispose();
            }

            //合并图片
            var mergerImg = MergerImg(bitMap);
            //保存图片
            mergerImg.Save(imageOutputPath, imageFormat);
            pdfFile.Dispose();
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        private static Bitmap MergerImg(params Bitmap[] maps)
        {
            int i = maps.Length;

            if (i == 0)
                throw new Exception("图片数不能够为0");
            else if (i == 1)
                return maps[0];

            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap backgroudImg = new Bitmap(maps[0].Width, i * maps[0].Height);
            Graphics g = Graphics.FromImage(backgroudImg);
            //清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);
            for (int j = 0; j < i; j++)
            {
                g.DrawImage(maps[j], 0, j * maps[j].Height, maps[j].Width, maps[j].Height);
            }
            g.Dispose();
            return backgroudImg;
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
