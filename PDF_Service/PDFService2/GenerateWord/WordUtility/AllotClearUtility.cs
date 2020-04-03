using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Words.Tables;
using Aspose.Words;

namespace Common
{
    /// <summary>
    /// 分拨结关
    /// </summary>
    public class AllotClearUtility : WordBase
    {
        public string _wordFile = null;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tempFile">模板路径</param>
        /// <param name="saveFile">保存路径</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public AllotClearUtility(object tempFile, string wordFile, Dictionary<string, string> dic)
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
            this._wordFile = wordFile;
        }
        /// <summary>
        /// 分拨结关
        /// 格式一
        /// </summary>
        /// <param name="list">表格的数据</param>
        public bool GenerateWord_FB1(List<InvoiceModel> list)
        {
            try
            {
                #region 添加行
                Table table = (Table)_doc.GetChildNodes(NodeType.Table, true)[0]; //拿到表格
                foreach (InvoiceModel item in list)
                {
                    table.AppendChild(CreateRow_1(item, _doc));
                }
                //统计
                table.AppendChild(CreateTotal_1(list, _doc));
                #endregion
                _doc.Save(_wordFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 针对当前厂商 添加行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateRow_1(InvoiceModel im, Document doc)
        {
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, im.rowindex, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductCode, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductName, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ProductDescrEN, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.ClearQty.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.LegalUnitEN, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, im.UnitPrice.ToString("0.00"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, (im.UnitPrice * im.ClearQty).ToString("0.00"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, im.CurrencyEN, 10, false, 1, 0));
            return row;
        }
        /// <summary>
        /// 添加统计行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateTotal_1(List<InvoiceModel> list, Document doc)
        {
            decimal sum = list.Sum(p => p.ClearQty);
            decimal Amount = list.Sum(p => (p.ClearQty * p.UnitPrice));
            string unit = list[0].LegalUnitEN;
            string Currency = list[0].CurrencyEN;
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "Total:", 10, true, 1, 0));
            row.Cells.Add(CreateCell(doc, sum.ToString("0.000"), 10, true, -1, 0));
            row.Cells.Add(CreateCell(doc, unit, 10, true, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, Amount.ToString("0.00"), 10, true, -1, 0));
            row.Cells.Add(CreateCell(doc, Currency, 10, true, 1, 0));
            return row;
        }
        /// <summary>
        /// 分拨结关
        /// 格式二
        /// </summary>
        /// <param name="list">表格的数据</param>
        public bool GenerateWord_FB2(List<OutStockListModel> list)
        {
            try
            {
                #region 添加行
                Table table = (Table)_doc.GetChildNodes(NodeType.Table, true)[0]; //拿到表格
                for (int i = 0; i < list.Count; i++)
                {
                    table.AppendChild(CreateRow_2(i + 1, list[i], _doc));
                }
                //统计
                decimal sum = list.Sum(p => p.LegalClearQty);
                table.AppendChild(CreateTotal_2(sum, _doc));
                #endregion
                _doc.Save(_wordFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 针对当前厂商 添加行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateRow_2(int index, OutStockListModel om, Document doc)
        {
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, index.ToString(), 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, om.ProductCodeC, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, om.LegalClearQty.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, om.ProductCode, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, om.OutDate.ToString("yyyy-MM-dd"), 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, om.HAWB, 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, om.OutDate.ToString("yyyy-MM-dd"), 10, false, 1, 0));
            return row;
        }
        /// <summary>
        /// 添加统计行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateTotal_2(decimal sum, Document doc)
        {
            Row row = new Row(doc);
            row.Cells.Add(CreateCell(doc, "Total:", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, sum.ToString("0.000"), 10, false, -1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            row.Cells.Add(CreateCell(doc, "", 10, false, 1, 0));
            return row;
        }
        /// <summary>
        /// 分拨结关
        /// 格式三
        /// </summary>
        /// <param name="list">表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord_FB3(List<InvoiceModel> list)
        {
            try
            {
                #region 添加行
                Table table = (Table)_doc.GetChildNodes(NodeType.Table, true)[0]; //拿到表格
                foreach (InvoiceModel item in list)
                {
                    table.AppendChild(CreateRow_3(item, _doc));
                }
                //统计
                table.AppendChild(CreateTotal_3(list, _doc));
                #endregion
                _doc.Save(_wordFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 针对当前厂商 添加行
        /// </summary>
        /// <param name="im"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Row CreateRow_3(InvoiceModel im, Document doc)
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
        public Row CreateTotal_3(List<InvoiceModel> list, Document doc)
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
    /// <summary>
    /// 厂商的名称
    /// </summary>
    public class ComPName
    {
        protected string _vendName { get; set; }
        public ComPName(string vendName)
        {
            this._vendName = vendName;
        }
        /// <summary>
        /// 厂商的名称转换
        /// </summary>
        /// <returns></returns>
        public string TransVendName()
        {
            string result = "";
            switch (_vendName)
            {
                case "ILMN":
                    result = "宜曼达贸易（上海）有限公司";
                    break;
                case "Juniper (SHA)":
                    result = "Juniper";
                    break;
                case "KUKA":
                    result = "KUKA Robotics";
                    break;
                case "TF(SHA)":
                    result = "Thermo fisher";
                    break;
                default:
                    result = _vendName;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 厂商调用分拨结关模板格式
        /// </summary>
        /// <returns></returns>
        public int TransFunc()
        {
            int result = 0;
            switch (_vendName)
            {
                case "BRUKER":
                case "BUNN":
                case "DRAEGER":
                case "Juniper (SHA)":
                    result = 2;
                    break;
                case "ILMN":
                    result = 3;
                    break;
                case "KUKA":
                case "TF(SHA)":
                    result = 1;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
    }
}
