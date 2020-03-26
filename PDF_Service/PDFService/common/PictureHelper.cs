using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common
{
    public class PictureHelper
    {


        public static Bitmap DrawRect(int width,int height,float borderWidth,Color borderColor)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);

            Pen pen = new Pen(borderColor, borderWidth);
            Rectangle rect = new Rectangle((int)borderWidth, (int)borderWidth, (int)(width - borderWidth * 2), (int)(height - borderWidth * 2));
            g.DrawRectangle(pen, rect);

            g.Dispose();
            return bmp;
        }



    }
}
