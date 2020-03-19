using System;
using System.Collections.Generic;
using System.Linq;
using word = Microsoft.Office.Interop.Word;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace PDF_Service
{
    /// <summary>
    /// BUNN
    /// </summary>
    public class BUNNUtility : WordBase
    {
        public BUNNUtility(string tempFile, string saveFile)
        {
            this.tempFile = Path.Combine(Application.StartupPath, @tempFile);
            this.saveFile = Path.Combine(Application.StartupPath, @saveFile);
        }

        /// <summary>
        /// 模版包含头部信息和表格，表格重复使用
        /// </summary>
        /// <param name="dt">重复表格的数据</param>
        /// <param name="dic">word中要替换的表达式和表格字段的对应关系</param>
        public bool GenerateWord(DataTable dt, Dictionary<string, string> dic)
        {
            if (!File.Exists(tempFile.ToString()))
            {
                MessageBox.Show(string.Format("{0}模版文件不存在，请先设置模版文件。", tempFile.ToString()));
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
                AddRow(2, dt.Rows.Count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        InsertCell(2, i + 2, j + 1, dt.Rows[i][j].ToString());
                        SetFont_Table(2, i + 2, j + 1, "Arial", 6.5, 0);
                        wDoc.Content.Tables[2].Cell(i + 2, j + 1).Range.Shading.BackgroundPatternColor = word.WdColor.wdColorAutomatic;
                    }
                }
                #region 插入统计行
                //插入统计行
                int TotalIndex = dt.Rows.Count + 3;//统计行的索引
                AddRow(2, 1);
                InsertCell(2, TotalIndex, 3, "Total:");
                SetFont_Table(2, TotalIndex, 3, "Arial", 10, 0);
                InsertCell(2, TotalIndex, 4, "");
                SetFont_Table(2, TotalIndex, 4, "Arial", 10, 0);
                InsertCell(2, TotalIndex, 5, "");
                SetFont_Table(2, TotalIndex, 5, "Arial", 10, 0);
                InsertCell(2, TotalIndex, 7, "");
                SetFont_Table(2, TotalIndex, 7, "Arial", 10, 0);
                InsertCell(2, TotalIndex, 8, "");
                SetFont_Table(2, TotalIndex, 8, "Arial", 10, 0);
                #endregion
                SetParagraph_Table(wDoc.Content.Tables[2], -1, 0);
                #endregion
                //#region 加入图片
                ////加入图片，并指定标签
                //string pic1 = "Pic1";
                //object pic2 = "Pic2";
                //string picpath1 = "C:\\Users\\Administrator\\Documents\\Visual Studio 2015\\Projects\\Test\\PDF_Service\\img\\chapter.png";
                //string picpath2 = "C:\\Users\\Administrator\\Documents\\Visual Studio 2015\\Projects\\Test\\PDF_Service\\img\\sign.png";
                //InsertPicture(pic1, picpath1, 162, 140);
                ////将图片设置为衬于文字下方   
                //word.Shape s = wDoc.Application.ActiveDocument.InlineShapes[1].ConvertToShape();
                //s.WrapFormat.Type = word.WdWrapType.wdWrapBehind;
                //wDoc.Bookmarks.get_Item(ref pic2).Range.InlineShapes.AddPicture(picpath2, ref Nothing, ref Nothing, ref Nothing);
                //#endregion
                wDoc.SaveAs(ref saveFile, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                      ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                DisposeWord();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败" + ex.Message);
                return false;
            }
        }


    }
}
