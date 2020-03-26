using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            ////http://www.coozhi.com/xiuxianaihao/shuhuayinyue/76855.html
            //string path = @"C:\Users\Administrator\Desktop\pdf\800479302_636495605353158628.pdf";
            //string coutpath = @"F:\sample";
            //string pathName = "";
            //PDFHelper.ConvertPDF2Image(path, coutpath, ImageFormat.Jpeg, PDFHelper.Definition.Three, out pathName);
            //string[] files = Directory.GetFiles(pathName);
            //PDFHelper.ConvertJPG2PDF(files, pathName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf");
            //PDFHelper.DeleteFile(files, ImageFormat.Jpeg.ToString());
            //string path = @"D:\保税仓库出口审批表_CDFI006005_20200309118601\";

            //string[] str = Directory.GetFiles(path, "*.pdf");
            ////PDFmerge.mergePDFFiles(str, "qqqqq");
            //string pdfPath = @"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\Test\ConsoleApp1\asd\aaaa.pdf";
            //PDFmerge.MergePDF(str, pdfPath);


            MyLock myLock = new MyLock();
            Thread thread1 = new Thread(myLock.Thread1Func);
            thread1.Start();
            Thread thread2 = new Thread(myLock.Thread2Func);
            thread2.Start();



            Console.ReadLine();
        }
        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }

        class MyLock
        {
            int count = 0;
            object objA = new object();
            object objB = new object();
            public void Thread1Func()
            {
                for (int i = 0; i < 5; i++)
                    StartEatPear("线程1");
            }
            public void Thread2Func()
            {
                for (int i = 0; i < 5; i++)
                    StartEatPear("线程2");
            }
            public int StartEatPear(string str)
            {
                lock (objA)
                {
                    //在临界区中修改objA会导致锁失效
                    //objA = objB;
                    if (count < 100)
                    {
                        Console.WriteLine(str + "执行中，count加20前：" + count);
                        count = count + 20;
                        Console.WriteLine(str + "执行中，count加20后：" + count);
                    }
                    else
                    {
                        Console.WriteLine(str + "执行中，count将return：" + count);
                        return count;
                    }
                }
                return count;
            }


        }

    }
}
