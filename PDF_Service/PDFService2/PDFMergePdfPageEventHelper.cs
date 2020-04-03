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
            if (tpl == null)
            {
                tpl = writer.DirectContent.CreateTemplate(30, 30);
            }
            #region 页眉
            Font fontFooter2 = JointacFont.FontCN(9, Font.NORMAL);
            if (PAGE_NUMBER)
            {
                Phrase header = new Phrase("PAGE " + (writer.PageNumber) + " OF ", fontFooter2);
                PdfContentByte cb = writer.DirectContent;
                cb.SetCharacterSpacing(1.3f);
                //页眉显示的位置
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, header, document.Right - 70, document.Top - 20, 0);
                //模版 显示总共页数
                cb.AddTemplate(tpl, document.Right - 53 + document.LeftMargin, document.Top - 20);//调节模版显示的位置
            }
            #endregion
        }
    }
}
