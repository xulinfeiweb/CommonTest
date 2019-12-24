﻿using O2S.Components.PDFRender4NET;
using PdfiumViewer;
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
    class Program
    {
        static void Main(string[] args)
        {

            //http://www.coozhi.com/xiuxianaihao/shuhuayinyue/76855.html
            string path = @"C:\Users\Administrator\Desktop\pdf\800479302_636495605353158628.pdf";
            string coutpath = @"F:\sample";
            string pathName = "";
            PDFHelper.ConvertPDF2Image(path, coutpath, ImageFormat.Jpeg, PDFHelper.Definition.Three, out pathName);
            string[] files = Directory.GetFiles(pathName);
            PDFHelper.ConvertJPG2PDF(files, pathName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf");
            PDFHelper.DeleteFile(files, ImageFormat.Jpeg.ToString());
            Console.ReadLine();
        }
        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }


    }
}
