using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PdfHelper
    {
        /// <summary>
        /// 读取合并的pdf文件名称
        /// </summary>
        /// <param name="Directorypath">目录</param>
        /// <param name="outpath">导出的路径</param>
        public static void MergePDF(string[] files, string outpath)
        {
            //foreach (string item in files)
            //{
            //    FileInfo f = new FileInfo(item);
            //    if (f.Name == "发票.pdf" || f.Name == "出库清单.pdf")
            //    {

            //    }
            //}
            string[] fles = { "D:\\保税仓库出口审批表_CDFI006005_20200309118601\\发票.pdf", "D:\\保税仓库出口审批表_CDFI006005_20200309118601\\出库清单.pdf" };
            mergePDFFiles(fles, outpath);
            DeleteAllPdf(fles, ".pdf");
        }


        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="arr">文件名数组</param>
        public static void BubbleSort(FileInfo[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i; j < arr.Length; j++)
                {
                    if (arr[i].LastWriteTime > arr[j].LastWriteTime)//按创建时间（升序）
                    {
                        FileInfo temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 合成pdf文件
        /// </summary>
        /// <param name="fileList">文件名list</param>
        /// <param name="outMergeFile">输出路径</param>
        public static void mergePDFFiles(string[] fileList, string outMergeFile)
        {
            try
            {
                List<PdfReader> readerList = new List<PdfReader>();
                Rectangle rl = PageSize.A4.Rotate();
                Document document = new Document(rl, 20, 20, 0, 0);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outMergeFile, FileMode.Create));
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                for (int i = 0; i < fileList.Length; i++)
                {
                    PdfReader reader = new PdfReader(fileList[i]);
                    readerList.Add(reader);
                    int iPageNum = reader.NumberOfPages;
                    for (int j = 1; j <= iPageNum; j++)
                    {
                        var newPageSize = new Rectangle(rl.Width, rl.Height);
                        document.SetPageSize(newPageSize); //这句话使得每页都跟原纸张大小一致.
                        document.NewPage();
                        //获取指定页面的旋转度
                        int rotation = reader.GetPageRotation(j);
                        PdfImportedPage newPage = writer.GetImportedPage(reader, j);

                        //添加内容页到新的页面，并更加旋转度设置对应的旋转
                        switch (rotation)
                        {
                            case 90:
                                cb.AddTemplate(newPage, 0, -1, 1, 0, 0, reader.GetPageSizeWithRotation(j).Height);
                                break;
                            case 180:
                                cb.AddTemplate(newPage, -1, 0, 0, -1, reader.GetPageSizeWithRotation(j).Width, reader.GetPageSizeWithRotation(j).Height);
                                break;
                            case 270:
                                cb.AddTemplate(newPage, 0, 1, -1, 0, reader.GetPageSizeWithRotation(j).Width, 0);
                                break;
                            default:
                                cb.AddTemplate(newPage, 1, 0, 0, 1, 0, 0);//等同于 cb.AddTemplate(page1, 0,0)
                                break;
                        }
                    }
                }
                document.Close();
                foreach (PdfReader item in readerList)
                {
                    item.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }
        /// <summary>
        /// 删除一个文件里所有的文件
        /// </summary>
        /// <param name="Directorypath">文件夹路径</param>
        public static void DeleteAllPdf(string[] fileList, string imgtype)
        {
            if (fileList.Length > 0)
            {
                //删除图片
                foreach (var file in fileList)
                {
                    FileInfo f = new FileInfo(file);
                    if (f.Extension == imgtype)
                        f.Delete();
                }
            }
        }
    }
}
