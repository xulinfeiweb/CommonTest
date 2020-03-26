using System;
using System.Collections.Generic;
using word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Data;
using System.Linq;

namespace Common
{
    /// <summary>
    /// LSG
    /// </summary>
    public class LSGUtility : WordBase
    {
        public LSGUtility(string saveFile)
        {
            this.saveFile = saveFile;
        }
        /// <summary>
        /// 模版包含头部信息和表格，表格重复使用
        /// </summary>
        /// <param name="list">重复表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord(object tempFile, List<InvoiceModel> list, Dictionary<string, string> dic)
        {
            if (!File.Exists(tempFile.ToString()))
            {
                return false;
            }
            try
            {
                wApp = new word.Application();
                //创建一个word应用程序实例  
                wApp.Visible = false;
                wDoc = wApp.Documents.Add(ref tempFile, ref Nothing, ref Nothing, ref Nothing);
                wDoc.Activate();// 当前文档置前
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
                    InsertCell(1, i + 2, 1, list[i].rowindex);
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
}
