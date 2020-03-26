using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class PDFBase
    {
        protected Document _doc;
        protected PdfWriter _writer;

        /// <summary>
        /// 内边距设置 默认值 4f
        /// </summary>
        /// <param name="padding"></param>
        public PDFBase(float padding = 4f)
        {
            this.Padding = padding;
        }
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth { get; set; } = 1.0f;

        /// <summary>
        /// 边框颜色
        /// </summary>
        public BaseColor BORDER_COLOR { get; set; } = BaseColor.BLACK;

        /// <summary>
        /// padding 内边距
        /// </summary>
        public float Padding { get; set; }

        /// <summary>
        /// 表格行高
        /// </summary>
        public float FixedHeight { get; set; } = 0;


        #region Cell and Border
        public PdfPCell CreatePdfPCellNoBorder(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);
            cell.SetLeading(1, 1);
            return cell;
        }

        /// <summary>
        /// 默认  左上边框
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCell(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = 0f;

            return cell;
        }
        /// <summary>
        /// 默认  左上边框
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCell(Phrase p, BaseColor color, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);
            cell.BackgroundColor = color;

            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = 0f;

            return cell;
        }
        /// <summary>
        /// 上边框 文本内容
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTop(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = 0f;

            return cell;
        }

        /// <summary>
        /// 上右边框 文本内容
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTopRight(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = 0f;

            return cell;
        }

        /// <summary>
        /// 右
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTopLeftRight(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = 0f;

            return cell;
        }
        /// <summary>
        /// 右
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTopLeftRight(Phrase p, BaseColor color, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);
            cell.BackgroundColor = color;

            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = 0f;

            return cell;
        }
        /// <summary>
        /// 左下 底部
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTopLeftBottom(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthRight = 0f;
            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        /// <summary>
        /// 底中 文字内容部分  无左边框和右边框
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTopBottom(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthRight = 0f;
            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        /// <summary>
        /// 右下 文字内容部分  无左边框
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellTopRightBottom(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        /// <summary>
        /// 下边框
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellBottom(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        /// <summary>
        /// 右下
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colspan"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellAll(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthTop = BorderWidth;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }


        public PdfPCell CreatePdfPCellLeft(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = 0f;

            return cell;
        }
        public PdfPCell CreatePdfPCellLeftBottom(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }
        public PdfPCell CreatePdfPCellLeftBottom(Phrase p, BaseColor color, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);
            cell.BackgroundColor = color;

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = 0f;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        public PdfPCell CreatePdfPCellLeftBottomRight(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        public PdfPCell CreatePdfPCellLeftBottomRight(Phrase p, BaseColor color, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);
            cell.BackgroundColor = color;

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }

        public PdfPCell CreatePdfPCellRight(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = 0f;

            return cell;
        }
        public PdfPCell CreatePdfPCellRightBottom(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = BorderWidth;

            return cell;
        }
        public PdfPCell CreatePdfPCellLeftRight(Phrase p, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);

            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = BorderWidth;
            cell.BorderWidthRight = BorderWidth;
            cell.BorderWidthBottom = 0f;

            return cell;
        }

        public PdfPCell CreatePdfPCellBackColor(Phrase p, BaseColor backColor, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = 0, int VerticalAlignment = 4, float fixedHeight = 0)
        {
            var cell = CreateBaseCell(p, rowSpan, colspan, HorizontalAlignment, VerticalAlignment);
            cell.BackgroundColor = backColor;

            cell.BorderWidthRight = 2;
            cell.BorderColorRight = BaseColor.WHITE;

            return cell;
        }

        #endregion


        #region MyRegion

        /// <summary>
        /// 设置PDF相关信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subject"></param>
        /// <param name="keywords"></param>
        /// <param name="creator"></param>
        /// <param name="author"></param>
        protected void SetDocumentInfo(string title = "", string subject = "", string keywords = "", string creator = "", string author = "")
        {
            _doc.AddTitle(title);
            _doc.AddSubject(subject);
            _doc.AddKeywords(keywords);
            _doc.AddCreator(creator);
            _doc.AddAuthor(author);
        }



        protected void AddImage(string imagePath, int width, int height, float xx, float yy)
        {
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
            img.ScaleAbsolute(width, height);
            img.SetAbsolutePosition(xx, yy);// _doc.PageSize.Width - img.ScaledWidth) / 2, 0);
            _writer.DirectContent.AddImage(img);

            _doc.Add(img);
        }

        protected void DrawLine(float x, float y, float xx, float yy, float size = 0)
        {
            PdfContentByte cb = _writer.DirectContent;
            if (size == 0)
            {
                cb.SetLineWidth(BorderWidth);
            }
            else
            {
                cb.SetLineWidth(size);
            }

            cb.MoveTo(x, y);
            cb.LineTo(xx, yy);
            cb.Stroke();
        }

        protected void DrawText(string text, float x, float y, float fontSize = 10)
        {
            PdfContentByte cb = _writer.DirectContent;
            cb.BeginText();
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(JointacFont.BaseFontCN, fontSize);
            cb.ShowTextAligned(Element.ALIGN_LEFT, text, x, y, 0);
            cb.EndText();
        }

        #endregion

        /// <summary>
        /// 矩形cell
        /// </summary>
        /// <returns></returns>
        public PdfPCell CreatePdfPCellRect()
        {
            var imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\rect.png");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);

            var cell = new PdfPCell(img, false);
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = 0;
            return cell;

        }

        public PdfPCell CreateImageCell(Image image, int rowSpan = 1, int colspan = 1, int HorizontalAlignment = Element.ALIGN_CENTER, int VerticalAlignment = Element.ALIGN_MIDDLE)
        {
            var cell = new PdfPCell(image, true);
            cell.BorderColor = BaseColor.BLACK;

            cell.Border = 0;

            if (FixedHeight != 0)
                cell.FixedHeight = FixedHeight;

            cell.Padding = Padding;
            cell.Rowspan = rowSpan;
            cell.Colspan = colspan;
            cell.UseAscender = true;
            cell.VerticalAlignment = VerticalAlignment;
            cell.HorizontalAlignment = HorizontalAlignment;
            return cell;
        }

        private PdfPCell CreateBaseCell(Phrase p, int rowSpan, int colspan, int HorizontalAlignment, int VerticalAlignment)
        {
            var cell = new PdfPCell(p);
            cell.BorderColor = BaseColor.BLACK;

            cell.BorderWidthRight = 0f;
            cell.BorderWidthTop = 0f;
            cell.BorderWidthLeft = 0f;
            cell.BorderWidthBottom = 0f;

            if (FixedHeight != 0)
                cell.FixedHeight = FixedHeight;

            cell.Padding = Padding;
            cell.Rowspan = rowSpan;
            cell.Colspan = colspan;
            cell.UseAscender = true;
            cell.VerticalAlignment = VerticalAlignment;
            cell.HorizontalAlignment = HorizontalAlignment;
            return cell;
        }

        /// <summary>
        /// Invoice
        /// </summary>
        /// <param name="textEN"></param>
        /// <param name="textCN"></param>
        /// <param name="fontEn"></param>
        /// <param name="fontCN"></param>
        /// <param name="isNewLine"></param>
        /// <returns></returns>
        protected Phrase CombineString(string textEN, string textCN, iTextSharp.text.Font fontEn, iTextSharp.text.Font fontCN, bool isNewLine = true)
        {
            if (isNewLine)
                textCN = "\r\n" + textCN;
            Chunk chunk1 = new Chunk(textEN, fontEn);
            Chunk chunk2 = new Chunk(textCN, fontCN);
            Phrase p = new Phrase();
            p.Add(chunk1);
            p.Add(chunk2);
            return p;
        }


        /// <summary>
        /// GRN
        /// </summary>
        /// <param name="textCN"></param>
        /// <param name="fontCN"></param>
        /// <param name="textEN"></param>
        /// <param name="fontEn"></param>
        /// <returns></returns>
        protected Phrase CombineString(string textCN, iTextSharp.text.Font fontCN, string textEN, iTextSharp.text.Font fontEn)
        {
            Chunk chunk1 = new Chunk(textCN, fontCN);
            Chunk chunk2 = new Chunk(textEN, fontEn);
            Phrase p = new Phrase();
            p.Add(chunk1);
            p.Add(chunk2);
            return p;
        }
    }
}
