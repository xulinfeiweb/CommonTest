using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Common
{
    /// <summary>
    /// 页眉页脚水印设置
    /// </summary>
    public class PDFMergePdfPageEventHelper : PdfPageEventHelper, IPdfPageEvent
    {
        public PdfTemplate tpl = null;

        public bool PAGE_NUMBER = true;

        //关闭PDF文档时
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            //template 显示总页数
            tpl.BeginText();
            tpl.SetFontAndSize(JointacFont.BaseFontCN, 10);//生成的模版的字体、颜色
            tpl.ShowText(writer.PageNumber.ToString());//模版显示的内容
            tpl.EndText();
            tpl.ClosePath();
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            #region 页脚

            if (tpl == null)
            {
                tpl = writer.DirectContent.CreateTemplate(30, 30);
            }

            Font fontFooter = JointacFont.FontCN(11, Font.NORMAL);
            if (PAGE_NUMBER)
            {
                //string page = "   " + (writer.PageNumber) + "/";
                //Chunk chunk = new Chunk(page, JointacFont.FontCN());
                //Phrase footer = new Phrase();
                //footer.Add(chunk);

                Phrase footer = new Phrase("Page " + (writer.PageNumber) + "/", fontFooter);
                PdfContentByte cb = writer.DirectContent;
                cb.SetCharacterSpacing(1.3f);

                #region 画线

                //cb.SetColorStroke(new Color(0, 0, 0));//线颜色
                //cb.SetColorFill(Color.BLACK);//字体颜色
                //cb.MoveTo(document.Left, document.Bottom + 25);
                //cb.LineTo(document.Right - 20, document.Bottom + 25);//线位置
                //cb.SetLineWidth(0.8f);
                //cb.ClosePathFillStroke();
                #endregion

                //页脚显示的位置
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, footer, document.Right - 59, document.Bottom + 22, 0);
                //模版 显示总共页数
                cb.AddTemplate(tpl, document.Right - 50 + document.LeftMargin, document.Bottom + 22);//调节模版显示的位置
            }

            #endregion
        }
    }
}
