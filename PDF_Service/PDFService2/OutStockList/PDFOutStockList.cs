using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
    public class PDFOutStockList : PDFBase
    {
        OutStockListDataModel _model;
        private int ROW = 19;
        /// <summary>
        /// 表格行高
        /// </summary>
        const float LINE_HEIGHT = 25;
        /// <summary>
        /// 数量单位
        /// </summary>
        private string LegalUnitEN = "";
        /// <summary>
        /// 重量单位
        /// </summary>
        private string WeightUnitEN = "";
        /// <summary>
        /// 币制
        /// </summary>
        private string CurrencyEN = "";
        private int fontSize = 7;
        private string _ChapterPaths = "";

        public PDFOutStockList(string fileName, OutStockListDataModel model)
        {
            _model = model;
            try
            {
                _doc = new Document(PageSize.A4.Rotate(), 20, 20, 0, 0);//设置纸张大小为A4 横向，页边距为0 
                _writer = PdfWriter.GetInstance(_doc, new FileStream(fileName, FileMode.Create));

                _doc.Open();
                if (_model.Flag != 2)
                    _writer.PageEvent = new OutStockListPdfPageEventHelper();//页眉页脚 水印

                //pdf内容
                if (_model.List != null && _model.List.Count > 0)
                {
                    int page = _model.List.Count / ROW + 1;
                    LegalUnitEN = _model.List[0].LegalUnitEN;
                    WeightUnitEN = _model.List[0].WeightUnitEN;
                    CurrencyEN = _model.List[0].CurrencyEN;
                    _ChapterPaths = _model.ChapterPaths;
                    if (page == 1)
                    {
                        LastPage(_model.List);//只有一页
                    }
                    else if (page == 2)
                    {
                        var list1 = _model.List.Take(ROW).ToList();
                        var list2 = _model.List.Skip(ROW).Take(ROW).ToList();
                        NormalPage(list1);
                        _doc.NewPage(); ;
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

        public void NormalPage(List<OutStockListModel> list)//不带合计
        {
            Header();
            Body(list);
        }

        /// <summary>
        /// 最后一页（带合计）
        /// </summary>
        /// <param name="list"></param>
        public void LastPage(List<OutStockListModel> list)
        {
            Header();

            BodyList(list);
        }

        #region Content

        private void Header()
        {
            var head = _model.Header;
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = _doc.PageSize.Width - 80;
            table.LockedWidth = true;
            //Row 1
            PdfPCell cell = CreatePdfPCellNoBorder(new Phrase(head.CKQDTitle, JointacFont.FontCN(13, Font.BOLD)), 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = 55;
            table.AddCell(cell);
            _doc.Add(table);
        }
        private void Body(List<OutStockListModel> list)
        {
            PdfPTable table = new PdfPTable(18);
            table.TotalWidth = _doc.PageSize.Width - 10;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 7, 4, 16, 6, 4, 4, 6, 4, 4, 5, 6, 5, 3, 3, 3, 6, 5, 9 });
            PdfPCell cell;
            #region Header
            BaseColor color = new BaseColor(200, 200, 200);
            cell = CreatePdfPCell(new Phrase("编号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = 20;
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("项号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("商品名称", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("账册备件号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("总件数", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("总金额", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("实物备件号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("件数", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("净重", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("货值", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("出库时间", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("原产国", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("数量单位", JointacFont.FontCN(6)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("重量单位", JointacFont.FontCN(6)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("币制", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("运输代码", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("发票号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCellTopLeftRight(new Phrase("分运单号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);
            #endregion
            for (int i = 0; i < list.Count; i++)
            {
                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].CID, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                cell.FixedHeight = LINE_HEIGHT;
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].H2000index, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductName, JointacFont.FontCN(6)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCodeC, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].totalQty.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].TotalAmount.ToString("0.00"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCode, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalClearQty.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].NetWeight.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].TotalAmount.ToString("0.00"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].OutDate.ToString("yyyy/MM/dd"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].MadeInEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].WeightUnitEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].CurrencyEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ShipCode, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottom(new Phrase(list[i].PONumber, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);

                cell = CreatePdfPCellLeftBottomRight(new Phrase(list[i].HAWB, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                table.AddCell(cell);
            }
            if (_model.Chapter == "true")
            {
                addChapter();
            }
            _doc.Add(table);
        }
        private void BodyList(List<OutStockListModel> list)
        {
            PdfPTable table = new PdfPTable(18);
            table.TotalWidth = _doc.PageSize.Width - 10;
            table.LockedWidth = true;

            table.SetWidths(new float[] { 7, 4, 16, 6, 4, 4, 6, 4, 4, 5, 6, 5, 3, 3, 3, 6, 5, 9 });
            PdfPCell cell;
            #region Header
            BaseColor color = new BaseColor(200, 200, 200);
            cell = CreatePdfPCell(new Phrase("编号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.FixedHeight = 20;
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("项号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("商品名称", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("账册备件号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("总件数", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("总金额", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("实物备件号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("件数", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("净重", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("货值", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("出库时间", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("原产国", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("数量单位", JointacFont.FontCN(6)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("重量单位", JointacFont.FontCN(6)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("币制", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("运输代码", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCell(new Phrase("发票号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);

            cell = CreatePdfPCellTopLeftRight(new Phrase("分运单号", JointacFont.FontCN(7)), color, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.BackgroundColor = color;
            table.AddCell(cell);
            #endregion
            for (int i = 0; i < ROW - 1; i++)
            {
                if (i < list.Count)
                {
                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].CID, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    cell.FixedHeight = LINE_HEIGHT;
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].H2000index, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductName, JointacFont.FontCN(6)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCodeC, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].totalQty.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].TotalAmount.ToString("0.00"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ProductCode, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalClearQty.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].NetWeight.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].TotalAmount.ToString("0.00"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].OutDate.ToString("yyyy/MM/dd"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].MadeInEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].LegalUnitEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].WeightUnitEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].CurrencyEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].ShipCode, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottom(new Phrase(list[i].PONumber, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    table.AddCell(cell);

                    cell = CreatePdfPCellLeftBottomRight(new Phrase(list[i].HAWB, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
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
            if (_model.Chapter == "true")
            {
                addChapter();
            }
            //统计
            Amount(table);
            _doc.Add(table);
        }
        /// <summary>
        /// 添加印章
        /// </summary>
        public void addChapter()
        {
            string[] files = Directory.GetFiles(_ChapterPaths, "*.jpg");
            Random random = new Random();
            int index = random.Next(files.Length - 1);
            string path = files[index];
            Image img = Image.GetInstance(path);
            img.ScalePercent(25);
            img.SetAbsolutePosition(_doc.Right - 280, 20 + index + 1);
            _writer.DirectContent.AddImage(img);
        }
        /// <summary>
        /// 合计
        /// </summary>
        public void Amount(PdfPTable table)
        {
            PdfPCell cell;
            cell = CreatePdfPCellLeftBottom(new Phrase("总计:", JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.FixedHeight = LINE_HEIGHT;
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 2);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.Sum.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.SumAmount.ToString("0.00"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.SumLegalClearQty.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.SumNetWeight.ToString("0.000"), JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(_model.SumTotalAmount.ToString("0.00"), JointacFont.FontCN(fontSize)), 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(LegalUnitEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(WeightUnitEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(CurrencyEN, JointacFont.FontCN(fontSize)), 1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottom(new Phrase(" "), 1, 1);
            table.AddCell(cell);

            cell = CreatePdfPCellLeftBottomRight(new Phrase(" "), 1, 1);
            table.AddCell(cell);
        }
        #endregion
    }
}
