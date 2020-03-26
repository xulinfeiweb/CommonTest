using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Forms;
using System.Reflection;
using TestReflex;
using System.Xml;

namespace PDF_Service
{
    public partial class PDF_Form : Form
    {
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

                PdfWriter _writer;
                Document _doc = new Document(PageSize.A4.Rotate(), 20, 20, 10, 0);
                _writer = PdfWriter.GetInstance(_doc, new FileStream(path, FileMode.Create));
                _doc.Open();

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
                        img.ScaleAbsolute(72, 72);
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
            string file = GetPath(GetValue("PDF_FileType"));
            this.txtID.Text = file;
        }
        private void btn_img_Click(object sender, EventArgs e)
        {
            string file = GetPath(GetValue("Img_FileType"));
            this.txt_img_path.Text = file;
        }
        private void btn_pdf_Click(object sender, EventArgs e)
        {
            string file = GetPath(GetValue("PDF_FileType"));
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
                    int currPage = 1;//PDF当前页，默认为第一页
                    for (int i = 1; i <= Pdf_Pages; i++)
                    {
                        ITextExtractionStrategy strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(i, new SimpleTextExtractionStrategy());
                        string contentOfPage = strategy.GetResultantText();
                        if (currPage != 1 && currPage != Pdf_Pages)
                            GetPDFText(contentOfPage, data, list, currPage);
                        else
                            GetPDFText(contentOfPage, data, list);
                        currPage++;
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
        public void GetPDFText(string Text,
            Dictionary<string, string> data,
            List<Dictionary<string, string>> list,
            int currPage = 0)
        {
            string PDF_Line = GetValue("PDF_Detail_Line");
            List<string> dataArr = Text.Split('\n').ToList();
            int line = dataArr.IndexOf(PDF_Line);
            if (line < 0)
                PDF_Line = GetValue("PDF_Detail_Line2");
            #region Number/date 单号/日期
            int index_number = dataArr.IndexOf(GetValue("Number_date"));
            if (index_number < 0)
            {
                index_number = dataArr.IndexOf(GetValue("Number_date2"));
            }
            string ShipCode = "";
            if (index_number > -1)
            {
                //int index_Ourref = dataArr.IndexOf(Config.GetValue("Our_ref_date"));
                //string[] strArr = null;
                //if ((index_Ourref - index_order) > 2)
                //{
                //    string orderStr = dataArr[index_order + 1] + dataArr[index_order + 2];
                //    strArr = orderStr.Split('/');
                //}
                //else
                //    strArr = dataArr[index_order + 1].Split('/');
                //data.Add("ORDNUM", strArr[0].Trim());
                //data.Add("ORDDAT", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);

                string[] strArr = dataArr[index_number + 1].Split('/');
                ShipCode = strArr[0].Trim();
                data.Add("ShipCode", strArr[0].Trim());
                data.Add("ShipDate", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region Ship-to address 收货方地址
            //收货方地址中字段数量不明确
            int index_shop = dataArr.IndexOf(GetValue("Ship_to_address"));
            if (index_shop < 0)
            {
                index_shop = dataArr.IndexOf(GetValue("Ship_to_address2"));
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
            int index_contact = dataArr.IndexOf(GetValue("Contact_person"));
            if (index_contact > -1)
            {
                data.Add("ContactName", dataArr[index_contact + 1]);
                data.Add("ContactNameEmail", dataArr[index_contact + 2]);
                data.Add("ContactNameTel", dataArr[index_contact + 3].Split(':')[1].TrimStart());
                data.Add("Email", dataArr[index_contact + 4].Split(':')[1].Trim());
            }
            int index_contact2 = dataArr.IndexOf(GetValue("Contact_person2"));
            if (index_contact2 > -1)
            {
                data.Add("ContactName", dataArr[index_contact2 + 1]);
                data.Add("ContactNameEmail", dataArr[index_contact2 + 2]);
                int order_index = dataArr.IndexOf(GetValue("Order_no_date2"));
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
            int index_order = dataArr.IndexOf(GetValue("Order_no_date"));
            if (index_order < 0)
            {
                index_order = dataArr.IndexOf(GetValue("Order_no_date2"));
            }
            if (index_order > -1)
            {
                string[] strArr = dataArr[index_order + 1].Split('/');
                data.Add("OrderCode", strArr[0].Trim());
                data.Add("OrderDate", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region Our ref/date 我司参考号码/日期
            int index_ourref = dataArr.IndexOf(GetValue("Our_ref"));
            if (index_ourref < 0)
            {
                index_ourref = dataArr.IndexOf(GetValue("Our_ref2"));
            }
            if (index_ourref > -1)
            {
                string[] strArr = dataArr[index_ourref + 1].Split('/');
                data.Add("OurRefCode", strArr[0].Trim());
                data.Add("OurRefDate", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region  PDF地址信息 Address
            int curr_index = dataArr.IndexOf(PDF_Line);
            int index_add = curr_index - 1;
            if (index_add >= 0)
            {
                //当数组len<6时，取上一个索引值。英文逗号分隔
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
                        if (mobile.Length > 11)
                        {
                            mobile = mobile.Substring(0, 11);
                        }
                    }
                    data.Add("ReceiptMobile", GetNumberOfStr(mobile));
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
                    if (arrAddr.Length >= 6)
                    {
                        data.Add("ReceiptName", arrAddr[0]);
                        data.Add("ReceiptAddress", arrAddr[1]);
                        data.Add("ReceiptZipCode", arrAddr[2]);
                        data.Add("ReceiptPeople", arrAddr[3].Trim());
                        data.Add("ReceiptTel", arrAddr[4]);
                        string mobile = GetNumberOfStr(arrAddr[5]);
                        data.Add("ReceiptMobile", mobile);
                    }
                    else
                    {
                        #region 中文逗号分隔
                        int ch_index = 0;
                        //当数组len<6时，取上一个索引值
                        string chstr = dataArr[ch_index];
                        if (chstr.Contains("PARTIAL DELIVERY"))
                            chstr = "";
                        while (dataArr[ch_index].Split('，').Length < 6 && ch_index < (curr_index - 1))
                        {
                            ch_index++;
                            if (!dataArr[ch_index].Contains("部分发货") && !dataArr[ch_index].Contains("PARTIAL DELIVERY"))
                                chstr += dataArr[ch_index];
                        }
                        string[] chAddr = chstr.Split('，');
                        if (chAddr.Length >= 6)
                        {
                            data.Add("ReceiptName", chAddr[0]);
                            data.Add("ReceiptAddress", chAddr[1]);
                            data.Add("ReceiptZipCode", chAddr[2]);
                            data.Add("ReceiptPeople", chAddr[3].Trim());
                            data.Add("ReceiptTel", chAddr[4]);
                            string mobile = GetNumberOfStr(chAddr[5]);
                            data.Add("ReceiptMobile", mobile);
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
                #region MyRegion
                int endIndex = 0;
                //集合中相同名称的元素个数
                int Count = dataArr.Where(p => p.Contains(PDF_Line)).Count();
                if (Count < 3)
                {
                    //明细结束索引
                    endIndex = dataArr.IndexOf(GetValue("Sold_to_address"));
                    if (endIndex < 0)
                    {
                        //只有是中间页时，才会需要当前索引
                        if (currPage > 0)
                            //明细结束索引
                            endIndex = dataArr.IndexOf(GetValue("EndIndex"));
                    }
                }
                else
                {
                    endIndex = dataArr.LastIndexOf(PDF_Line);
                    if (Count == 4)
                        endIndex--;
                }
                #endregion
                //明细的行数
                int contentRow = (endIndex - begIndex) / 4;
                if (contentRow > 0)
                    GetDetailList(list, dataArr, begIndex, endIndex, contentRow);
            }
            #endregion
            #region 英文版PDF获取明细获取方式
            int check_EN = dataArr.IndexOf("Description ##");
            if (check_EN > 0)
            {
                //明细开始索引
                int begIndex = check_EN + 2;
                //明细结束索引
                int check_index = dataArr.IndexOf(GetValue("Shipment_details2"));
                #region 数据分页，索引获取
                int endIndex = 0;
                if (check_index > 0)
                {
                    endIndex = dataArr.LastIndexOf(PDF_Line);
                }
                else
                {
                    endIndex = dataArr.IndexOf(GetValue("Sold_to_address"));
                }
                #endregion
                //明细的行数
                int contentRow = (endIndex - begIndex) / 3;
                if (contentRow > 0)
                {
                    #region 获取明细
                    //明细当前行索引
                    //for (int i = 0; i < contentRow; i++)
                    //{
                    //    int index = (begIndex + (i * 3));
                    //    Dictionary<string, string> detail = new Dictionary<string, string>();
                    //    while (dataArr[index].Split(' ').Length < 10
                    //        || dataArr[index].Split(' ')[0].Length != 6)
                    //    {
                    //        index++;
                    //    }
                    //    string firstRow = RemoveStrSpace(dataArr[index]);
                    //    string[] firstArr = firstRow.Replace("\\s+", " ").Split(' ');
                    //    string towRow = dataArr[index + 1];
                    //    string threeRow = dataArr[index + 2];
                    //    #region 多行物料描述操作
                    //    string fourRow = dataArr[index + 3];
                    //    if ((fourRow.Split(' ').Length < 10
                    //        || fourRow.Split(' ')[0].Length != 6)
                    //        && (index + 4) < endIndex)
                    //    {
                    //        threeRow += " " + fourRow;
                    //    }
                    //    string fiveRow = dataArr[index + 4];
                    //    if ((fiveRow.Split(' ').Length < 10
                    //        || fiveRow.Split(' ')[0].Length != 6)
                    //        && (index + 5) < endIndex)
                    //    {
                    //        threeRow += " " + fiveRow;
                    //    }
                    //    #endregion
                    //    detail.Add("ProjectRow", firstArr[0]);
                    //    detail.Add("MaterialCode", firstArr[1]);
                    //    string num = GetNumberOfStr(firstArr[2]);
                    //    string unit = firstArr[2].Replace(num, "");
                    //    detail.Add("Number", num);
                    //    detail.Add("Unit", unit);
                    //    detail.Add("MaterialDesc", threeRow);
                    //    detail.Add("IssueLocation", towRow);
                    //    list.Add(detail);
                    //}
                    #endregion

                    #region 获取明细
                    //明细当前最后行索引
                    int LastIndex = begIndex;
                    for (int i = 0; i < contentRow; i++)
                    {
                        //最后行索引不能大于结束索引
                        if (LastIndex < endIndex)
                        {
                            LastIndex += 3;
                            int index = (begIndex + (i * 3));
                            Dictionary<string, string> detail = new Dictionary<string, string>();
                            while (dataArr[index].Split(' ').Length < 10
                            || dataArr[index].Split(' ')[0].Length != 6)
                            {
                                index++;
                            }
                            string firstRow = RemoveStrSpace(dataArr[index]);
                            string[] firstArr = firstRow.Replace("\\s+", " ").Split(' ');
                            string towRow = dataArr[index + 1],
                             threeRow = dataArr[index + 2];
                            #region 多行物料描述操作
                            while ((dataArr[index + 3].Split(' ')[0].Length != 6
                                || !CheckStrIsNumber(dataArr[index + 4].Split(' ')[0])
                                || dataArr[index + 3].Split(' ').Length < 10)
                                && (index + 3) < endIndex)
                            {
                                threeRow = threeRow + " " + dataArr[index + 3];
                                index++;
                                LastIndex++;
                            }
                            #endregion
                            detail.Add("ProjectRow", firstArr[0]);
                            detail.Add("MTR", firstArr[1]);
                            string num = GetNumberOfStr(firstArr[2]);
                            string unit = firstArr[2].Replace(num, "");
                            detail.Add("MTRQTY", num);
                            detail.Add("MTRSN", "");
                            detail.Add("MTRUNT", unit);
                            detail.Add("MaterialDesc", threeRow);
                            detail.Add("LOC", towRow);
                            list.Add(detail);
                        }
                    }
                    #endregion
                }
            }
            #endregion
            #region Shipment details 发货批次详述
            //获取发货批次详述开始索引
            int index_shipment = dataArr.IndexOf(GetValue("Shipment_details"));
            if (index_shipment < 0)
            {
                index_shipment = dataArr.IndexOf(GetValue("Shipment_details2"));
                if (index_shipment < 0)
                {
                    index_shipment = dataArr.LastIndexOf(PDF_Line);
                }
            }
            if (index_shipment > 0)
            {
                //循环判断是否存在，并赋值。
                for (int i = 1; i <= 5; i++)
                {
                    string DataStr = dataArr[index_shipment + i];
                    if (DataStr.Contains("Shipping conditions"))
                        data.Add("ShippingCond", DataStr.Split(':')[1].Trim());
                    if (DataStr.Contains("Terms of delivery"))
                        data.Add("TermsOfDelivery", DataStr.Split(':')[1].Trim());
                    if (DataStr.Contains("Gross weight"))
                    {
                        string[] GrossWeight = RemoveStrSpace(DataStr).Split(' ');
                        data.Add("GrossWeight", GrossWeight[GrossWeight.Length - 2]);
                        data.Add("GrossWeightUint", GrossWeight[GrossWeight.Length - 1]);
                    }
                    if (DataStr.Contains("Net weight"))
                    {
                        string[] NetWeight = RemoveStrSpace(DataStr).Split(' ');
                        data.Add("NetWeight", NetWeight[NetWeight.Length - 2]);
                        data.Add("NetWeightUint", NetWeight[NetWeight.Length - 1]);
                    }
                    if (DataStr.Contains("Volumes"))
                    {
                        string[] Volumes = RemoveStrSpace(DataStr).Split(' ');
                        data.Add("Volumes", Volumes[Volumes.Length - 2]);
                        data.Add("VolumesUint", Volumes[Volumes.Length - 1]);
                    }
                }
            }
            #endregion
        }

        private void GetDetailList(List<Dictionary<string, string>> list, List<string> dataArr, int begIndex, int endIndex, int contentRow)
        {
            #region 获取明细
            //明细当前最后行索引
            int LastIndex = begIndex;
            for (int i = 0; i < contentRow; i++)
            {
                //最后行索引不能大于结束索引
                if (LastIndex < endIndex)
                {
                    LastIndex += 4;
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
                    string towRow = dataArr[index + 1],
                           threeRow = dataArr[index + 2],
                           fourRow = dataArr[index + 3];
                    #region 多行物料描述操作
                    while ((dataArr[index + 4].Split(' ')[0].Length != 6
                        || !CheckStrIsNumber(dataArr[index + 4].Split(' ')[0])
                        || dataArr[index + 4].Split(' ').Length < 10
                        || dataArr[index + 4].Split(' ').Length > 15)
                        && (index + 4) < endIndex)
                    {
                        fourRow = fourRow + " " + dataArr[index + 4];
                        index++;
                        LastIndex++;
                    }
                    #endregion
                    string ProjectRow = firstArr[0];
                    detail.Add("ProjectRow", ProjectRow);
                    string strNum = firstArr[2];
                    if (firstArr.Length == 3)
                    {
                        detail.Add("MaterialCode", firstArr[1].Trim());
                    }
                    if (firstArr.Length > 3)
                    {
                        strNum = firstArr[firstArr.Length - 1];
                        string code = string.Empty;
                        for (int j = 0; j < firstArr.Length; j++)
                        {
                            if (firstArr[j] != ProjectRow && firstArr[j] != strNum)
                                code += " " + firstArr[j];
                        }
                        detail.Add("MaterialCode", code.Trim());
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
            return JsonConvert.SerializeObject(dict);
        }
        /// <summary>
        /// jsonToxml
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Json2XML(string str, string root)
        {
            string result = null;
            //XmlDocument xml = JsonConvert.DeserializeXmlNode(str);
            XmlDocument xml = (XmlDocument)JsonConvert.DeserializeXmlNode(str, root);
            result = xml.OuterXml;
            return result;
        }
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        public string[] GetPDFPaths(string file)
        {
            //String path = @"C:\Users\Administrator\Desktop\pdf\发货\发货";
            //path = @"C:\Users\Administrator\Desktop\pdf\CDC\发货";
            //path = @"C:\Users\Administrator\Desktop\pdf\发货2\发货";
            return Directory.GetFiles(file, "*.pdf");
        }
        private void btn_adc_Click(object sender, EventArgs e)
        {
            TransactionScope scope = new TransactionScope();
            scope.Complete();
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
            #region 测试所有文件下的PDF文件
            int index = 0;
            string err = "";
            try
            {
                Dictionary<string, Dictionary<string, object>> AllData = new Dictionary<string, Dictionary<string, object>>();
                string[] files = GetPDFPaths(textBox1.Text.Trim());
                foreach (var file in files)
                {
                    index++;
                    //if (index == 61)
                    //{

                    //}
                    err = file;
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
                    int currPage = 1;//PDF当前页，默认为第一页
                    for (int i = 1; i <= Pdf_Pages; i++)
                    {
                        ITextExtractionStrategy strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(i, new SimpleTextExtractionStrategy());
                        string contentOfPage = strategy.GetResultantText();
                        if (currPage != 1 && currPage != Pdf_Pages)
                            GetPDFText(contentOfPage, data, list, currPage);
                        else
                            GetPDFText(contentOfPage, data, list);
                        currPage++;
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
                richTextBox.Text = err + "*********" + index.ToString() + ex.Message;
            }
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
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
                    int currPage = 1;//PDF当前页，默认为第一页
                    for (int i = 1; i <= Pdf_Pages; i++)
                    {
                        ITextExtractionStrategy strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(i, new SimpleTextExtractionStrategy());
                        string contentOfPage = strategy.GetResultantText();
                        if (currPage != 1 && currPage != Pdf_Pages)
                            GetPDFTextToXML(contentOfPage, data, list, currPage);
                        else
                            GetPDFTextToXML(contentOfPage, data, list);
                        currPage++;
                    }
                    Dictionary<string, object> detial = new Dictionary<string, object>();
                    detial.Add("LN", list);
                    Dictionary<string, object> Obj = new Dictionary<string, object>();
                    Obj.Add("DN_HEAD", data);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("CRETIM", DateTime.Now.ToString());
                    Obj.Add("EDI_DOC", dic);
                    Obj.Add("DN_LNS", detial);
                    string json = SerializeDictionaryToJsonString(Obj);
                    string xml = Json2XML(json.ToString(), "IDOC");
                    string result = @"<DELVRY>" + xml + "</DELVRY>";
                    XmlDocument XmlLoad = new XmlDocument();
                    XmlLoad.LoadXml(result);
                    string code = XmlLoad.SelectSingleNode("DELVRY/IDOC/DN_HEAD/DNNUM").InnerText;
                    richTextBox.Text = result;
                }
                catch (Exception ex)
                {
                    richTextBox.Text = ex.Message;
                }
            }
        }
        /// <summary>
        /// PDF内容转xml
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="data"></param>
        /// <param name="list"></param>
        /// <param name="currPage"></param>
        public void GetPDFTextToXML(string Text,
           Dictionary<string, string> data,
           List<Dictionary<string, string>> list,
           int currPage = 0)
        {
            string PDF_Line = GetValue("PDF_Detail_Line");
            List<string> dataArr = Text.Split('\n').ToList();
            int line = dataArr.IndexOf(PDF_Line);
            if (line < 0)
                PDF_Line = GetValue("PDF_Detail_Line2");
            #region Number/date 单号/日期
            int index_number = dataArr.IndexOf(GetValue("Number_date"));
            if (index_number < 0)
            {
                index_number = dataArr.IndexOf(GetValue("Number_date2"));
            }
            string ShipCode = "";
            if (index_number > -1)
            {
                string[] strArr = dataArr[index_number + 1].Split('/');
                ShipCode = strArr[0].Trim();
                data.Add("DNNUM", strArr[0].Trim());
            }
            #endregion
            #region Your Order no./date 贵司订单号码/日期
            int index_order = dataArr.IndexOf(GetValue("Order_no_date"));
            if (index_order < 0)
            {
                index_order = dataArr.IndexOf(GetValue("Order_no_date2"));
            }
            if (index_order > -1)
            {
                string[] strArr = dataArr[index_order + 1].Split('/');
                data.Add("ORDNUM", strArr[0].Trim());
                data.Add("ORDDAT", strArr[3] + "-" + strArr[1].Trim() + "-" + strArr[2]);
            }
            #endregion
            #region  PDF地址信息 Address
            int curr_index = dataArr.IndexOf(PDF_Line);
            int index_add = curr_index - 1;
            if (index_add >= 0)
            {
                //当数组len<6时，取上一个索引值。英文逗号分隔
                while (dataArr[index_add].Split(',').Length < 6 && index_add > 0)
                {
                    index_add--;
                }
                string[] strArr = dataArr[index_add].Split(',');
                if (strArr.Length >= 6 && index_add >= 0)
                {
                    #region 规则数据格式 
                    data.Add("RCPCMP", strArr[0]);
                    data.Add("RCPADDR", strArr[1]);
                    data.Add("RCPZIP", strArr[2]);
                    data.Add("RCPNAM", strArr[3].Trim());
                    data.Add("RCPTEL", strArr[4]);
                    string mobile = GetNumberOfStr(strArr[5]);
                    if (strArr.Length == 6)
                    {
                        if (mobile.Length < 11)
                        {
                            mobile += dataArr[index_add + 1];
                        }
                    }
                    data.Add("RCPMBL", GetNumberOfStr(mobile));
                    data.Add("CRTDAT", DateTime.Now.ToString());
                    if (strArr.Length > 6)
                    {
                        int k = index_add;
                        string remark = strArr[6];
                        while ((curr_index - k) > 1 && k < curr_index)
                        {
                            remark += dataArr[k + 1];
                            k++;
                        }
                        data.Add("RCPCMP", "");
                        data.Add("RCPADDR", remark);
                        data.Add("RCPZIP", "");
                        data.Add("RCPNAM", "");
                        data.Add("RCPTEL", "");
                        data.Add("RCPMBL", "");
                        data.Add("CRTDAT", DateTime.Now.ToString());
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
                    if (arrAddr.Length >= 6)
                    {
                        data.Add("RCPCMP", arrAddr[0]);
                        data.Add("RCPADDR", arrAddr[1]);
                        data.Add("RCPZIP", arrAddr[2]);
                        data.Add("RCPNAM", arrAddr[3].Trim());
                        data.Add("RCPTEL", arrAddr[4]);
                        string mobile = GetNumberOfStr(arrAddr[5]);
                        data.Add("RCPMBL", mobile);
                        data.Add("CRTDAT", DateTime.Now.ToString());
                    }
                    else
                    {
                        #region 中文逗号分隔
                        int ch_index = 0;
                        //当数组len<6时，取上一个索引值
                        string chstr = dataArr[ch_index];
                        if (chstr.Contains("PARTIAL DELIVERY"))
                            chstr = "";
                        while (dataArr[ch_index].Split('，').Length < 6 && ch_index < (curr_index - 1))
                        {
                            ch_index++;
                            if (!dataArr[ch_index].Contains("部分发货") && !dataArr[ch_index].Contains("PARTIAL DELIVERY"))
                                chstr += dataArr[ch_index];
                        }
                        string[] chAddr = chstr.Split('，');
                        if (chAddr.Length >= 6)
                        {
                            data.Add("RCPCMP", chAddr[0]);
                            data.Add("RCPADDR", chAddr[1]);
                            data.Add("RCPZIP", chAddr[2]);
                            data.Add("RCPNAM", chAddr[3].Trim());
                            data.Add("RCPTEL", chAddr[4]);
                            string mobile = GetNumberOfStr(chAddr[5]);
                            data.Add("RCPMBL", mobile);
                            data.Add("CRTDAT", DateTime.Now.ToString());
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
                            data.Add("RCPCMP", "");
                            data.Add("RCPADDR", remark);
                            data.Add("RCPZIP", "");
                            data.Add("RCPNAM", "");
                            data.Add("RCPTEL", "");
                            data.Add("RCPMBL", "");
                            data.Add("CRTDAT", DateTime.Now.ToString());
                        }
                        #endregion
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
                #region MyRegion
                int endIndex = 0;
                //集合中相同名称的元素个数
                int Count = dataArr.Where(p => p.Contains(PDF_Line)).Count();
                if (Count < 3)
                {
                    //明细结束索引
                    endIndex = dataArr.IndexOf(GetValue("Sold_to_address"));
                    if (endIndex < 0)
                    {
                        //只有是中间页时，才会需要当前索引
                        if (currPage > 0)
                            //明细结束索引
                            endIndex = dataArr.IndexOf(GetValue("EndIndex"));
                    }
                }
                else
                {
                    endIndex = dataArr.LastIndexOf(PDF_Line);
                    if (Count == 4)
                        endIndex--;
                }
                #endregion
                //明细的行数
                int contentRow = (endIndex - begIndex) / 4;
                if (contentRow > 0)
                    GetPDFDetailList(list, dataArr, begIndex, endIndex, contentRow);
            }
            #endregion
            #region 英文版PDF获取明细获取方式
            int check_EN = dataArr.IndexOf("Description ##");
            if (check_EN > 0)
            {
                //明细开始索引
                int begIndex = check_EN + 2;
                //明细结束索引
                int check_index = dataArr.IndexOf(GetValue("Shipment_details2"));
                #region 数据分页，索引获取
                int endIndex = 0;
                if (check_index > 0)
                {
                    endIndex = dataArr.LastIndexOf(PDF_Line);
                }
                else
                {
                    endIndex = dataArr.IndexOf(GetValue("Sold_to_address"));
                }
                #endregion
                //明细的行数
                int contentRow = (endIndex - begIndex) / 3;
                if (contentRow > 0)
                {
                    #region 获取明细
                    //明细当前最后行索引
                    int LastIndex = begIndex;
                    for (int i = 0; i < contentRow; i++)
                    {
                        //最后行索引不能大于结束索引
                        if (LastIndex < endIndex)
                        {
                            LastIndex += 3;
                            int index = (begIndex + (i * 3));
                            Dictionary<string, string> detail = new Dictionary<string, string>();
                            while (dataArr[index].Split(' ').Length < 10
                            || dataArr[index].Split(' ')[0].Length != 6)
                            {
                                index++;
                            }
                            string firstRow = RemoveStrSpace(dataArr[index]);
                            string[] firstArr = firstRow.Replace("\\s+", " ").Split(' ');
                            string towRow = dataArr[index + 1],
                             threeRow = dataArr[index + 2];
                            #region 多行物料描述操作
                            while ((dataArr[index + 3].Split(' ')[0].Length != 6
                                || !CheckStrIsNumber(dataArr[index + 4].Split(' ')[0])
                                || dataArr[index + 3].Split(' ').Length < 10)
                                && (index + 3) < endIndex)
                            {
                                threeRow = threeRow + " " + dataArr[index + 3];
                                index++;
                                LastIndex++;
                            }
                            #endregion
                            detail.Add("ProjectRow", firstArr[0]);
                            detail.Add("MTR", firstArr[1]);
                            string num = GetNumberOfStr(firstArr[2]);
                            string unit = firstArr[2].Replace(num, "");
                            detail.Add("MTRQTY", num);
                            detail.Add("MTRSN", "");
                            detail.Add("MTRUNT", unit);
                            detail.Add("MaterialDesc", threeRow);
                            detail.Add("LOC", towRow);
                            list.Add(detail);
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        private void GetPDFDetailList(
            List<Dictionary<string, string>> list,
            List<string> dataArr,
            int begIndex,
            int endIndex,
            int contentRow)
        {
            #region 获取明细
            //明细当前最后行索引
            int LastIndex = begIndex;
            for (int i = 0; i < contentRow; i++)
            {
                //最后行索引不能大于结束索引
                if (LastIndex < endIndex)
                {
                    LastIndex += 4;
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
                    string towRow = dataArr[index + 1],
                           threeRow = dataArr[index + 2],
                           fourRow = dataArr[index + 3];
                    #region 多行物料描述操作
                    while ((dataArr[index + 4].Split(' ')[0].Length != 6
                        || !CheckStrIsNumber(dataArr[index + 4].Split(' ')[0])
                        || dataArr[index + 4].Split(' ').Length < 10
                        || dataArr[index + 4].Split(' ').Length > 15)
                        && (index + 4) < endIndex)
                    {
                        fourRow = fourRow + " " + dataArr[index + 4];
                        index++;
                        LastIndex++;
                    }
                    #endregion
                    string ProjectRow = firstArr[0];
                    //detail.Add("ProjectRow", ProjectRow);
                    string strNum = firstArr[2];
                    if (firstArr.Length == 3)
                    {
                        detail.Add("MTR", firstArr[1].Trim());
                    }
                    if (firstArr.Length > 3)
                    {
                        strNum = firstArr[firstArr.Length - 1];
                        string code = string.Empty;
                        for (int j = 0; j < firstArr.Length; j++)
                        {
                            if (firstArr[j] != ProjectRow && firstArr[j] != strNum)
                                code += " " + firstArr[j];
                        }
                        detail.Add("MTR", code.Trim());
                    }

                    string num1 = GetNumberOfStr(strNum);
                    string unit = strNum.Replace(num1, "");
                    string[] towarr = towRow.Split('/');
                    detail.Add("MTRQTY", num1);
                    //detail.Add("Number2", GetNumberOfStr(towarr[0]));
                    //detail.Add("Number3", towarr[1]);
                    detail.Add("MTRUNT", unit);
                    detail.Add("MTRSN", "");
                    //detail.Add("MaterialDesc", fourRow);
                    detail.Add("LOC", threeRow);
                    list.Add(detail);
                }
            }
            #endregion
        }

        private void PDF_Form_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] files = { "F:\\zipFile" };
            CreateZip(files, @"C:\Users\Administrator\Desktop\pdf\1231231.zip");
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="strFile">文件夹数组</param>
        /// <param name="strZip">压缩文件输出目录</param>
        public static void CreateZip(string[] strFile, string strZip)
        {
            ZipOutputStream outstream = new ZipOutputStream(File.Create(strZip));
            outstream.SetLevel(9);
            for (int i = 0, j = strFile.Length; i < j; i++)
            {
                string item = strFile[i];
                if (!Directory.Exists(item))
                {
                    continue;
                }
                CreateZipFiles(item, outstream, "*.pdf", item);
            }
            outstream.Finish();
            outstream.Close();
        }
        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream, string searchPattern, string staticFile)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath, searchPattern);
            foreach (string file in filesArray)
            {
                //如果当前是文件夹，递归
                if (Directory.Exists(file))
                {
                    CreateZipFiles(file, zipStream, searchPattern, staticFile);
                }
                else
                {
                    //如果是文件，开始压缩
                    FileStream fileStream = File.OpenRead(file);

                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    string tempFile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);

                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dir = System.IO.Path.GetFullPath("../..") + "\\Temp\\";
            #region 测试反射
            object[] parameters = new object[1];
            parameters[0] = 28;
            Type type = Type.GetType("TestReflex.RefText, TestReflex, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Fbase instance = (Fbase)Activator.CreateInstance(type, parameters);
            string name = instance.name;
            //设置Reads方法中的参数类型，Type[]类型；如有多个参数可以追加多个   
            Type[] params_type = new Type[2];
            params_type[0] = typeof(DataTable);
            params_type[1] = typeof(Dictionary<string, string>);
            //设置Reads方法中的参数值；如有多个参数可以追加多个 
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Item", typeof(string));
            DataRow dr1 = dt1.NewRow();
            dr1["Item"] = "123";
            dt1.Rows.Add(dr1);
            
            Dictionary<string, string> dic1 = new Dictionary<string, string>();
            dic1.Add("key", "2");
            Object[] params_obj = new Object[2];
            params_obj[0] = dt1;
            params_obj[1] = dic1;
            //执行Reads方法
            object value = type.GetMethod("Reads", params_type).Invoke(instance, params_obj);
            #endregion

            string WantedPath = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf(@"\"));
            string WantedPath2 = WantedPath.Substring(0, WantedPath.LastIndexOf(@"\"));
            string path1 = WantedPath2 + "\\Temp\\BUNN\\BUNN模板.doc";
            string path2 = WantedPath2 + "\\word\\BUNN\\neword.doc";
            string path3 = WantedPath2 + "\\pdf\\BUNN\\newpdf.pdf";
            BUNNUtility wordu = new BUNNUtility(path1, path2);
            #region  生成word
            DataTable dt = new DataTable();
            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("PartNo", typeof(string));
            dt.Columns.Add("ChinaName", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("Qty", typeof(string));
            dt.Columns.Add("NetWeight", typeof(string));
            dt.Columns.Add("GrossWeight", typeof(string));
            dt.Columns.Add("UnitPrice", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            dt.Columns.Add("Currency", typeof(string));
            dt.Columns.Add("Origin", typeof(string));
            dt.Columns.Add("HsCode", typeof(string));

            DataRow dr = dt.NewRow();
            //dr["Item"] = "Item";
            //dr["PartNo"] = "PartNo";
            //dr["ChinaName"] = "中文品名";
            //dr["Description"] = "Description";
            //dr["Model"] = "Model";
            //dr["Qty"] = "Qty";
            //dr["NetWeight"] = "Net Weight";
            //dr["GrossWeight"] = "Gross Weight";
            //dr["UnitPrice"] = "Unit Price";
            //dr["Amount"] = "Amount";
            //dr["Currency"] = "Currency";
            //dr["Origin"] = "Origin";
            //dr["HsCode"] = "HsCode";
            //dt.Rows.Add(dr);
            //第一行
            dr = dt.NewRow();
            dr["Item"] = "1";
            dr["PartNo"] = "42750.0051";
            dr["ChinaName"] = "咖啡桶";
            dr["Description"] = "TF SERVER,DSG2 1.5G BLK NOBAS";
            dr["Model"] = "TF SERVER";
            dr["Qty"] = "20";
            dr["NetWeight"] = "114.300";
            dr["GrossWeight"] = "122.00";
            dr["UnitPrice"] = "192.13";
            dr["Amount"] = "3842.60";
            dr["Currency"] = "USD";
            dr["Origin"] = "China";
            dr["HsCode"] = "9617009000";
            dt.Rows.Add(dr);
            //第二行
            dr = dt.NewRow();
            dr["Item"] = "2";
            dr["PartNo"] = "32125.0000";
            dr["ChinaName"] = "咖啡桶";
            dr["Description"] = "TF SERVER,DSG2 1.5G BLK NOBAS";
            dr["Model"] = "AIRPOT";
            dr["Qty"] = "27";
            dr["NetWeight"] = "59.400";
            dr["GrossWeight"] = "64.00";
            dr["UnitPrice"] = "29.75";
            dr["Amount"] = "803.25";
            dr["Currency"] = "USD";
            dr["Origin"] = "China";
            dr["HsCode"] = "9617009000";
            dt.Rows.Add(dr);
            //第3行
            dr = dt.NewRow();
            dr["Item"] = "3";
            dr["PartNo"] = "32125.0000";
            dr["ChinaName"] = "咖啡桶";
            dr["Description"] = "TF SERVER,DSG2 1.5G BLK NOBAS";
            dr["Model"] = "AIRPOT";
            dr["Qty"] = "3";
            dr["NetWeight"] = "6.300";
            dr["GrossWeight"] = "7.00";
            dr["UnitPrice"] = "29.30";
            dr["Amount"] = "87.90";
            dr["Currency"] = "USD";
            dr["Origin"] = "China";
            dr["HsCode"] = "9617009000";
            dt.Rows.Add(dr);
            //第4行
            dr = dt.NewRow();
            dr["Item"] = "4";
            dr["PartNo"] = "32125.0000";
            dr["ChinaName"] = "咖啡桶";
            dr["Description"] = "TF SERVER,DSG2 1.5G BLK NOBAS";
            dr["Model"] = "AIRPOT";
            dr["Qty"] = "1";
            dr["NetWeight"] = "2.300";
            dr["GrossWeight"] = "3.00";
            dr["UnitPrice"] = "29.09";
            dr["Amount"] = "29.09";
            dr["Currency"] = "USD";
            dr["Origin"] = "China";
            dr["HsCode"] = "9617009000";
            dt.Rows.Add(dr);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("BuyerName", "Bunn（Shanghai）Trading CO., LTD");
            dic.Add("CID", "BUNN2020-25");
            dic.Add("BuyerAddress", "A208, No. 1618 Yishan Road, Minhang District, Shanghai, China 201103");
            dic.Add("Date", "2020-2-28");
            dic.Add("SellerName", "BUNN-O-MATIC CORPORATION");
            dic.Add("SellerAddress", "1400 Stevenson Drive, Springfield, illinois, USA, 62703");
            dic.Add("Origin", "China");

            bool ck = wordu.GenerateWord(dt, dic);
            if (ck)
            {
                MessageBox.Show("生成成功。");
            }
            #endregion
            #region word转换PDF
            bool sn = wordu.WordToPdf(path2, path3);
            if (sn)
            {
                MessageBox.Show("生成成功。");
            }
            #endregion
        }
    }

    public class MyClass
    {

        public void m1()
        {
            Console.WriteLine("Called method 1.");
        }

        public static int m2(int x)
        {
            return x * x;
        }

        public void m3(int x, double y)
        {
            Console.WriteLine("Called method 3, paramaters: x = {0}, y = {1:E}.", x, y);
        }


        private static string m5(double x) //私有静态方法，不能直接调用，但可以绑定到委托  
        {
            return Math.Sqrt(x).ToString();
        }
    }
}
