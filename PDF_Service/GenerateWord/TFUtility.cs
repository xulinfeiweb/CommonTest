using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Words;
using System.Threading.Tasks;
using Aspose.Words.Tables;
using System.Data;
using System.Windows.Forms;
using System.Web.Services.Description;

namespace PDF_Service.GenerateWord
{
    public class TFUtility
    {
        /// <summary>
        /// AsPose.Word's Export
        /// </summary>
        /// <returns></returns>
        public static string AsPoseExportData(Dictionary<string, string> param)
        {
            //Report Template'work Path
            string path = Path.Combine("");
            Document doc = new Document(path);
            DocumentBuilder builder = new DocumentBuilder(doc);


            builder.MoveToCell(0, 0, 1, 0);
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom;
            builder.Write(param["ReportName"].ToString());
            builder.MoveToDocumentEnd();


            //Insert Empty Rows
            builder.StartTable();
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            for (int j = 0; j < 1; j++)
            {
                builder.InsertCell();
                builder.EndRow();
            }
            builder.EndTable();
            builder.MoveToDocumentEnd();


            //Insert Table Head
            builder.StartTable();
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            for (int j = 0; j < 2; j++)
            {
                if (j == 0)
                {
                    builder.InsertCell();
                    builder.Bold = true;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write("EventNo:");
                    builder.InsertCell();
                    builder.Bold = false;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(param["EventNo"].ToString());
                    builder.InsertCell();
                    builder.Write("");
                    builder.InsertCell();
                    builder.Write("");
                }
                else if (j == 1)
                {
                    builder.InsertCell();
                    builder.Bold = true;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write("Venue:");
                    builder.InsertCell();
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Bold = false;
                    builder.Write(param["Venue"].ToString());
                    builder.InsertCell();
                    builder.Bold = true;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                    builder.Write("Meeting Date:");
                    builder.InsertCell();
                    builder.Bold = false;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(Convert.ToDateTime(param["ActivityDate"]).ToString("yyyy-MM-dd"));
                }
                builder.EndRow();
            }
            builder.EndTable();
            //Insert Other table
            builder.MoveToDocumentEnd();


            //Insert Empty Rows
            builder.StartTable();
            builder.CellFormat.Borders.LineStyle = Aspose.Words.LineStyle.None;
            for (int j = 0; j < 1; j++)
            {
                builder.InsertCell();
                builder.EndRow();
            }
            builder.EndTable();
            builder.MoveToDocumentEnd();


            //Data Table Start 
            builder.StartTable();
            builder.CellFormat.Borders.LineStyle = Aspose.Words.LineStyle.Inset;
            builder.Bold = true;
            DataTable dtAttendee = new DataTable();
            int dtAttendeeCnt = dtAttendee.Rows.Count;
            int columncnt = 5;


            //Insert Table Data
            for (int i = 0; i < dtAttendeeCnt; i++)
            {
                if (i == 0)
                {
                    //Create Table Head Data
                    for (int j = 0; j < columncnt; j++)
                    {
                        builder.Bold = true;
                        if (j == 0)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                            builder.Write("Name");
                        }
                        else if (j == 1)
                        {
                            builder.InsertCell();
                            builder.Write("Title");
                        }
                        else if (j == 2)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                            builder.Write("Hospitals&Speciaty/Organ");
                        }
                        else if (j == 3)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                            builder.Write("Signature");
                        }
                        else if (j == 4)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                            builder.Write("Remarks");
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < columncnt; j++)
                    {
                        builder.InsertCell();
                        if (j == 2)
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                        }
                        else
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        }
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                        builder.Bold = false;
                        builder.Font.Size = 10;
                        builder.Write(dtAttendee.Rows[i][j].ToString());
                    }
                }
                builder.EndRow();
            }
            builder.EndTable();
            //Save output document
            string filename = Path.Combine("");
            doc.Save(filename);
            return filename;
        }


        public bool NCRDocDic(string TemPath, string savePath, List<InvoiceModel> list, Dictionary<string, string> dic)
        {

            if (!File.Exists(TemPath.ToString()))
            {
                MessageBox.Show(string.Format("{0}模版文件不存在，请先设置模版文件。", TemPath.ToString()));
                return false;
            }
            string templateFile = TemPath;
            Document doc = new Document(templateFile);//读取模板
            DocumentBuilder builder = new DocumentBuilder(doc);
            try
            {
                //使用文本方式替换
                foreach (string name in dic.Keys)
                {
                    if (doc.Range.Bookmarks[name] != null)
                    {
                        Bookmark mark = doc.Range.Bookmarks[name];
                        mark.Text = dic[name];
                    }
                }
                #region row
                Table table = (Table)doc.GetChildNodes(NodeType.Table, true)[0]; //拿到表格
                for (int i = 0; i < list.Count; i++)
                {
                    table.Rows.Insert(1, CreateRow(list[i], doc));
                    builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.Empty;
                }
                table.Rows.Insert(1, CreateTotal(list, doc)); //将此行插入第一行的上方
                #endregion
                doc.Save(savePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool NCRDocDic2(string TemPath, string savePath, List<InvoiceModel> list, Dictionary<string, string> dic)
        {

            if (!File.Exists(TemPath.ToString()))
            {
                MessageBox.Show(string.Format("{0}模版文件不存在，请先设置模版文件。", TemPath.ToString()));
                return false;
            }
            string templateFile = TemPath;
            Document doc = new Document(templateFile);//读取模板
            DocumentBuilder builder = new DocumentBuilder(doc);
            try
            {
                //使用文本方式替换
                foreach (string name in dic.Keys)
                {
                    if (doc.Range.Bookmarks[name] != null)
                    {
                        Bookmark mark = doc.Range.Bookmarks[name];
                        mark.Text = dic[name];
                    }
                }
                #region row
                Table table = (Table)doc.GetChildNodes(NodeType.Table, true)[0]; //拿到表格
                for (int i = 0; i < list.Count; i++)
                {
                    table.Rows.Insert(1, CreateRow(list[i], doc));
                }
                table.Rows.Insert(1, CreateTotal(list, doc)); //将此行插入第一行的上方
                #endregion
                doc.Save(savePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 设置单元格内容对齐方式
        /// Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="bold"></param>
        /// <param name="Align"></param>
        /// <param name="Vertical"></param>
        /// <returns></returns>
        public Cell CreateCell(Document doc, string value, double size, bool bold, int Align, int Vertical)
        {
            Cell c1 = new Cell(doc);
            Paragraph p = new Paragraph(doc);
            Run run = new Run(doc, value);
            run.Font.Size = size;
            run.Font.Bold = bold;
            p.AppendChild(run);
            switch (Align)
            {
                case -1: p.ParagraphFormat.Alignment = ParagraphAlignment.Left; break;//左对齐
                case 0: p.ParagraphFormat.Alignment = ParagraphAlignment.Center; break;//水平居中
                case 1: p.ParagraphFormat.Alignment = ParagraphAlignment.Right; break;//右对齐
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
        public Row CreateRow(InvoiceModel im, Document doc)
        {
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, im.rowindex, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductCode, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductName, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductDescrEN, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.H2000Index, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ClearQty.ToString("0.000"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.NetWeight.ToString("0.000"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.GrossWeight.ToString("0.000"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.UnitPrice.ToString("0.00"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, (im.UnitPrice * im.ClearQty).ToString("0.00"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.CurrencyEN, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.MadeInEN, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.HSCode, 6.5, false, 1, 0));
            return row;
        }
        /// <summary>
        /// 添加统计行
        /// </summary>
        /// <param name="list"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateTotal(List<InvoiceModel> list, Document doc)
        {
            decimal sum = list.Sum(p => p.ClearQty);
            decimal NetWeight = list.Sum(p => (p.ClearQty * p.NetWeight));
            decimal Amount = list.Sum(p => (p.ClearQty * p.UnitPrice));
            string Currency = list[0].CurrencyEN;
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "Total:", 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, sum.ToString("0.000"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, NetWeight.ToString("0.000"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, NetWeight.ToString("0.000"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, "", 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, Amount.ToString("0.00"), 6.5, false, -1, 0));
            row.Cells.Add(CreateCell(doc, Currency, 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 6.5, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 6.5, false, 1, 0));
            return row;
        }
        public bool WordToPDF(string path, string savePath)
        {
            bool result = false;
            Document d = new Document(path);
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
