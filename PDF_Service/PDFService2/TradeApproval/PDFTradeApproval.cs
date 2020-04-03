using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Common
{
    /// <summary>
    /// 一般贸易审批表
    /// </summary>
    public class PDFTradeApproval : PDFBase
    {
        TradeApprovalDataModel _model;
        const int ROW = 20;
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

        public PDFTradeApproval(string fileName, TradeApprovalDataModel model)
        {
            _model = model;
            try
            {
                _doc = new Document(PageSize.A4, 20, 20, 0, 0);//设置纸张大小为A4，页边距为0 
                _writer = PdfWriter.GetInstance(_doc, new FileStream(fileName, FileMode.Create));
                _doc.Open();

                _writer.PageEvent = new TradeApprovalPdfPageEventHelper();//页眉页脚 水印

                //pdf内容
                if (_model.List != null && _model.List.Count > 0)
                {
                    for (int j = 0; j < _model.List.Count; j++)
                    {
                        _model.List[j].rowindex = j + 1;
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

        public void NormalPage(List<TradeApprovalModel> list)//不带合计
        {
            Navigation();

            Header();

            Body(list);

            Footer();
        }

        public void LastPage(List<TradeApprovalModel> list)//带合计
        {
            Navigation();

            Header();

            BodyLast(list);

            Footer();
        }

        private void Navigation()
        {
            string title = "保税仓库出库(口)审批表";
            Chunk chunk = new Chunk(title, JointacFont.FontCN(16, Font.BOLD));
            Phrase p = new Phrase();
            p.Add(chunk);
            PdfContentByte pcb = _writer.DirectContent;
            ColumnText.ShowTextAligned(pcb, Element.ALIGN_CENTER, p, _doc.PageSize.Width / 2, _doc.Top - 30, 0);
        }

        private void Header()
        {
            var header = _model.Header;
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 12, 48, 10, 30 });
            PdfPCell cell;
            cell = CreatePdfPCellNoBorder(new Phrase("", JointacFont.FontCN()), 1, 7);
            cell.FixedHeight = 55;
            table.AddCell(cell);
            //Row 1
            cell = CreatePdfPCellNoBorder(new Phrase("经营单位:", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.wTradeName, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("贸易方式:", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.TradeName, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT);
            table.AddCell(cell);
            //Row 2
            cell = CreatePdfPCell(new Phrase("审批编号", JointacFont.FontCN()), 2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(header.CID, JointacFont.FontCN()), 2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase("报关单号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellTopLeftRight(new Phrase(header.CustomsNo, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 3
            cell = CreatePdfPCell(new Phrase("出库日期", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellTopLeftRight(new Phrase(GetDate(header.OutDateFrom.ToString(), header.OutDateTo.ToString()), JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 4
            cell = CreatePdfPCell(new Phrase("报批日期", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(StringHelper.GetDateCHString(header.ReportDate.ToString()), JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase("审批日期", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellTopLeftRight(new Phrase(StringHelper.GetDateCHString(header.ExportDate.ToString()), JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 5
            cell = CreatePdfPCell(new Phrase("电子帐册", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(header.H2000BookM, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(" "), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCellTopLeftRight(new Phrase(" "), 1, 1);
            table.AddCell(cell);

            _doc.Add(table);
        }
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="begDate">开始</param>
        /// <param name="endDate">结束</param>
        /// <returns></returns>
        public string GetDate(string begDate, string endDate)
        {
            string begtime = StringHelper.GetDateCHString2(begDate);
            string endtime = StringHelper.GetDateCHString2(begDate);
            if (!string.IsNullOrWhiteSpace(begDate) && !string.IsNullOrWhiteSpace(endDate))
                return begtime + "至" + endtime;
            else if (string.IsNullOrWhiteSpace(begDate) && string.IsNullOrWhiteSpace(endDate))
                return "";
            else
            {
                if (string.IsNullOrWhiteSpace(begDate))
                    return endtime;
                else
                    return begtime;
            }
        }
        private void Body(List<TradeApprovalModel> list)
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = _doc.PageSize.Width - 20;

            table.LockedWidth = true;

            table.SetWidths(new float[] { 5, 7, 26, 10, 7, 5, 10, 10, 10, 10 });
            table.HeaderRows = 1;
            PdfPCell cell;

            #region Head
            cell = CreatePdfPCell(new Phrase("序号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("项号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("商品名称", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("备件号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("数量/单位", JointacFont.FontCN()), 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("进库单价/币制/CIF", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("出库单价/币制/CIF", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("总价/币制", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTopLeftRight(new Phrase("海关批注", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            #endregion

            for (int i = 0; i < list.Count; i++)
            {
                cell = CreatePdfPCell(new Phrase(list[i].rowindex.ToString(), JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].H2000index.ToString(), JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].ProductName, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                cell.FixedHeight = LINE_HEIGHT;
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].ProductCode, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].totalQty.ToString("0.000"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);
                cell = CreatePdfPCell(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].UnitPriceIn.ToString("0.00") + list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].UnitPrice.ToString("0.00") + list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCell(new Phrase(list[i].Amount.ToString("0.00") + list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellTopLeftRight(new Phrase(""), 1, 1);
                table.AddCell(cell);
            }

            _doc.Add(table);
        }

        private void BodyLast(List<TradeApprovalModel> list)
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = _doc.PageSize.Width - 20;

            table.LockedWidth = true;

            table.SetWidths(new float[] { 5, 7, 26, 10, 7, 5, 10, 10, 10, 10 });
            table.HeaderRows = 1;
            PdfPCell cell;

            #region Head
            cell = CreatePdfPCell(new Phrase("序号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("项号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("商品名称", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("备件号", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("数量/单位", JointacFont.FontCN()), 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("进库单价/币制/CIF", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("出库单价/币制/CIF", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("总价/币制", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellTopLeftRight(new Phrase("海关批注", JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            #endregion


            for (int i = 0; i < ROW - 1; i++)
            {
                if (i < list.Count)
                {
                    cell = CreatePdfPCell(new Phrase(list[i].rowindex.ToString(), JointacFont.FontCN()), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].H2000index.ToString(), JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].ProductName, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    cell.FixedHeight = LINE_HEIGHT;
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].ProductCode, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].totalQty.ToString("0.000"), JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);
                    cell = CreatePdfPCell(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].UnitPriceIn.ToString("0.00") + list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].UnitPrice.ToString("0.00") + list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(list[i].Amount.ToString("0.00") + list[i].CurrencyEN, JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellTopLeftRight(new Phrase(" "), 1, 1);
                    table.AddCell(cell);
                }
                else
                {
                    cell = CreatePdfPCell(new Phrase(" "));
                    cell.FixedHeight = LINE_HEIGHT;
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCell(new Phrase(" "));
                    table.AddCell(cell);

                    cell = CreatePdfPCellTopLeftRight(new Phrase(" "));
                    table.AddCell(cell);
                }
            }

            cell = CreatePdfPCell(new Phrase("合计", JointacFont.FontCN(9, Font.BOLD)), 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(" "), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(_model.Sum.ToString("0.000"), JointacFont.FontCN(9, Font.BOLD)), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(LegalUnitEN, JointacFont.FontCN(9, Font.BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(" "), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCell(new Phrase(_model.SumPrice.ToString("0.00") + CurrencyEN, JointacFont.FontCN(9, Font.BOLD)), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellTopLeftRight(new Phrase(" "), 1, 1);
            table.AddCell(cell);
            _doc.Add(table);
        }

        private void Footer()
        {
            var footer = _model.Header;
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SpacingBefore = 0.4f;//表格间距
            PdfPCell cell;

            //Row 1
            cell = CreatePdfPCellTopLeftRight(new Phrase("备注:", JointacFont.FontCN()), 1, 10, Element.ALIGN_LEFT);
            table.AddCell(cell);
            cell = CreatePdfPCellLeft(new Phrase(""), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCellRight(new Phrase(footer.remark, JointacFont.FontCN()), 1, 9);
            cell.MinimumHeight = 45;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("审批意见:", JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            cell.MinimumHeight = 40;
            table.AddCell(cell);
            cell = CreatePdfPCellTopRight(new Phrase(""), 1, 10);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(""), 1, 5);
            table.AddCell(cell);
            cell = CreatePdfPCellRightBottom(new Phrase("海关经办员盖章:", JointacFont.FontCN()), 1, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = CreatePdfPCellNoBorder(new Phrase("单位盖章:", JointacFont.FontCN()), 1, 2, Element.ALIGN_RIGHT);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(""), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("负责人:", JointacFont.FontCN()), 1, 2, Element.ALIGN_RIGHT);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(footer.H2KOperator, JointacFont.FontCN()), 1, 2, Element.ALIGN_LEFT);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(""), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("经办人:", JointacFont.FontCN()), 1, 1, Element.ALIGN_RIGHT);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(footer.UserName, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT);
            table.AddCell(cell);

            _doc.Add(table);
        }

    }
}
