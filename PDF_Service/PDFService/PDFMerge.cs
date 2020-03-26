using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class PDFMerge
    {
        /// <summary>
        /// 合成pdf文件
        /// </summary>
        /// <param name="fileList">文件名list</param>
        /// <param name="outMergeFile">输出路径</param>
        public static void mergePDFFiles(string[] fileList, string outMergeFile)
        {
            try
            {
                List<PdfReader> prList = new List<PdfReader>();
                Rectangle rl = PageSize.A4.Rotate();
                Document doc = new Document(rl, 20, 20, 0, 0);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outMergeFile, FileMode.Create));
                doc.Open();
                writer.PageEvent = new PDFMergePdfPageEventHelper();//页脚
                PdfContentByte cb = writer.DirectContent;
                for (int i = 0; i < fileList.Length; i++)
                {
                    PdfReader reader = new PdfReader(fileList[i]);
                    prList.Add(reader);
                    int iPageNum = reader.NumberOfPages;
                    for (int j = 1; j <= iPageNum; j++)
                    {
                        doc.SetPageSize(new Rectangle(rl)); //这句话使得每页都跟原纸张大小一致.
                        doc.NewPage();
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
                doc.Close();
                foreach (PdfReader item in prList)
                {
                    item.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
