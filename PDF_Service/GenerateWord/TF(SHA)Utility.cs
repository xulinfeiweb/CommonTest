using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF_Service.GenerateWord
{
    /// <summary>
    /// TF(SHA)
    /// </summary>
    public class TF_SHA_Utility : WordBase
    {
        public TF_SHA_Utility(string tempFile, string saveFile)
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
                AddRow(1, dt.Rows.Count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        InsertCell(1, i + 2, j + 1, dt.Rows[i][j].ToString());
                        SetFont_Table(1, i + 2, j + 1, "Arial", 10, 0);
                    }
                }
                #region 插入统计行
                //插入统计行
                int TotalIndex = dt.Rows.Count + 3;//统计行的索引
                AddRow(1, 1);
                InsertCell(1, TotalIndex, 3, "Total:");
                SetFont_Table(1, TotalIndex, 3, "Arial", 10, 0);
                InsertCell(1, TotalIndex, 4, "");
                SetFont_Table(1, TotalIndex, 4, "Arial", 10, 0);
                InsertCell(1, TotalIndex, 5, "");
                SetFont_Table(1, TotalIndex, 5, "Arial", 10, 0);
                InsertCell(1, TotalIndex, 7, "");
                SetFont_Table(1, TotalIndex, 7, "Arial", 10, 0);
                InsertCell(1, TotalIndex, 8, "");
                SetFont_Table(1, TotalIndex, 8, "Arial", 10, 0);
                #endregion
                #endregion
                #region 加入图片
                //加入图片，并指定标签
                string pic = "Pic";
                string picpath = "C:\\Users\\Administrator\\Documents\\Visual Studio 2015\\Projects\\Test\\PDF_Service\\img\\3.png";
                InsertPicture(pic, picpath, 162, 140);
                //将图片设置为衬与文字下方
                word.Shape s = wDoc.Application.ActiveDocument.InlineShapes[1].ConvertToShape();
                s.WrapFormat.Type = word.WdWrapType.wdWrapSquare;
                #endregion
                //保存word
                wDoc.SaveAs(ref saveFile, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                      ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //释放资源
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
