using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// iTextSharp Font
    /// </summary>
    public class JointacFont
    {
        public static BaseFont BaseFontCN = BaseFont.CreateFont("C://WINDOWS//Fonts//simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        public static BaseFont BaseFontCN2 = BaseFont.CreateFont("C://WINDOWS//Fonts//simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//黑体

        public static BaseFont BaseFontCN3 = BaseFont.CreateFont("C://WINDOWS//Fonts//simkai.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//楷体

        public static BaseFont BaseFontCN4 = BaseFont.CreateFont("C://WINDOWS//Fonts//simfang.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//仿宋

        public static BaseFont BaseFontCN5 = BaseFont.CreateFont("C://WINDOWS//Fonts//msyh.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//微软雅黑

        public static BaseFont BaseFontEN = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        
        public static Font FontCN(float size = 9,int style = Font.NORMAL)
        {
            return new Font(BaseFontCN, size, style, new BaseColor(0, 0, 0));
        }

        public static Font FontEN(float size = 9, int style = Font.NORMAL)
        {
            return new Font(BaseFontEN, size, style, new BaseColor(0, 0, 0));
        }

    }

    public class JointacFormat
    {
        public const string FormatDate = "yyyy-MM-dd";

        public const string FormatTime = "HH:mm:ss";

        public const string FormatDateTime = FormatDate + " " + FormatTime;
    }
}
