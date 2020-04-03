using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Words.Tables;
using Aspose.Words;

namespace Common
{
    /// <summary>
    /// WGQBUNN
    /// </summary>
    public class WGQBUNNUtility : WordBase
    {
        public WGQBUNNUtility(string temFile,
           string wordFile,
           string pdfFile,
           List<InvoiceModel> list,
           Dictionary<string, string> dic)
        {
            base.GenerateWord(temFile, wordFile, list, dic, CreateRow, CreateTotal);
            base.WordToPDF(wordFile, pdfFile);
        }
        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
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
            row.Cells.Add(CreateCell(doc, im.NetWeight.ToString("0.000"), 6.5, false, -1, 0));
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
    }
}
