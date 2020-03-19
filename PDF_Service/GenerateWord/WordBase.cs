using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using word = Microsoft.Office.Interop.Word;
using System.Threading.Tasks;

namespace PDF_Service
{
    public class WordBase
    {
        protected object tempFile = null;
        protected object saveFile = null;
        protected static word._Document wDoc = null; //word文档
        protected static word._Application wApp = null; //word进程
        protected object Nothing = System.Reflection.Missing.Value;
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="picturePath"></param>
        /// <param name="width"></param>
        /// <param name="hight"></param>
        protected void InsertPicture(string bookmark, string picturePath, float width, float hight)
        {
            object miss = System.Reflection.Missing.Value;
            object oStart = bookmark;
            Object linkToFile = false;       //图片是否为外部链接
            Object saveWithDocument = true;  //图片是否随文档一起保存
            object range = wDoc.Bookmarks.get_Item(ref oStart).Range;//图片插入位置
            wDoc.InlineShapes.AddPicture(picturePath, ref linkToFile, ref saveWithDocument, ref range);
            wDoc.Application.ActiveDocument.InlineShapes[1].Width = width;   //设置图片宽度
            wDoc.Application.ActiveDocument.InlineShapes[1].Height = hight;  //设置图片高度
        }

        protected word.Table InsertTable(string bookmark, int rows, int columns, float width)
        {
            object miss = System.Reflection.Missing.Value;
            object oStart = bookmark;
            word.Range range = wDoc.Bookmarks.get_Item(ref oStart).Range;//表格插入位置
            word.Table newTable = wDoc.Tables.Add(range, rows, columns, ref miss, ref miss);
            //设置表的格式
            newTable.Borders.Enable = 1;  //允许有边框，默认没有边框(为0时报错，1为实线边框，2、3为虚线边框，以后的数字没试过)
            newTable.Borders.OutsideLineWidth = word.WdLineWidth.wdLineWidth050pt;//边框宽度
            if (width != 0)
            {
                newTable.PreferredWidth = width;//表格宽度
            }
            newTable.AllowPageBreaks = false;
            return newTable;
        }
        /// <summary>
        /// 设置表格内容对齐方式 Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)Microsoft.Office.Interop.Word.Table table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="Align"></param>
        /// <param name="Vertical"></param>
        protected void SetParagraph_Table(word.Table table, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: table.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: table.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: table.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: table.Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: table.Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: table.Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }
        }
        /// <summary>
        /// 设置表格内容对齐方式 Align水平方向，Vertical垂直方向(左对齐，居中对齐，右对齐分别对应Align和Vertical的值为-1,0,1)Microsoft.Office.Interop.Word.Table table
        /// </summary>
        /// <param name="n"></param>
        /// <param name="Align"></param>
        /// <param name="Vertical"></param>
        public void SetParagraph_Table(int n, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: wDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: wDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: wDoc.Content.Tables[n].Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: wDoc.Content.Tables[n].Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: wDoc.Content.Tables[n].Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: wDoc.Content.Tables[n].Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }
        }

        /// <summary>
        /// 设置单元格内容对齐方式
        /// </summary>
        /// <param name="n"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="Align"></param>
        /// <param name="Vertical"></param>
        public void SetParagraph_Table(int n, int row, int column, int Align, int Vertical)
        {
            switch (Align)
            {
                case -1: wDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft; break;//左对齐
                case 0: wDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter; break;//水平居中
                case 1: wDoc.Content.Tables[n].Cell(row, column).Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphRight; break;//右对齐
            }
            switch (Vertical)
            {
                case -1: wDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalTop; break;//顶端对齐
                case 0: wDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalCenter; break;//垂直居中
                case 1: wDoc.Content.Tables[n].Cell(row, column).Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalBottom; break;//底端对齐
            }
        }

        //给表格中单元格插入元素，table所在表格，row行号，column列号，value插入的元素
        protected void InsertCell(word.Table table, int row, int column, string value)
        {
            table.Cell(row, column).Range.Text = value;
        }

        //给表格中单元格插入元素，n表格的序号从1开始记，row行号，column列号，value插入的元素
        protected void InsertCell(int n, int row, int column, string value)
        {
            wDoc.Content.Tables[n].Cell(row, column).Range.Text = value;
        }

        //给表格插入一行数据，n为表格的序号，row行号，columns列数，values插入的值
        protected void InsertCell(int n, int row, int columns, string[] values)
        {
            word.Table table = wDoc.Content.Tables[n];
            for (int i = 0; i < columns; i++)
            {
                table.Cell(row, i + 1).Range.Text = values[i];
            }
        }

        // 给表格插入一行,n表格的序号从1开始记
        public void AddRow(int n)
        {
            object miss = System.Reflection.Missing.Value;
            wDoc.Content.Tables[n].Rows.Add(ref miss);
        }

        // 给表格添加一行
        public void AddRow(word.Table table)
        {
            object miss = System.Reflection.Missing.Value;
            table.Rows.Add(ref miss);
        }

        /// <summary>
        /// 给表格插入rows行
        /// </summary>
        /// <param name="n">为表格的序号</param>
        /// <param name="rows">插入的行数</param>
        public void AddRow(int n, int rows)
        {
            object miss = System.Reflection.Missing.Value;
            word.Table table = wDoc.Content.Tables[n];
            for (int i = 0; i < rows; i++)
            {
                table.Rows.Add(ref miss);
            }
        }
        /// <summary>
        /// 设置表格字体
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fontName"></param>
        /// <param name="size"></param>
        public void SetFont_Table(word.Table table, string fontName, double size)
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

        /// <summary>
        /// 设置单元格字体
        /// </summary>
        /// <param name="n"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="fontName"></param>
        /// <param name="size"></param>
        /// <param name="bold"></param>
        public void SetFont_Table(int n, int row, int column, string fontName, double size, int bold)
        {
            if (size != 0)
            {
                wDoc.Content.Tables[n].Cell(row, column).Range.Font.Size = Convert.ToSingle(size);
            }
            if (fontName != "")
            {
                wDoc.Content.Tables[n].Cell(row, column).Range.Font.Name = fontName;
            }
            wDoc.Content.Tables[n].Cell(row, column).Range.Font.Bold = bold;// 0 表示不是粗体，其他值都是
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected void DisposeWord()
        {
            object saveOption = word.WdSaveOptions.wdSaveChanges;

            wDoc.Close(ref saveOption, ref Nothing, ref Nothing);

            saveOption = word.WdSaveOptions.wdDoNotSaveChanges;

            wApp.Quit(ref saveOption, ref Nothing, ref Nothing); //关闭Word进程
        }
        //// <summary>
        /// 把Word文件转换成pdf文件
        /// </summary>
        /// <param name="sourcePath">需要转换的文件路径和文件名称</param>
        /// <param name="targetPath">转换完成后的文件的路径和文件名名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool WordToPdf(object sourcePath, string targetPath)
        {
            bool result = false;
            word.WdExportFormat wdExportFormatPDF = word.WdExportFormat.wdExportFormatPDF;
            object missing = Type.Missing;
            word.Application application = null;
            word.Document document = null;
            try
            {
                application = new word.Application();
                document = application.Documents.Open(ref sourcePath, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                if (document != null)
                {
                    document.ExportAsFixedFormat(targetPath, wdExportFormatPDF, false, word.WdExportOptimizeFor.wdExportOptimizeForPrint, word.WdExportRange.wdExportAllDocument, 0, 0, word.WdExportItem.wdExportDocumentContent, true, true, word.WdExportCreateBookmarks.wdExportCreateWordBookmarks, true, true, false, ref missing);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (document != null)
                {
                    document.Close(ref missing, ref missing, ref missing);
                    document = null;
                }
                if (application != null)
                {
                    application.Quit(ref missing, ref missing, ref missing);
                    application = null;
                }
            }
            return result;
        }

    }
}
