using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    /// <summary>
    /// word基类
    /// </summary>
    public class WordBase
    {
        /// <summary>
        /// Document
        /// </summary>
        protected Document _doc = null;
        
        /// <summary>
        /// 设置单元格内容对齐方式
        /// Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="doc"></param>
        /// <param name="Align"></param>
        /// <param name="Vertical"></param>
        /// <returns></returns>
        public Cell CreateCell(Document doc, string value, double size, bool bold, int Align, int Vertical, string fontName = "Arial")
        {
            Cell c1 = new Cell(doc);
            Paragraph p = new Paragraph(doc);
            Run run = new Run(doc, value);
            run.Font.Size = size;
            run.Font.Bold = bold;
            run.Font.Name = fontName;
            p.AppendChild(run);
            switch (Align)
            {
                case -1: p.ParagraphFormat.Alignment = ParagraphAlignment.Right; break;//左对齐
                case 0: p.ParagraphFormat.Alignment = ParagraphAlignment.Center; break;//水平居中
                case 1: p.ParagraphFormat.Alignment = ParagraphAlignment.Left; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: c1.CellFormat.VerticalAlignment = CellVerticalAlignment.Top; break;//顶端对齐
                case 0: c1.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; break;//垂直居中
                case 1: c1.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom; break;//底端对齐
            }
            c1.AppendChild(p);
            return c1;
        }
        /// <summary>
        /// 模版包含头部信息和表格，表格重复使用
        /// </summary>
        /// <param name="list">重复表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord(object tempFile,string saveFile,
            List<InvoiceModel> list,
            Dictionary<string, string> dic,
            Func<InvoiceModel, Document, Row> func1,
            Func<List<InvoiceModel>, Document, Row> func2
            )
        {
            if (!File.Exists(tempFile.ToString()))
            {
                return false;
            }
            try
            {
                _doc = new Document(tempFile.ToString());//读取模板
                //书签替换
                foreach (string name in dic.Keys)
                {
                    if (_doc.Range.Bookmarks[name] != null)
                    {
                        Bookmark mark = _doc.Range.Bookmarks[name];
                        mark.Text = dic[name];
                    }
                }
                #region 添加行
                Table table = (Table)_doc.GetChildNodes(NodeType.Table, true)[0]; //拿到表格
                foreach (InvoiceModel im in list)
                {
                    table.AppendChild(func1(im, _doc));
                }
                //统计
                table.AppendChild(func2(list, _doc));
                #endregion
                _doc.Save(saveFile.ToString());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// word转 PDF
        /// </summary>
        /// <param name="wordPath">word路径</param>
        /// <param name="savePath">PDF保存路径</param>
        /// <returns></returns>
        public bool WordToPDF(string wordPath, string savePath)
        {
            bool result = false;
            Document d = new Document(wordPath);
            try
            {
                d.Save(savePath, SaveFormat.Pdf);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
