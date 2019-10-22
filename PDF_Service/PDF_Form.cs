﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using PdfHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF_Service
{
    public partial class PDF_Form : Form
    {
        /// <summary>
        /// 只支持pdf文件
        /// </summary>
        private const string PDF_FileType = "PDF文件|*.pdf";
        /// <summary>
        /// 图片类型
        /// </summary>
        private const string Img_FileType = "Image文件|*.jpg;*.png;*.bmp;*.gif";
        public PDF_Form()
        {
            InitializeComponent();
        }
        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_pdf_path.Text))
            {
                MessageBox.Show("请选择PDF文件", "提示");
            }
            else
            {
                WritePDF(txt_pdf_path.Text.Trim(), txt_pdf_value.Text.Trim(), txt_img_path.Text.Trim());
            }
        }
        /// <summary>
        ///生成PDF
        /// </summary>
        /// <param name="path">pdf文件路径</param>
        /// <param name="value">文本值</param>
        public void WritePDF(string path, string value, string imgpath)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(path) + "_new.pdf";
            string currpath = System.IO.Path.GetDirectoryName(path) + "\\" + filename;
            using (FileStream stream = new FileStream(currpath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfReader pdfReader = new PdfReader(path);//读pdf
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, stream))
                {
                    //插入文字
                    BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//获取系统的字体
                    string pdf_value = "123456789";
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        pdf_value = value;
                    }
                    iTextSharp.text.Font font2 = new iTextSharp.text.Font(baseFont, 11);//字体样式
                    Phrase ActualName2 = new Phrase(pdf_value, font2);//姓名
                    PdfContentByte over2 = pdfStamper.GetOverContent(1);//pdf页数
                    ColumnText.ShowTextAligned(over2, Element.ALIGN_CENTER, ActualName2, 265, 800, 0);//姓名

                    //插入图片
                    string img_path = "C:\\Users\\Administrator\\Desktop\\图片\\允儿.jpg";
                    if (!string.IsNullOrWhiteSpace(imgpath))
                    {
                        img_path = imgpath;
                    }
                    if (!string.IsNullOrWhiteSpace(img_path))
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(img_path);
                        img.ScaleAbsolute(72, 100);
                        img.SetAbsolutePosition(76, 475);
                        over2.AddImage(img);
                    }
                    //清空文本
                    ClearTextBox();
                    MessageBox.Show("生成成功。", "提示");
                }
            }
        }
        private void ClearTextBox()
        {
            txt_pdf_value.Text = string.Empty;
            txt_img_path.Text = string.Empty;
            txt_pdf_path.Text = string.Empty;
        }
        /// <summary>
        /// 选择文件并获取文件路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetPath(string type)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = type;
            string file = "";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                file = fileDialog.FileName;
            }
            return file;
        }
        private void btn_id_Click(object sender, EventArgs e)
        {
            string file = GetPath(PDF_FileType);
            this.txtID.Text = file;
        }
        private void btn_img_Click(object sender, EventArgs e)
        {
            string file = GetPath(Img_FileType);
            this.txt_img_path.Text = file;
        }
        private void btn_pdf_Click(object sender, EventArgs e)
        {
            string file = GetPath(PDF_FileType);
            txt_pdf_path.Text = file;
        }
        private void btn_query_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("请选择PDF文件", "提示");
            }
            else
            {
                try
                {
                    PdfReader reader = new PdfReader(txtID.Text);
                    PdfReaderContentParser parser = new PdfReaderContentParser(reader);
                    int Pdf_Pages = reader.NumberOfPages;
                    string jsonData = string.Empty;
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                    for (int i = 1; i <= Pdf_Pages; i++)
                    {
                        ITextExtractionStrategy strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(i, new SimpleTextExtractionStrategy());
                        string contentOfPage = strategy.GetResultantText();
                        GetPDFText(contentOfPage, data, list);
                    }
                    Dictionary<string, object> Obj = new Dictionary<string, object>();
                    Obj.Add("Head", data);
                    Obj.Add("Data", list);
                    richTextBox.Text = SerializeDictionaryToJsonString(Obj);
                }
                catch (Exception ex)
                {
                    richTextBox.Text = ex.Message;
                }
            }
        }
        public void GetPDFText(string Text, Dictionary<string, string> data, List<Dictionary<string, string>> list)
        {
            List<string> dataArr = Text.Split('\n').ToList();
            #region Number/date 单号/日期
            int index_number = dataArr.IndexOf("Number/date 单号/日期");
            if (index_number < 0)
            {
                index_number = dataArr.IndexOf("Number/date");
            }
            string ShipCode = "";
            if (index_number > -1)
            {
                string[] strArr = dataArr[index_number + 1].Split('/');
                ShipCode = strArr[0].Trim();
                data.Add("ShipCode", strArr[0].Trim());
                data.Add("ShipDate", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region Ship-to address 收货方地址
            //收货方地址中字段数量不明确
            int index_shop = dataArr.IndexOf("Ship-to address 收货方地址");
            if (index_shop < 0)
            {
                index_shop = dataArr.IndexOf("Ship-to address ");
            }
            if (index_shop > -1)
            {
                //收货方地址的行数
                int bgn_row = dataArr.IndexOf(" " + ShipCode) - (index_shop + 1);
                for (int i = 1; i <= bgn_row; i++)
                {
                    int index = index_shop + i;
                    if (dataArr[index].Split(':').Length == 2)
                        data.Add("StrName_" + i, dataArr[index].Split(':')[1].Trim());
                    else
                        data.Add("StrName_" + i, dataArr[index]);
                }
                //data.Add("ReceiveName", dataArr[index_shop + 1]);
                //string ReceiveCode = dataArr[index_shop + 2];
                //if (CheckStrIsNumber(ReceiveCode))
                //{
                //    data.Add("ReceiveCode", ReceiveCode);
                //}
                //else
                //{
                //    data.Add("ReceiveAddress", ReceiveCode);
                //}
                //for (int i = 0; i < bgn_row; i++)
                //{
                //    data.Add("ReceivePosition", ReceiveCode);
                //}
            }
            #endregion
            #region Contact person/E-mail 联系人/电子邮件
            int index_contact = dataArr.IndexOf("Contact person/E-mail 联系人/电子邮件");
            if (index_contact > -1)
            {
                data.Add("ContactName", dataArr[index_contact + 1]);
                data.Add("ContactNameEmail", dataArr[index_contact + 2]);
                data.Add("ContactNameTel", dataArr[index_contact + 3].Split(':')[1].TrimStart());
                data.Add("Email", dataArr[index_contact + 4].Split(':')[1].Trim());
            }
            int index_contact2 = dataArr.IndexOf("Contact person/E-mail");
            if (index_contact2 > -1)
            {
                data.Add("ContactName", dataArr[index_contact2 + 1]);
                data.Add("ContactNameEmail", dataArr[index_contact2 + 2]);
                int order_index = dataArr.IndexOf("Your Order no./date");
                //Tel: / Fax:
                string str = dataArr[index_contact2 + 3];
                string[] arr = str.Split('/');
                data.Add("ContactNameTel", arr[0].Split(':')[1].Trim());
                if (order_index - (index_contact2 + 3) == 1)
                {
                    data.Add("ContactNameFax", arr[1].Split(':')[1].Trim());
                }
                if (order_index - (index_contact2 + 3) > 1)
                {
                    data.Add("ContactNameFax", (arr[1].Split(':')[1] + dataArr[index_contact2 + 4]).Trim());
                }
            }
            #endregion
            #region Your Order no./date 贵司订单号码/日期
            int index_order = dataArr.IndexOf("Your Order no./date 贵司订单号码/日期");
            if (index_order < 0)
            {
                index_order = dataArr.IndexOf("Your Order no./date");
            }
            if (index_order > -1)
            {
                string[] strArr = dataArr[index_order + 1].Split('/');
                data.Add("OrderCode", strArr[0].Trim());
                data.Add("OrderDate", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region Our ref/date 我司参考号码/日期
            int index_ourref = dataArr.IndexOf("Our ref/date 我司参考号码/日期");
            if (index_ourref < 0)
            {
                index_ourref = dataArr.IndexOf("Our ref/date");
            }
            if (index_ourref > -1)
            {
                string[] strArr = dataArr[index_ourref + 1].Split('/');
                data.Add("OurRefCode", strArr[0].Trim());
                data.Add("OurRefDate", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region Address
            int curr_index = dataArr.IndexOf("______________________________________________________________________________");
            int index_add = curr_index - 1;
            if (index_add >= 0)
            {
                //当数组len<6时，取上一个索引值
                while (dataArr[index_add].Split(',').Length < 6 && index_add > 0)
                {
                    index_add--;
                }
                string[] strArr = dataArr[index_add].Split(',');
                if (strArr.Length >= 6 && index_add >= 0)
                {
                    #region 规则数据格式 
                    data.Add("ReceiptName", strArr[0]);
                    data.Add("ReceiptAddress", strArr[1]);
                    data.Add("ReceiptZipCode", strArr[2]);
                    data.Add("ReceiptPeople", strArr[3].Trim());
                    data.Add("ReceiptTel", strArr[4]);
                    string mobile = GetNumberOfStr(strArr[5]);
                    if (strArr.Length == 6)
                    {
                        if (mobile.Length < 11)
                        {
                            mobile += dataArr[index_add + 1];
                        }
                    }
                    data.Add("ReceiptMobile", mobile);
                    if (strArr.Length > 6)
                    {
                        int k = index_add;
                        string remark = strArr[6];
                        while ((curr_index - k) > 1 && k < curr_index)
                        {
                            remark += dataArr[k + 1];
                            k++;
                        }
                        data.Add("ReceiptRemark", remark);
                    }
                    #endregion
                }
                else
                {
                    #region 不规则数据
                    int index = 0;
                    string strAddr = dataArr[index];
                    if (strAddr.Contains("PARTIAL DELIVERY"))
                        strAddr = "";
                    while (strAddr.Split(',').Length < 6 && index < (curr_index - 1))
                    {
                        index++;
                        if (!dataArr[index].Contains("部分发货") && !dataArr[index].Contains("PARTIAL DELIVERY"))
                            strAddr += dataArr[index];
                    }
                    string[] arrAddr = strAddr.Split(',');
                    if (arrAddr.Length == 6)
                    {
                        data.Add("ReceiptName", arrAddr[0]);
                        data.Add("ReceiptAddress", arrAddr[1]);
                        data.Add("ReceiptZipCode", arrAddr[2]);
                        data.Add("ReceiptPeople", arrAddr[3].Trim());
                        data.Add("ReceiptTel", arrAddr[4]);
                        data.Add("ReceiptMobile", arrAddr[5]);
                    }
                    else if (arrAddr.Length == 5)
                    {
                        data.Add("ReceiptAddress", arrAddr[0]);
                        data.Add("ReceiptZipCode", arrAddr[1]);
                        data.Add("ReceiptPeople", arrAddr[2]);
                        data.Add("ReceiptTel", arrAddr[3].Trim());
                        data.Add("ReceiptMobile", arrAddr[4]);
                    }
                    else
                    {
                        string remark = "";
                        for (int i = 0; i < curr_index; i++)
                        {
                            if (!dataArr[i].Contains("部分发货") && !dataArr[i].Contains("PARTIAL DELIVERY"))
                            {
                                remark += dataArr[i];
                            }
                        }
                        data.Add("ReceiptRemark", remark);
                    }
                    #endregion
                }
            }
            #endregion
            #region 中文版PDF获取明细获取方式
            int check_CH = dataArr.IndexOf("物料描述 库位");
            if (check_CH > 0)
            {
                //明细开始索引
                int begIndex = check_CH + 2;
                //明细结束索引
                int check_index = dataArr.IndexOf("Shipment details 发货批次详述");
                #region 数据分页，索引获取
                int endIndex = 0;
                if (check_index > 0)
                {
                    endIndex = dataArr.LastIndexOf("______________________________________________________________________________") - 1;
                }
                else
                {
                    endIndex = dataArr.LastIndexOf("Sold-to address 售达方地址") - 1;
                }
                #endregion
                //明细的行数
                int contentRow = (endIndex - begIndex) / 4;
                if (contentRow > 0)
                {
                    #region 获取明细
                    //明细当前行索引
                    for (int i = 0; i < contentRow; i++)
                    {
                        int index = (begIndex + (i * 4));
                        Dictionary<string, string> detail = new Dictionary<string, string>();
                        while (dataArr[index].Split(' ').Length < 10 ||
                            dataArr[index].Split(' ').Length > 15 ||
                            dataArr[index].Split(' ')[0].Length != 6)
                        {
                            index++;
                        }
                        string firstRow = RemoveStrSpace(dataArr[index]);
                        string[] firstArr = firstRow.Replace("\\s+", " ").Split(' ');
                        string towRow = dataArr[index + 1];
                        string threeRow = dataArr[index + 2];
                        string fourRow = dataArr[index + 3];
                        #region 多行物料描述操作
                        string fiveRow = dataArr[index + 4];
                        string[] fiveRowArr = fiveRow.Split(' ');
                        if ((fiveRowArr.Length < 10
                            || fiveRowArr[0].Length != 6
                            || fiveRowArr.Length > 15)
                            && (index + 4) < endIndex)
                        {
                            fourRow += " " + fiveRow;
                        }
                        string sixRow = dataArr[index + 5];
                        string[] sixRowArr = sixRow.Split(' ');
                        if ((sixRowArr.Length < 10
                            || sixRowArr[0].Length != 6
                            || sixRowArr.Length > 15)
                            && (index + 5) < endIndex)
                        {
                            fourRow += " " + sixRow;
                        }
                        #endregion
                        detail.Add("ProjectRow", firstArr[0]);
                        detail.Add("MaterialCode", firstArr[1]);
                        string strNum = firstArr[2];
                        if (firstArr.Length > 3)
                        {
                            strNum = firstArr[firstArr.Length - 1];
                        }
                        string num1 = GetNumberOfStr(strNum);
                        string unit = strNum.Replace(num1, "");
                        string[] towarr = towRow.Split('/');
                        detail.Add("Number", num1);
                        detail.Add("Number2", GetNumberOfStr(towarr[0]));
                        detail.Add("Number3", towarr[1]);
                        detail.Add("Unit", unit);
                        detail.Add("MaterialDesc", fourRow);
                        detail.Add("IssueLocation", threeRow);
                        list.Add(detail);
                    }
                    #endregion
                }
            }
            #endregion
            #region 英文版PDF获取明细获取方式
            int check_EN = dataArr.IndexOf("Description ##");
            if (check_EN > 0)
            {
                //明细开始索引
                int begIndex = check_EN + 2;
                //明细结束索引
                int check_index = dataArr.IndexOf("Shipment details");
                #region 数据分页，索引获取
                int endIndex = 0;
                if (check_index > 0)
                {
                    endIndex = dataArr.LastIndexOf("______________________________________________________________________________");
                }
                else
                {
                    endIndex = dataArr.LastIndexOf("Sold-to address 售达方地址");
                }
                #endregion
                //明细的行数
                int contentRow = (endIndex - begIndex) / 3;
                if (contentRow > 0)
                {
                    #region 获取明细
                    //明细当前行索引
                    for (int i = 0; i < contentRow; i++)
                    {
                        int index = (begIndex + (i * 3));
                        Dictionary<string, string> detail = new Dictionary<string, string>();
                        while (dataArr[index].Split(' ').Length < 10
                            || dataArr[index].Split(' ')[0].Length != 6)
                        {
                            index++;
                        }
                        string firstRow = RemoveStrSpace(dataArr[index]);
                        string[] firstArr = firstRow.Replace("\\s+", " ").Split(' ');
                        string towRow = dataArr[index + 1];
                        string threeRow = dataArr[index + 2];
                        #region 多行物料描述操作
                        string fourRow = dataArr[index + 3];
                        if ((fourRow.Split(' ').Length < 10 
                            || fourRow.Split(' ')[0].Length != 6) 
                            && (index + 4) < endIndex)
                        {
                            threeRow += " " + fourRow;
                        }
                        string fiveRow = dataArr[index + 4];
                        if ((fiveRow.Split(' ').Length < 10
                            || fiveRow.Split(' ')[0].Length != 6)
                            && (index + 5) < endIndex)
                        {
                            threeRow += " " + fiveRow;
                        }
                        #endregion
                        detail.Add("ProjectRow", firstArr[0]);
                        detail.Add("MaterialCode", firstArr[1]);
                        string num = GetNumberOfStr(firstArr[2]);
                        string unit = firstArr[2].Replace(num, "");
                        detail.Add("Number", num);
                        detail.Add("Unit", unit);
                        detail.Add("MaterialDesc", threeRow);
                        detail.Add("IssueLocation", towRow);
                        list.Add(detail);
                    }
                    #endregion
                }
            }
            #endregion
            #region Shipment details 发货批次详述
            int index_shipment = dataArr.IndexOf("Shipment details 发货批次详述");
            if (index_shipment < 0)
            {
                index_shipment = dataArr.IndexOf("Shipment details");
            }
            if (index_shipment > -1)
            {
                string ShippingCond = dataArr[index_shipment + 1];
                if (ShippingCond.Contains("Shipping conditions"))
                    data.Add("ShippingCond", ShippingCond.Split(':')[1].Trim());
                string TermsOfDelivery = dataArr[index_shipment + 2];
                if (TermsOfDelivery.Contains("Terms of delivery"))
                    data.Add("TermsOfDelivery", TermsOfDelivery.Split(':')[1].Trim());
                string Gross = dataArr[index_shipment + 3];
                if (Gross.Contains("Gross weight"))
                {
                    string[] GrossWeight = RemoveStrSpace(Gross).Split(' ');
                    data.Add("GrossWeight", GrossWeight[GrossWeight.Length - 2]);
                    data.Add("GrossWeightUint", GrossWeight[GrossWeight.Length - 1]);
                }
                string Net = dataArr[index_shipment + 4];
                if (Net.Contains("Net weight"))
                {
                    string[] NetWeight = RemoveStrSpace(Net).Split(' ');
                    data.Add("NetWeight", NetWeight[NetWeight.Length - 2]);
                    data.Add("NetWeightUint", NetWeight[NetWeight.Length - 1]);
                }
                string Vol = dataArr[index_shipment + 5];
                if (Vol.Contains("Volumes"))
                {
                    string[] Volumes = RemoveStrSpace(Vol).Split(' ');
                    data.Add("Volumes", Volumes[Volumes.Length - 2]);
                    data.Add("VolumesUint", Volumes[Volumes.Length - 1]);
                }
            }
            #endregion
        }
        /// <summary>
        /// 把一个或多个空格替换成一个空格
        /// </summary>
        /// <returns></returns>
        public string RemoveStrSpace(string str)
        {
            return new Regex("[\\s]+").Replace(str, " ");
        }
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetNumberOfStr(string str)
        {
            return new Regex("[^\\d.\\d]").Replace(str, "");
        }
        /// <summary>
        /// 验证一个字符串是否是纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool CheckStrIsNumber(string str)
        {
            return new Regex("^\\d+$").IsMatch(str);
        }
        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";
            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        public string[] GetPDFPaths()
        {
            String path = @"C:\Users\Administrator\Desktop\pdf\发货\发货";
            //path = @"C:\Users\Administrator\Desktop\pdf\CDC\发货";
            return Directory.GetFiles(path, "*.pdf");
        }
        private void btn_adc_Click(object sender, EventArgs e)
        {
            #region MyRegion
            //if (string.IsNullOrWhiteSpace(txtID.Text))
            //{
            //    MessageBox.Show("请选择PDF文件", "提示");
            //}
            //else
            //{
            //    try
            //    {
            //        PdfReader reader = new PdfReader(txtID.Text);
            //        PdfReaderContentParser parser = new PdfReaderContentParser(reader);

            //        string jsonData = string.Empty;
            //        Dictionary<string, string> data = new Dictionary<string, string>();
            //        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            //        ITextExtractionStrategy strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(1, new SimpleTextExtractionStrategy());
            //        string contentOfPage = strategy.GetResultantText();

            //        string content = PdfTextExtractor.GetTextFromPage(reader, 1);
            //        List<string> dataArr = contentOfPage.Split('\n').ToList();
            //        List<string> dataArr2 = content.Split('\n').ToList();
            //        richTextBox.Text = contentOfPage;
            //    }
            //    catch (Exception ex)
            //    {
            //        richTextBox.Text = ex.Message;
            //    }
            //}
            #endregion
            string[] asd = GetPDFPaths();
            #region 测试所有文件下的PDF文件
            int index = 0;
            try
            {
                Dictionary<string, Dictionary<string, object>> AllData = new Dictionary<string, Dictionary<string, object>>();
                string[] files = GetPDFPaths();
                foreach (var file in files)
                {
                    index++;
                    //if (index == 713)
                    //{

                    //}
                    PdfReader reader = new PdfReader(file);
                    PdfReaderContentParser parser = new PdfReaderContentParser(reader);
                    #region 测试代码
                    //string content = PdfTextExtractor.GetTextFromPage(reader, 1);
                    //richTextBox.Text = content;

                    //ITextExtractionStrategy strategy1 = parser.ProcessContent<SimpleTextExtractionStrategy>(1, new SimpleTextExtractionStrategy());
                    //string asd = strategy1.GetResultantText();
                    //richTextBox.Text = asd;
                    //List<string> da = asd.Split('\n').ToList();
                    //return;
                    #endregion
                    int Pdf_Pages = reader.NumberOfPages;
                    string jsonData = string.Empty;
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                    for (int i = 1; i <= Pdf_Pages; i++)
                    {
                        ITextExtractionStrategy strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(i, new SimpleTextExtractionStrategy());
                        string contentOfPage = strategy.GetResultantText();
                        GetPDFText(contentOfPage, data, list);
                    }
                    Dictionary<string, object> Obj = new Dictionary<string, object>();
                    Obj.Add("Head", data);
                    Obj.Add("Data", list);
                    //richTextBox.Text = SerializeDictionaryToJsonString(Obj);
                    string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                    //string txt = SerializeDictionaryToJsonString(Obj);
                    AllData.Add(filename, Obj);
                    #region 写入Txt文件
                    //string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                    //string path = @"C:\Users\Administrator\Desktop\pdf\发货\发货txt\" + filename + ".txt";
                    //if (!File.Exists(path))
                    //{
                    //    FileStream fs = File.Create(path);
                    //    fs.Close();
                    //}
                    //FileStream FS = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    //StreamWriter sw = new StreamWriter(FS);
                    //sw.WriteLine(txt);
                    //sw.Flush();
                    //sw.Close();
                    //FS.Close();
                    #endregion
                }
                richTextBox.Text = SerializeDictionaryToJsonString(AllData);
            }
            catch (Exception ex)
            {

                richTextBox.Text = index.ToString() + ex.Message;
            }
            #endregion
        }
    }
}
