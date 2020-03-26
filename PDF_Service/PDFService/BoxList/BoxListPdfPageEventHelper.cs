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
    public class BoxListPdfPageEventHelper : PdfPageEventHelper, IPdfPageEvent
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


            #region 页眉
            //Font fontFooter2 = JointacFont.FontCN(9, Font.NORMAL);
            //if (PAGE_NUMBER)
            //{
            //    Phrase header;
            //    int right = 60;
            //    header = new Phrase("第 " + (writer.PageNumber) + " 页,共   页", fontFooter2);

            //    PdfContentByte cb = writer.DirectContent;
            //    cb.SetCharacterSpacing(1.3f);
            //    //页眉显示的位置
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, header, document.Right - right, document.Top - 20, 0);
            //    //模版 显示总共页数
            //    cb.AddTemplate(tpl, document.Right - 53 + document.LeftMargin, document.Top - 20);//调节模版显示的位置
            //}

            //Font fontHeader = new Font(JointacFont.BaseFontCN, 12, Font.ITALIC);
            //PdfContentByte pch = writer.DirectContent;
            //pch.SetCharacterSpacing(1.3f);

            //pch.SetColorStroke(new Color(200, 200, 200));//线颜色
            //pch.SetColorFill(Color.BLACK);//字体颜色
            //pch.MoveTo(document.Left, document.Top - 5);
            //pch.LineTo(document.Right, document.Top - 5);//线位置
            //pch.ClosePathFillStroke();

            //Phrase header = new Phrase("Jointac PDF File", fontHeader);
            ////页眉显示位置
            //ColumnText.ShowTextAligned(pch, Element.ALIGN_CENTER, header, document.Right / 2, document.Top + 10, 0);

            #endregion

            PdfContentByte cba = writer.DirectContentUnder;

            #region 印章(Seal)
            //System.Drawing.Image seal = CreateSeal();
            //iTextSharp.text.Image sealImage = iTextSharp.text.Image.GetInstance(seal, Color.WHITE);
            //sealImage.ScalePercent(40);//40%比例缩小
            //sealImage.SetAbsolutePosition(document.Right - 160, 30);
            //cba.AddImage(sealImage);

            #endregion


            #region 背景色

            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1263, 893);
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            //System.Drawing.Color c = System.Drawing.Color.FromArgb(0xffffff);//0x33ff33     0xffffff

            //System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(c);//这里修改颜色
            //g.FillRectangle(b, 0, 0, 1263, 893);
            //System.Drawing.Image ig = bmp;
            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ig, new BaseColor(0xFF, 0xFF, 0xFF));
            //img.SetAbsolutePosition(0, 0);
            //cba.AddImage(img);

            #endregion


            #region 水印

            //string syurl = @"C:\Users\o\Desktop\sy.jpg";//水印图片path
            //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(syurl);
            //image.RotationDegrees = 30;//旋转角度

            //PdfGState gs = new PdfGState();
            //gs.FillOpacity = 0.1f;//透明度
            //cba.SetGState(gs);

            //int x = -1000;
            //for (int j = 0; j < 15; j++)
            //{
            //    x = x + 180;
            //    int a = x;
            //    int y = -170;
            //    for (int i = 0; i < 10; i++)
            //    {
            //        a = a + 180;
            //        y = y + 180;
            //        image.SetAbsolutePosition(a, y);
            //        cba.AddImage(image);
            //    }
            //}

            #endregion
        }

        public System.Drawing.Bitmap CreateSeal()
        {
            LinseedSeal _seal = new LinseedSeal();
            _seal.Company = "上海展通国际物流有限公司";
            _seal.SealSize = 300;
            _seal.CircleBorderWidth = 4;
            _seal.Indent = 30;
            _seal.LetterSpace = 3;
            _seal.TextFont = new System.Drawing.Font("宋体", 30, System.Drawing.FontStyle.Regular);
            var bitmap = _seal.GenerateSeal();

            return bitmap;
        }
    }
}
