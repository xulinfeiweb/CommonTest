using w = Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Service
{
    public class Report
    {
        private w._Application wordApp = null;
        private w._Document wordDoc = null;
        public w._Application Application
        {
            get
            {
                return wordApp;
            }
            set
            {
                wordApp = value;
            }
        }
        public w._Document Document
        {
            get
            {
                return wordDoc;
            }
            set
            {
                wordDoc = value;
            }
        }

        //通过模板创建新文档
        public void CreateNewDocument(string filePath)
        {
            //killWinWordProcess();
            wordApp = new w.Application();
            wordApp.DisplayAlerts = w.WdAlertLevel.wdAlertsNone;
            wordApp.Visible = false;
            object missing = System.Reflection.Missing.Value;
            object templateName = filePath;
            wordDoc = wordApp.Documents.Open(ref templateName, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing);
        }

        //保存新文件
        public void SaveDocument(string filePath)
        {
            object fileName = filePath;
            object format = w.WdSaveFormat.wdFormatDocument;//保存格式
            object miss = System.Reflection.Missing.Value;
            wordDoc.SaveAs(ref fileName, ref format, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss);
            //关闭wordDoc，wordApp对象
            object SaveChanges = w.WdSaveOptions.wdSaveChanges;
            object OriginalFormat = w.WdOriginalFormat.wdOriginalDocumentFormat;
            object RouteDocument = false;
            wordDoc.Close(ref SaveChanges, ref OriginalFormat, ref RouteDocument);
            wordApp.Quit(ref SaveChanges, ref OriginalFormat, ref RouteDocument);
        }
        public w.Table InsertTable(string bookmark, int rows, int columns, float width)
        {
            object miss = System.Reflection.Missing.Value;
            object oStart = bookmark;
            w.Range range = wordDoc.Bookmarks.get_Item(ref oStart).Range;//表格插入位置
            w.Table newTable = wordDoc.Tables.Add(range, rows, columns, ref miss, ref miss);
            //设置表的格式
            newTable.Borders.Enable = 1;  //允许有边框，默认没有边框(为0时报错，1为实线边框，2、3为虚线边框，以后的数字没试过)
            newTable.Borders.OutsideLineWidth = w.WdLineWidth.wdLineWidth050pt;//边框宽度
            if (width != 0)
            {
                newTable.PreferredWidth = width;//表格宽度
            }
            newTable.AllowPageBreaks = false;
            return newTable;
        }
        public void SetParagraph_Table(w.Table table, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: table.Range.ParagraphFormat.Alignment = w.WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: table.Range.ParagraphFormat.Alignment = w.WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: table.Range.ParagraphFormat.Alignment = w.WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: table.Range.Cells.VerticalAlignment = w.WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: table.Range.Cells.VerticalAlignment = w.WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: table.Range.Cells.VerticalAlignment = w.WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }
        }

        //设置表格字体
        public void SetFont_Table(w.Table table, string fontName, double size)
        {
            if (size != 0)
            {
                table.Range.Font.Size = Convert.ToSingle(size);
            }
            if (fontName != "")
            {
                table.Range.Font.Name = fontName;
            }
        }

        //是否使用边框,n表格的序号,use是或否
        public void UseBorder(int n, bool use)
        {
            if (use)
            {
                wordDoc.Content.Tables[n].Borders.Enable = 1;  //允许有边框，默认没有边框(为0时报错，1为实线边框，2、3为虚线边框，以后的数字没试过)
            }
            else
            {
                wordDoc.Content.Tables[n].Borders.Enable = 2;  //允许有边框，默认没有边框(为0时报错，1为实线边框，2、3为虚线边框，以后的数字没试过)
            }
        }

        //给表格插入一行,n表格的序号从1开始记
        public void AddRow(int n)
        {
            object miss = System.Reflection.Missing.Value;
            wordDoc.Content.Tables[n].Rows.Add(ref miss);
        }

        //给表格添加一行
        public void AddRow(w.Table table)
        {
            object miss = System.Reflection.Missing.Value;
            table.Rows.Add(ref miss);
        }

        //给表格插入rows行,n为表格的序号
        public void AddRow(int n, int rows)
        {
            object miss = System.Reflection.Missing.Value;
            w.Table table = wordDoc.Content.Tables[n];
            for (int i = 0; i < rows; i++)
            {
                table.Rows.Add(ref miss);
            }
        }

        //给表格中单元格插入元素，table所在表格，row行号，column列号，value插入的元素
        public void InsertCell(w.Table table, int row, int column, string value)
        {
            table.Cell(row, column).Range.Text = value;
        }

        //给表格中单元格插入元素，n表格的序号从1开始记，row行号，column列号，value插入的元素
        public void InsertCell(int n, int row, int column, string value)
        {
            wordDoc.Content.Tables[n].Cell(row, column).Range.Text = value;
        }

        //给表格插入一行数据，n为表格的序号，row行号，columns列数，values插入的值
        public void InsertCell(int n, int row, int columns, string[] values)
        {
            w.Table table = wordDoc.Content.Tables[n];
            for (int i = 0; i < columns; i++)
            {
                table.Cell(row, i + 1).Range.Text = values[i];
            }
        }
        public w.Table InsertTable2(string bookmark, int rows, int columns, float width)
        {
            object miss = System.Reflection.Missing.Value; object oStart = bookmark; w.Range range = wordDoc.Bookmarks.get_Item(ref oStart).Range;//出错
            w.Table newTable = wordDoc.Tables.Add(range, rows, columns, ref miss, ref miss);
            newTable.Borders.Enable = 1;
            newTable.Borders.OutsideLineWidth = w.WdLineWidth.wdLineWidth025pt;
            newTable.PreferredWidth = width; newTable.AllowPageBreaks = false;
            wordApp.Selection.ParagraphFormat.Alignment = w.WdParagraphAlignment.wdAlignParagraphCenter;
            //水平居中
            wordApp.Selection.Cells.VerticalAlignment = w.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            //垂直居中 
            return newTable;
        }
    }
}