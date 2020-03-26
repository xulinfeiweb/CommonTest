using System;
using System.Collections.Generic;
using word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Data;
using System.Linq;

namespace Common
{
    /// <summary>
    /// 分拨结关
    /// </summary>
    public class AllotClearUtility : WordBase
    {
        public AllotClearUtility(string saveFile)
        {
            this.saveFile = saveFile;
        }

        /// <summary>
        /// 分拨结关
        /// 格式一
        /// </summary>
        /// <param name="list">表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord_FB(object tempFile, List<InvoiceModel> list, Dictionary<string, string> dic)
        {
            if (!File.Exists(tempFile.ToString()))
            {
                return false;
            }
            try
            {
                //创建一个word应用程序实例  
                wApp = new word.Application();
                wApp.Visible = false;
                wDoc = wApp.Documents.Add(ref tempFile, ref Nothing, ref Nothing, ref Nothing);
                // 当前文档置前
                wDoc.Activate();
                //循环所有书签，并赋值
                foreach (var item in dic)
                {
                    object obDD_Name = item.Key;
                    wDoc.Bookmarks.get_Item(ref obDD_Name).Range.Text = item.Value;
                }
                #region 在表格中插入行
                AddRow(1, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    InsertCell(1, i + 2, 1, (i + 1).ToString());
                    InsertCell(1, i + 2, 2, list[i].ProductCode);
                    InsertCell(1, i + 2, 3, list[i].ProductName);
                    InsertCell(1, i + 2, 4, list[i].ProductDescrEN);
                    InsertCell(1, i + 2, 5, list[i].ClearQty.ToString("0.000"));
                    InsertCell(1, i + 2, 6, list[i].LegalUnitEN);
                    InsertCell(1, i + 2, 7, list[i].UnitPrice.ToString("0.00"));
                    InsertCell(1, i + 2, 8, (list[i].UnitPrice * list[i].ClearQty).ToString("0.00"));
                    InsertCell(1, i + 2, 9, list[i].CurrencyEN);
                    for (int j = 0; j < 9; j++)
                    {
                        SetFont_Table(1, i + 2, j + 1, "Arial", 10, 0);
                        //底纹设为白色，即无色
                        wDoc.Content.Tables[1].Cell(i + 2, j + 1).Range.Shading.BackgroundPatternColor = word.WdColor.wdColorAutomatic;
                        #region 单元格对齐方式
                        int Align = -1;//默认左对齐
                        ////左对齐列
                        //int[] arr1 = { 0, 1, 2, 3, 5, 8 };
                        //右对齐列
                        int[] arr = { 4, 6, 7 };
                        if (arr.Contains(j))
                            Align = 1;
                        SetParagraph_Table(1, i + 2, j + 1, Align, 0);
                        #endregion
                    }
                }
                #region 插入统计行
                decimal sum = list.Sum(p => p.ClearQty);
                decimal Amount = list.Sum(p => (p.ClearQty * p.UnitPrice));
                string unit = list[0].LegalUnitEN;
                string Currency = list[0].CurrencyEN;
                //插入统计行
                int TotalIndex = list.Count + 3;//统计行的索引
                AddRow(1, 1);
                InsertCell(1, TotalIndex, 4, "Total:");
                SetFont_Table(1, TotalIndex, 4, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 4, -1, 0);

                InsertCell(1, TotalIndex, 5, sum.ToString("0.000"));
                SetFont_Table(1, TotalIndex, 5, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 5, 1, 0);

                InsertCell(1, TotalIndex, 6, unit);
                SetFont_Table(1, TotalIndex, 6, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 6, -1, 0);

                InsertCell(1, TotalIndex, 8, Amount.ToString("0.00"));
                SetFont_Table(1, TotalIndex, 8, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 8, 1, 0);

                InsertCell(1, TotalIndex, 9, Currency);
                SetFont_Table(1, TotalIndex, 9, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 9, -1, 0);
                #endregion
                #endregion
                wDoc.SaveAs(ref saveFile, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                      ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                DisposeWord();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 分拨结关
        /// 格式二
        /// </summary>
        /// <param name="list">表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord_FB(object tempFile, List<OutStockListModel> list, Dictionary<string, string> dic)
        {
            if (!File.Exists(tempFile.ToString()))
            {
                return false;
            }
            try
            {
                //创建一个word应用程序实例  
                wApp = new word.Application();
                wApp.Visible = false;
                wDoc = wApp.Documents.Add(ref tempFile, ref Nothing, ref Nothing, ref Nothing);
                // 当前文档置前
                wDoc.Activate();
                //循环所有书签，并赋值
                foreach (var item in dic)
                {
                    object obDD_Name = item.Key;
                    wDoc.Bookmarks.get_Item(ref obDD_Name).Range.Text = item.Value;
                }
                #region 在表格中插入行
                AddRow(1, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    InsertCell(1, i + 2, 1, (i + 1).ToString());
                    InsertCell(1, i + 2, 2, list[i].ProductCodeC);
                    InsertCell(1, i + 2, 3, list[i].LegalClearQty.ToString("0.000"));
                    InsertCell(1, i + 2, 4, list[i].ProductCode);
                    InsertCell(1, i + 2, 5, list[i].OutDate.ToString("yyyy-MM-dd"));
                    InsertCell(1, i + 2, 6, list[i].HAWB);
                    InsertCell(1, i + 2, 7, list[i].OutDate.ToString("yyyy-MM-dd"));
                    for (int j = 0; j < 7; j++)
                    {
                        SetFont_Table(1, i + 2, j + 1, "Arial", 10, 0);
                        //底纹设为白色，即无色
                        wDoc.Content.Tables[1].Cell(i + 2, j + 1).Range.Shading.BackgroundPatternColor = word.WdColor.wdColorAutomatic;
                        #region 单元格对齐方式
                        int Align = -1;//默认左对齐
                        ////左对齐列
                        //int[] arr1 = { 0, 1, 3, 4, 5, 6 };
                        //右对齐列
                        int[] arr = { 2 };
                        if (arr.Contains(j))
                            Align = 1;
                        SetParagraph_Table(1, i + 2, j + 1, Align, 0);
                        #endregion
                    }
                }
                #region 插入统计行
                decimal sum = list.Sum(p => p.LegalClearQty);
                //插入统计行
                int TotalIndex = list.Count + 3;//统计行的索引
                AddRow(1, 1);
                InsertCell(1, TotalIndex, 1, "Total:");
                SetFont_Table(1, TotalIndex, 1, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 1, -1, 0);

                InsertCell(1, TotalIndex, 3, sum.ToString("0.000"));
                SetFont_Table(1, TotalIndex, 3, "Arial", 10, 0);
                SetParagraph_Table(1, TotalIndex, 3, 1, 0);
                #endregion
                #endregion
                wDoc.SaveAs(ref saveFile, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                      ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                DisposeWord();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 分拨结关
        /// 格式三
        /// </summary>
        /// <param name="list">表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord_FB2(object tempFile, List<InvoiceModel> list, Dictionary<string, string> dic)
        {
            if (!File.Exists(tempFile.ToString()))
            {
                return false;
            }
            try
            {
                //创建一个word应用程序实例  
                wApp = new word.Application();
                wApp.Visible = false;
                wDoc = wApp.Documents.Add(ref tempFile, ref Nothing, ref Nothing, ref Nothing);
                // 当前文档置前
                wDoc.Activate();
                //循环所有书签，并赋值
                foreach (var item in dic)
                {
                    object obDD_Name = item.Key;
                    wDoc.Bookmarks.get_Item(ref obDD_Name).Range.Text = item.Value;
                }
                #region 在表格中插入行
                AddRow(1, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    InsertCell(1, i + 2, 1, (i + 1).ToString());
                    InsertCell(1, i + 2, 2, list[i].ProductCode);
                    InsertCell(1, i + 2, 3, list[i].ProductDescrEN);
                    InsertCell(1, i + 2, 4, list[i].ClearQty.ToString("0.000"));
                    InsertCell(1, i + 2, 5, list[i].NetWeight.ToString("0.000"));
                    InsertCell(1, i + 2, 6, list[i].UnitPrice.ToString("0.00"));
                    InsertCell(1, i + 2, 7, (list[i].UnitPrice * list[i].ClearQty).ToString("0.00"));
                    InsertCell(1, i + 2, 8, list[i].CurrencyEN);
                    InsertCell(1, i + 2, 9, list[i].MadeInEN);
                    for (int j = 0; j < 9; j++)
                    {
                        SetFont_Table(1, i + 2, j + 1, "Arial", 10, 0);
                        //底纹设为白色，即无色
                        wDoc.Content.Tables[1].Cell(i + 2, j + 1).Range.Shading.BackgroundPatternColor = word.WdColor.wdColorAutomatic;
                        #region 单元格对齐方式
                        int Align = -1;//默认左对齐
                        //左对齐列
                        //int[] arr1 = { 0, 1, 2, 7, 8 };
                        //右对齐列
                        int[] arr = { 3, 4, 5, 6 };
                        if (arr.Contains(j))
                            Align = 1;
                        SetParagraph_Table(1, i + 2, j + 1, Align, 0);
                        #endregion
                    }
                }
                #region 插入统计行
                decimal sum = list.Sum(p => p.ClearQty);
                decimal NetWeight = list.Sum(p => (p.ClearQty * p.NetWeight));
                decimal Amount = list.Sum(p => (p.ClearQty * p.UnitPrice));
                string Currency = list[0].CurrencyEN;
                //插入统计行
                int TotalIndex = list.Count + 3;//统计行的索引
                AddRow(1, 1);
                InsertCell(1, TotalIndex, 3, "Total:");
                SetFont_Table(1, TotalIndex, 3, "Arial", 10, 1);
                SetParagraph_Table(1, TotalIndex, 3, -1, 0);

                InsertCell(1, TotalIndex, 4, sum.ToString("0.000"));
                SetFont_Table(1, TotalIndex, 4, "Arial", 10, 1);
                SetParagraph_Table(1, TotalIndex, 4, 1, 0);

                InsertCell(1, TotalIndex, 5, NetWeight.ToString("0.000"));
                SetFont_Table(1, TotalIndex, 5, "Arial", 10, 1);
                SetParagraph_Table(1, TotalIndex, 5, 1, 0);

                InsertCell(1, TotalIndex, 7, Amount.ToString("0.00"));
                SetFont_Table(1, TotalIndex, 7, "Arial", 10, 1);
                SetParagraph_Table(1, TotalIndex, 7, 1, 0);

                InsertCell(1, TotalIndex, 8, Currency);
                SetFont_Table(1, TotalIndex, 8, "Arial", 10, 1);
                SetParagraph_Table(1, TotalIndex, 8, -1, 0);
                #endregion
                #endregion
                wDoc.SaveAs(ref saveFile, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                      ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                DisposeWord();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
    }
}
