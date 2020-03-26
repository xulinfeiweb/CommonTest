using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class PDFCInvoice : PDFBase
    {
        CInvoiceDataModel _model;
        private int ROW = 16;
        /// <summary>
        /// 表格行高
        /// </summary>
        const float LINE_HEIGHT = 25;
        /// <summary>
        /// 数量单位
        /// </summary>
        private string LegalUnitEN = "";
        /// <summary>
        /// 币制
        /// </summary>
        private string CurrencyEN = "";

        public PDFCInvoice(string fileName, CInvoiceDataModel model) : base(2f)
        {
            _model = model;
            try
            {
                _doc = new Document(PageSize.A4.Rotate(), 0, 0, 0, 5);//设置纸张大小为A4，页边距为0 
                _writer = PdfWriter.GetInstance(_doc, new FileStream(fileName, FileMode.Create));

                _doc.Open();

                _writer.PageEvent = new CInvoicePdfPageEventHelper();//页眉页脚 水印

                //pdf内容
                if (_model.List != null && _model.List.Count > 0)
                {
                    for (int j = 0; j < _model.List.Count; j++)
                    {
                        _model.List[j].rowindex = (j + 1).ToString();
                    }
                    LegalUnitEN = _model.List[0].LegalUnitEN;
                    CurrencyEN = _model.List[0].CurrencyEN;
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
            Header();

            Body(list);
        }

        public void LastPage(List<InvoiceModel> list)//带合计
        {
            Header();

            BodyLast(list);
        }
        private void Header()
        {
            var header = _model.Header;
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            PdfPCell cell;

            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            cell.FixedHeight = 10;
            table.AddCell(cell);
            //Row 1
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead1, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead2, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead3, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.invoicehead4, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 2
            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            table.AddCell(cell);
            //Row 3
            cell = CreatePdfPCellNoBorder(new Phrase("SHIP FROM:", JointacFont.FontCN(10, Font.BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("SHIP TO:", JointacFont.FontCN(10, Font.BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 5
            cell = CreatePdfPCellNoBorder(new Phrase(header.f0, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t0, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 6
            cell = CreatePdfPCellNoBorder(new Phrase(header.f1, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t1, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 7
            cell = CreatePdfPCellNoBorder(new Phrase(header.f2, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t2, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 8
            cell = CreatePdfPCellNoBorder(new Phrase(header.f3, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t3, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 9
            cell = CreatePdfPCellNoBorder(new Phrase(header.f4, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t4, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 10
            cell = CreatePdfPCellBottom(new Phrase(" "), 1, 2);
            cell.FixedHeight = 10;
            table.AddCell(cell);
            _doc.Add(table);
        }
        private void Body(List<InvoiceModel> list)
        {
            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4, 6, 20, 18, 5, 4, 7, 8, 6, 8, 6, 8 });
            table.HeaderRows = 1;
            PdfPCell cell;
            #region  
            BaseColor color = new BaseColor(200, 200, 200);
            cell = CreatePdfPCellLeftBottom(new Phrase("Item", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);
            cell = CreatePdfPCellLeftBottom(new Phrase("PartNo", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("CDesc", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Edesc", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Qty", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Unit", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Unit Price", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Amount Price", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Currency", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Origin", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("H2000Index", JointacFont.FontCN(8)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottomRight(new Phrase("HsCode", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            #endregion
            for (int i = 0; i < list.Count; i++)
            {
                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].rowindex, JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.FixedHeight = LINE_HEIGHT;
                table.AddCell(cell);
                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCode, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductName, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductDescrEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ClearQty.ToString("0.000"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].UnitPrice.ToString("0.00"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase((list[i].ClearQty * list[i].UnitPrice).ToString("0.00"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].MadeInEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].H2000Index, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottomRight(new Phrase(list[i].HSCode, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);
            }
            _doc.Add(table);
        }
        private void BodyLast(List<InvoiceModel> list)
        {
            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 4, 6, 20, 18, 5, 4, 7, 8, 6, 8, 6, 8 });
            table.HeaderRows = 1;
            PdfPCell cell;
            #region  
            BaseColor color = new BaseColor(200, 200, 200);
            cell = CreatePdfPCellLeftBottom(new Phrase("Item", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);
            cell = CreatePdfPCellLeftBottom(new Phrase("PartNo", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("CDesc", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Edesc", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Qty", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Unit", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Unit Price", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Amount Price", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Currency", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Origin", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("H2000Index", JointacFont.FontCN(8)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottomRight(new Phrase("HsCode", JointacFont.FontCN()), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            #endregion
            for (int i = 0; i < ROW - 1; i++)
            {
                if (i < list.Count)
                {
                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].rowindex, JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                    cell.FixedHeight = LINE_HEIGHT;
                    table.AddCell(cell);
                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCode, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductName, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductDescrEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ClearQty.ToString("0.000"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].UnitPrice.ToString("0.00"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase((list[i].ClearQty * list[i].UnitPrice).ToString("0.00"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].MadeInEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].H2000Index, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottomRight(new Phrase(list[i].HSCode, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);
                }
                else
                {
                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    cell.FixedHeight = LINE_HEIGHT;
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottomRight(new Phrase(" "));
                    table.AddCell(cell);
                }
            }
            //统计
            Amount(table);
            _doc.Add(table);
        }
        /// <summary>
        /// 合计
        /// </summary>
        public void Amount(PdfPTable table)
        {
            PdfPCell cell;
            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 2);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Total:", JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.Sum.ToString("0.000"), JointacFont.FontCN()), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellLeftBottom(new Phrase(LegalUnitEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.SumAmount.ToString("0.00"), JointacFont.FontCN()), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellLeftBottom(new Phrase(CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 2);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottomRight(new Phrase(" "), 1, 1);
            table.AddCell(cell);
        }
    }
}
