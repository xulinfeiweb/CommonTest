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
    public class BoxList : PDFBase
    {
        BoxListDataModel _model;
        private int ROW = 20;
        /// <summary>
        /// 表格行高
        /// </summary>
        private float LINE_HEIGHT = 20;

        /// <summary>
        /// 单位
        /// </summary>
        private string _Unit = "";
        /// <summary>
        /// 重量单位
        /// </summary>
        private string _WeightUnitEN = "";
        /// <summary>
        /// 装箱单
        /// </summary>
        /// <param name="fileName">PDF路径</param>
        /// <param name="model">实体</param>

        public BoxList(string fileName, BoxListDataModel model) : base(2f)
        {
            _model = model;
            try
            {
                #region 配置
                Rectangle rl = PageSize.A4;
                _doc = new Document(PageSize.A4.Rotate(), 20, 20, 0, 0);//设置纸张大小为A4，页边距为0 
                _writer = PdfWriter.GetInstance(_doc, new FileStream(fileName, FileMode.Create));
                _doc.Open();
                _writer.PageEvent = new BoxListPdfPageEventHelper();//页眉页脚 水印
                #endregion

                //pdf内容
                if (_model.List != null && _model.List.Count > 0)
                {

                    for (int j = 0; j < _model.List.Count; j++)
                    {
                        _model.List[j].rowindex = (j + 1).ToString();
                    }
                    _Unit = _model.List[0].LegalUnitEN;
                    _WeightUnitEN = _model.List[0].WeightUnitEN;
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

            Footer();
        }
        public void LastPage(List<InvoiceModel> list)//带合计
        {
            Header();

            BodyList(list);

            Footer();
        }
        private void Header()
        {
            var header = _model.Header;
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            PdfPCell cell;

            //Row 1
            cell = CreatePdfPCellNoBorder(new Phrase("PACKING LIST", JointacFont.FontCN(14)), 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = 30;
            table.AddCell(cell);
            //Row 2
            cell = CreatePdfPCellNoBorder(new Phrase("SHIP FROM:", JointacFont.FontCN(10, Font.BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("SHIP TO:", JointacFont.FontCN(10, Font.BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 3
            cell = CreatePdfPCellNoBorder(new Phrase(header.f0, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t0, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 4
            cell = CreatePdfPCellNoBorder(new Phrase(header.f1, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t1, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 5
            cell = CreatePdfPCellNoBorder(new Phrase(header.f2, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t2, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 6
            cell = CreatePdfPCellNoBorder(new Phrase(header.f3, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t3, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 7
            cell = CreatePdfPCellNoBorder(new Phrase(header.f4, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase(header.t4, JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            //Row 8
            cell = CreatePdfPCellNoBorder(new Phrase(" "), 1, 2);
            cell.FixedHeight = 10;
            table.AddCell(cell);
            _doc.Add(table);
        }

        private void Body(List<InvoiceModel> list)
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 5, 10, 22, 22, 7, 5, 7, 7, 5, 10 });
            PdfPCell cell;
            #region  
            BaseColor color = new BaseColor(200, 200, 200);
            cell = CreatePdfPCell(new Phrase("Item", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("PartNo", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("CDesc", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Edesc", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Qty", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Unit", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("UnitWt.(kg)", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("TWt.(kg)", JointacFont.FontCN(7)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Unit", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCellTopLeftRight(new Phrase("H2000Index", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);
            #endregion

            for (int i = 0; i < list.Count; i++)
            {
                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].rowindex.ToString(), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.FixedHeight = LINE_HEIGHT;
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCode, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductName, JointacFont.FontCN(7)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductDescrEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ClearQty.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].NetWeight.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].TotalNet.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].WeightUnitEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottomRight(new Phrase(list[i].H2000Index, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);
            }
            _doc.Add(table);
        }
        private void BodyList(List<InvoiceModel> list)
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = _doc.PageSize.Width - 20;
            table.LockedWidth = true;
            table.SetWidths(new float[] { 5, 10, 22, 22, 7, 5, 7, 7, 5, 10 });
            PdfPCell cell;
            #region  
            BaseColor color = new BaseColor(200, 200, 200);
            cell = CreatePdfPCell(new Phrase("Item", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("PartNo", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("CDesc", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Edesc", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Qty", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Unit", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("UnitWt.(kg)", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("TWt.(kg)", JointacFont.FontCN(7)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("Unit", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCellTopLeftRight(new Phrase("H2000Index", JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);
            #endregion

            for (int i = 0; i < ROW - 1; i++)
            {
                if (i < list.Count)
                {
                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].rowindex.ToString(), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                    cell.FixedHeight = LINE_HEIGHT;
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCode, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductName, JointacFont.FontCN(7)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductDescrEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ClearQty.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].NetWeight.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].TotalNet.ToString("0.000"), JointacFont.FontCN(8)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].WeightUnitEN, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottomRight(new Phrase(list[i].H2000Index, JointacFont.FontCN(8)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
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
            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);
            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase("Total:", JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.Sum.ToString("0.000"), JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_Unit, JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.SumTotalNet.ToString("0.000"), JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_WeightUnitEN, JointacFont.FontCN(8, BOLD)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottomRight(new Phrase(" "), 1, 1);
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
            cell.FixedHeight = 15;
            table.AddCell(cell);
            decimal grossWt = 0;
            if (string.IsNullOrWhiteSpace(footer.GrossWt.ToString()) || footer.GrossWt == 0)
            {
                grossWt = _model.WeightWrites;
            }
            else
            {
                grossWt = footer.GrossWt;
            }
            if (string.IsNullOrWhiteSpace(_model.Boxs))
                _model.Boxs = "0";
            cell = CreatePdfPCellNoBorder(new Phrase("Box:" + Convert.ToDecimal(_model.Boxs).ToString("0"), JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            cell = CreatePdfPCellNoBorder(new Phrase("Gross Weight(KG):" + grossWt.ToString("0.000"), JointacFont.FontCN()), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);
            _doc.Add(table);
        }

    }
}
