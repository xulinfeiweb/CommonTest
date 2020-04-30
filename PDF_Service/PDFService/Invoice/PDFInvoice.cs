using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using static iTextSharp.text.Font;
using System.IO;

namespace Common
{
    /// <summary>
    /// 发货凭证
    /// </summary>
    public class PDFInvoice : PDFBase
    {
        InvoiceDataModel _model;
        private int ROW = 30;
        /// <summary>
        /// 表格行高
        /// </summary>
        private float LINE_HEIGHT = 20;
        /// <summary>
        /// 币制
        /// </summary>
        private string CurrencyEN = "";
        /// <summary>
        /// 宽
        /// </summary>
        private string WeightUnitEN = "";
        /// <summary>
        ///厂商的图片路径
        /// </summary>
        private string _topPath = "";
        /// <summary>
        /// 印章的图片路径
        /// </summary>
        private string _ChapterPaths = "";
        private string _title = "SOLD TO:";
        /// <summary>
        /// 发票
        /// </summary>
        /// <param name="fileName">PDF路径</param>
        /// <param name="path">图片路径</param>
        /// <param name="model">实体</param>

        public PDFInvoice(string fileName, string path, InvoiceDataModel model) : base(2f)
        {
            _model = model;
            _topPath = path;
            try
            {
                #region 配置
                Rectangle rl = PageSize.A4;
                if (_model.Flag == 1)
                {
                    ROW = 16;
                    rl = PageSize.A4.Rotate();
                }
                if (_model.Flag == 2)
                {
                    ROW = 17;
                    rl = PageSize.A4.Rotate();
                }
                _doc = new Document(rl, 20, 20, 0, 0);//设置纸张大小为A4，页边距为0 
                _writer = PdfWriter.GetInstance(_doc, new FileStream(fileName, FileMode.Create));
                _doc.Open();
                if (_model.Flag != 2)
                    _writer.PageEvent = new InvoicePdfPageEventHelper("EN");//页眉页脚 水印
                #endregion

                //pdf内容
                if (_model.List != null && _model.List.Count > 0)
                {
                    if (_model.Flag != 1)
                    {
                        for (int j = 0; j < _model.List.Count; j++)
                        {
                            _model.List[j].rowindex = (j + 1).ToString();
                        }
                        _title = "Buyer:";
                    }
                    CurrencyEN = _model.List[0].CurrencyEN;
                    WeightUnitEN = _model.List[0].WeightUnitEN;
                    _ChapterPaths = _model.ChapterPaths;
                    int page = _model.List.Count / ROW + 1;
                    if (page == 1)
                    {
                        LastPage(_model.List);//只有一页
                    }
                    else if (page == 2)
                    {
                        var list1 = _model.List.Take(ROW).ToList();
                        var list2 = _model.List.Skip(ROW).Take(ROW).ToList();
                        NormalPage(list1);
                        _doc.NewPage();
                        LastPage(list2);
                    }
                    else
                    {
                        var list1 = _model.List.Take(ROW).ToList();
                        NormalPage(list1);
                        for (int i = 1; i < page; i++)
                        {
                            var list2 = _model.List.Skip(ROW * i).Take(ROW).ToList();
                            _doc.NewPage();
                            if (i == page - 1)
                            {
                                LastPage(list2);//最后一页
                            }
                            else
                            {
                                NormalPage(list2);
                            }
                        }
                    }
                }
                _doc.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NormalPage(List<InvoiceModel> list)//不带合计
        {
            Navigation();

            Header();
            //是否分拨结关 1：是，0：不是
            if (_model.Flag == 1)
            {
                BodyLast2(list);
            }
            else
            {
                BodyLast(list);
            }
            Footer();
        }

        public void LastPage(List<InvoiceModel> list)//带合计
        {
            Navigation();

            Header();
            //是否分拨结关 1：是，0：不是
            if (_model.Flag == 1)
            {
                BodyLast2(list, true);
            }
            else
            {
                BodyLast(list, true);
            }
            Footer();
        }

        private void Navigation()
        {
            if (!string.IsNullOrWhiteSpace(_topPath))
                AddImage(_topPath, 150, 25, _doc.Left, _doc.Top - 38);
        }
        private void Header()
        {
            var header = _model.Header;
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            int fontSize = 9;
            if (_model.Flag != 1)
                fontSize = 8;
            PdfPCell cell;
            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            cell.FixedHeight = 38;
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(!string.IsNullOrWhiteSpace(header.h0) ? header.h0 : " ", JointacFont.FontCN(7)), 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(!string.IsNullOrWhiteSpace(header.h1) ? header.h1 : " ", JointacFont.FontCN(7)), 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(!string.IsNullOrWhiteSpace(header.h2) ? header.h2 : " ", JointacFont.FontCN(7)), 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 1
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead1, JointacFont.FontCN(_model.Flag != 1 ? 13 : 16, Font.BOLD)), 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            cell.FixedHeight = 5;
            table.AddCell(cell);
            //Row 2
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead2, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead3 + "        " + header.invoicehead4, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 3
            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            cell.FixedHeight = 5;
            table.AddCell(cell);
            //Row 4
            cell = CreatePdfPCellNoBorder(new Phrase(_title, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("SHIP TO:", JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 5
            cell = CreatePdfPCellNoBorder(new Phrase(header.b0, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t0, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 6
            cell = CreatePdfPCellNoBorder(new Phrase(header.b1, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t1, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 7
            cell = CreatePdfPCellNoBorder(new Phrase(header.b2, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t2, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 8
            cell = CreatePdfPCellNoBorder(new Phrase(header.b3, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t3, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 9
            cell = CreatePdfPCellNoBorder(new Phrase(header.b4, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t4, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 10
            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            cell.FixedHeight = 5;
            table.AddCell(cell);
            _doc.Add(table);
        }

        private void BodyLast(List<InvoiceModel> list, bool isLastPage = false)
        {
            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4, 10, 35, 6, 9, 9, 9, 6, 12 });
            table.HeaderRows = 1;
            PdfPCell cell;
            #region  
            cell = CreatePdfPCellBottom(new Phrase("Item", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("PartNo", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Description", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Qty", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Net Weight", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Unit Price", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Amount", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Currency", JointacFont.FontCN(7)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Origin", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            #endregion

            for (int i = 0; i < list.Count; i++)
            {
                Font font = JointacFont.FontCN();
                if (list[i].IsFather == 1)
                    font = JointacFont.FontCN(9, Font.BOLD);
                cell = CreatePdfPCellNoBorder(new Phrase(list[i].rowindex.ToString(), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.FixedHeight = LINE_HEIGHT;
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].ProductCode, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].ProductDescrEN, JointacFont.FontCN(7)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].ClearQty.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].NetWeight.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].UnitPrice.ToString("0.00"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase((list[i].ClearQty * list[i].UnitPrice).ToString("0.00"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].CurrencyEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].MadeInEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);
                if (list[i].IsFather == 1)
                {
                    //cell = CreatePdfPCellNoBorder(new Phrase("----------------------------------------------------------------------------------------------------------------------------------------------"), 1, 12);
                    cell = CreatePdfPCellNoBorder(new Phrase("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------"), 1, 12);
                    cell.PaddingTop = -5;
                    cell.PaddingBottom = -5;
                    table.AddCell(cell);
                }
                if (list[i].IsFather == -1)
                {
                    cell = CreatePdfPCellTop(new Phrase(" "), 1, 12);
                    cell.FixedHeight = 3;
                    table.AddCell(cell);
                }
            }
            if (_model.Chapter == "true")
            {
                float yheight = 0;
                if (list.Count == ROW)
                    yheight = 0;
                if (list.Count < ROW)
                    yheight = (ROW - list.Count) * LINE_HEIGHT;
                addChapter_Invoice(yheight);
            }
            //统计
            if (isLastPage)
            {
                Amount(table);
            }
            else
            {
                cell = CreatePdfPCellBottom(new Phrase(" "), 1, 9);
                cell.FixedHeight = 1;
                table.AddCell(cell);
            }
            _doc.Add(table);
        }

        private void BodyLast2(List<InvoiceModel> list, bool isLastPage = false)
        {
            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4, 10, 8, 7, 26, 5, 4, 7, 7, 6, 9, 7 });
            table.HeaderRows = 1;
            PdfPCell cell;
            #region  
            cell = CreatePdfPCellBottom(new Phrase("Item", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("PO#-SO#", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("DN#", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("PartNo", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Description", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Qty", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Unit", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Unit Price", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Amount", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Currency", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("Origin", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellBottom(new Phrase("HsCode", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            #endregion

            for (int i = 0; i < list.Count; i++)
            {
                Font font = JointacFont.FontCN();
                if (list[i].IsFather == 1)
                    font = JointacFont.FontCN(9, BOLD);
                cell = CreatePdfPCellNoBorder(new Phrase(list[i].rowindex.ToString(), font), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.FixedHeight = LINE_HEIGHT;
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].PO, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].DN, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].ProductCode, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].ProductDescrEN, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].ClearQty.ToString("0.000"), font), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].QuantityUnitEN, font), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].UnitPrice.ToString("0.00"), font), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase((list[i].ClearQty * list[i].UnitPrice).ToString("0.00"), font), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].CurrencyEN, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].MadeInEN, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellNoBorder(new Phrase(list[i].HSCode, font), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);
                if (list[i].IsFather == 1)
                {
                    cell = CreatePdfPCellNoBorder(new Phrase("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------"), 1, 12);
                    cell.PaddingTop = -5;
                    cell.PaddingBottom = -5;
                    table.AddCell(cell);
                }
                if (list[i].IsFather == -1)
                {
                    cell = CreatePdfPCellTop(new Phrase(" "), 1, 12);
                    cell.FixedHeight = 3;
                    table.AddCell(cell);
                }
            }
            if (_model.Chapter == "true")
            {
                float yheight = 0;
                if (list.Count == ROW)
                    yheight = 0;
                if (list.Count < ROW)
                    yheight = (ROW - list.Count) * LINE_HEIGHT;
                addChapter_Invoice(yheight);
            }
            //统计
            if (isLastPage)
            {
                Amount2(table);
            }
            else
            {
                cell = CreatePdfPCellBottom(new Phrase(" "), 1, 12);
                cell.FixedHeight = 1;
                table.AddCell(cell);
            }
            _doc.Add(table);
        }

        /// <summary>
        /// 添加印章
        /// </summary>
        public void addChapter_Invoice(float yy)
        {
            string[] files = Directory.GetFiles(_ChapterPaths, "*.jpg");
            Random random = new Random();
            int index = random.Next(files.Length - 1);
            string path = files[index];
            Image img = Image.GetInstance(path);
            img.ScalePercent(25);
            int y = index + 1;
            if (_model.Flag == 1)
                y += 50;
            else if (_model.Flag == 2)
                y += 32;
            else
                y += 20;
            img.SetAbsolutePosition(_doc.Right - 180, y + yy);
            _writer.DirectContent.AddImage(img);
        }
        /// <summary>
        /// 合计
        /// </summary>
        public void Amount(PdfPTable table)
        {
            PdfPCell cell;
            cell = CreatePdfPCellTop(new Phrase(" "), 1, 2);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase("Total:", JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.FixedHeight = FixedHeight;
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(_model.Sum.ToString("0.000"), JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(_model.SumWeight.ToString("0.000"), JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(_model.SumAmount.ToString("0.00"), JointacFont.FontCN(8, BOLD)), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(CurrencyEN, JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(" "), 1, 1);
            table.AddCell(cell);
        }
        /// <summary>
        /// 合计
        /// </summary>
        public void Amount2(PdfPTable table)
        {
            PdfPCell cell;
            cell = CreatePdfPCellTop(new Phrase(" "), 1, 3);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase("Total:", JointacFont.FontCN(10, BOLD)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(_model.Sum2.ToString("0.000"), JointacFont.FontCN(10, BOLD)), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(" "), 1, 2);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(_model.SumAmount2.ToString("0.00"), JointacFont.FontCN(10, BOLD)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(CurrencyEN, JointacFont.FontCN(10, BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTop(new Phrase(" "), 1, 2);
            table.AddCell(cell);
        }
        private void Footer()
        {
            var footer = _model.Header;
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            PdfPCell cell;

            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 1);
            cell.FixedHeight = 10;
            table.AddCell(cell);
            decimal grossWt = 0;
            if (string.IsNullOrWhiteSpace(footer.GrossWt.ToString()) || footer.GrossWt == 0)
            {
                grossWt = _model.WeightWrite;
            }
            else
            {
                grossWt = footer.GrossWt;
            }
            if (string.IsNullOrWhiteSpace(_model.Box))
                _model.Box = "0";
            cell = CreatePdfPCellNoBorder(new Phrase("Box:" + Convert.ToDecimal(_model.Box).ToString("0"), JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("Weight:" + grossWt.ToString("0.000") + WeightUnitEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            _doc.Add(table);
        }

    }
}
