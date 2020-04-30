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

            //http://www.coozhi.com/xiuxianaihao/shuhuayinyue/76855.html
            string path = @"C:\Users\Administrator\Desktop\pdf\800479302_636495605353158628.pdf";
            string coutpath = @"F:\sample";
            string pathName = "";
            //PDFHelper.ConvertPDF2Image(path, coutpath, ImageFormat.Jpeg, PDFHelper.Definition.Three, out pathName);
            //PDFHelper.ConvertPDF2Image(path, coutpath, ImageFormat.Jpeg, PDFHelper.Definition.Two, out pathName);
            //string[] files = Directory.GetFiles(pathName);
            //PDFHelper.ConvertJPG2PDF(files, pathName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf");
            //PDFHelper.DeleteFile(files, ImageFormat.Jpeg.ToString());
            //string path = @"D:\保税仓库出口审批表_CDFI006005_20200309118601\";

            //string[] str = Directory.GetFiles(path, "*.pdf");
            //PDFmerge.mergePDFFiles(str, "qqqqq");
            //string pdfPath = @"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\Test\ConsoleApp1\asd\aaaa.pdf";
            //PDFmerge.MergePDF(str, pdfPath);

            #region 线程
            //MyLock myLock = new MyLock();
            //Thread thread1 = new Thread(myLock.Thread1Func);
            //thread1.Start();
            //Thread thread2 = new Thread(myLock.Thread2Func);
            //thread2.Start();
            #endregion

            #region MyRegion

            #endregion
            #region MyRegion
            //B b = new B();
            //A a = b;
            //a.M1();
            //a.M2();
            //b.M1();
            //b.M2();
            #endregion


            Console.WriteLine(ReturnValue2());
            Console.ReadLine();
        }
        public static int ReturnValue2()
        {
            MyInt x = new MyInt();
            x.MyValue = 3;
            MyInt y = new MyInt();
            y = x;
            y.MyValue = 4;
            return x.MyValue;
        }
        //递归写法
        public int foo(int n)
        {
            if (n < 2) return 1;
            else return foo(n - 2) + foo(n - 1);
        }
        private static int FibonacciSearch(int[] array, int key)
        {
            int length = array.Length;
            int low = 0, high = length - 1, mid, k = 0;
            mid = (low + high) / 2;
            while (mid < high)
            {
                if (array[mid] == key) { return mid; break; }
                else if (array[mid] > key) { high = mid; mid = (low + high) / 2; }
                else if (array[mid] < key) { low = mid; mid = (low + high) / 2; }
            }
            return mid;
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
        class A
        {
            int i = 1;
            public virtual void M1()
            {
                Console.WriteLine(" i am A ");
            }
            public void M2()
            {
                Console.WriteLine(" i am {0} ", i);
            }
        }
        class B : A
        {
            int i = 2;
            public override void M1()
            {
                Console.WriteLine(" i am B ");
            }
            public new void M2()
            {
                Console.WriteLine(" i am {0} ", i);
            }
        }
        public class MyInt
        {
            public int MyValue;
        }
    }
}
