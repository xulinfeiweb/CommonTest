using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
    /// <summary>
    /// ILMN
    /// </summary>
    public class ILMNUtility : WordBase
    {
        public ILMNUtility(string temFile,
           string wordFile,
           string pdfFile,
           List<InvoiceModel> list,
           Dictionary<string, string> dic)
        {
            base.GenerateWord(temFile, wordFile, list, dic, CreateRow, CreateTotal);
            base.WordToPDF(wordFile, pdfFile);
        }
        /// <summary>
        /// 针对当前厂商 添加行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateRow(InvoiceModel im, Document doc)
        {
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, im.rowindex, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductCode, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductDescrEN, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ClearQty.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.NetWeight.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.UnitPrice.ToString("0.00"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, (im.UnitPrice * im.ClearQty).ToString("0.00"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.CurrencyEN, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.MadeInEN, 10, false, 1, 0));
            return row;
        }
        /// <summary>
        /// 添加统计行
        /// </summary>
        /// <param name="im"></param>
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
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "Total:", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, sum.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, NetWeight.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, Amount.ToString("0.00"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, Currency, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            return row;
        }
    }
}
