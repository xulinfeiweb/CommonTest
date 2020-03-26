using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;


namespace Common
{
    public class LinseedSeal
    {
        #region Property

        private int _indent = 15;
        //外切圆
        private Rectangle _rect = new Rectangle(0, 0, 160, 160);
        //内切圆
        private Rectangle _rectcircle = new Rectangle(
                    new Point(15, 15),
                    new Size(130, 130)
                    );

        private CharDirectionEnum charDirect = CharDirectionEnum.Center;

        private int _degree = 90;

        public string Company { get; set; } = "上海展通国际物流有限公司";

        public string BaseString { get; set; } = "";

        public Font TextFont { get; set; } = new Font("宋体", 12, FontStyle.Regular);

        public Color FillColor { get; set; } = Color.Red;
        public Color PathColor { get; set; } = Color.Red;
        public Color TopColor { get; set; } = Color.Black;

        public int LetterSpace { get; set; } = 1;

        //public bool ShowPath { get; set; } = true;

        public int SealSize
        {
            set
            {
                _rect = new Rectangle(0, 0, value, value);
                _rectcircle = new Rectangle(
                    new Point(_rect.X + _indent, _rect.Y + _indent), 
                    new Size(_rect.Width - 2 * _indent, _rect.Height - _indent * 2)
                    );
            }
        }

        public int Indent
        {
            set
            {
                _indent = value;
                _rectcircle = new Rectangle(
                    _indent, 
                    _indent, 
                    _rect.Width - _indent * 2,
                    _rect.Height - _indent * 2
                    );
            }
        }

        public int CircleBorderWidth { get; set; } = 2;

        #endregion

        public LinseedSeal() { }

        public Bitmap GenerateSeal()
        {
            Bitmap bit = new Bitmap(_rect.Width, _rect.Height);
            Graphics g = Graphics.FromImage(bit);

            //Draw Star and BaseString
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            DrawSealBase(g);



            float[] fCharWidth = new float[Company.Length];
            float fTotalWidth = ComputeStringLength(Company, g, fCharWidth, LetterSpace, charDirect);

            double fSweepAngle = fTotalWidth * 360 / (_rectcircle.Width * Math.PI);
            double fStartAngle = 270 - fSweepAngle / 2;

            //angle and point
            PointF[] points = new PointF[Company.Length];
            double[] fCharAngle = new double[Company.Length];
            ComputeCharPos(fCharWidth, points, fCharAngle, fStartAngle);

            //Draw every character
            for (int i = 0; i < Company.Length; i++)
            {
                DrawRotatedText(g, Company[i].ToString(), (float)(fCharAngle[i] + _degree), points[i]);
            }

            g.Dispose();
            return bit;
        }


        private void DrawSealBase(Graphics g)
        {
            g.FillRectangle(Brushes.White, _rect);//背景矩形
            g.FillEllipse(new SolidBrush(FillColor), new Rectangle(CircleBorderWidth, CircleBorderWidth, _rect.Width - 2 * CircleBorderWidth, _rect.Height - 2 * CircleBorderWidth));//外切圆
            g.FillEllipse(Brushes.White, new Rectangle(2 * CircleBorderWidth, 2 * CircleBorderWidth, _rect.Width - 4 * CircleBorderWidth, _rect.Height - 4 * CircleBorderWidth));//内切圆

            #region Draw Center Star

            string strStar = "★";
            Font fnt = new Font(TextFont.FontFamily, TextFont.Size * 3);
            SizeF size = g.MeasureString(strStar, fnt);
            StringFormat strFmt = new StringFormat();
            strFmt.Alignment = StringAlignment.Center;
            strFmt.LineAlignment = StringAlignment.Center;

            RectangleF rectF = new RectangleF((_rect.Width - size.Width) / 2, (_rect.Height - size.Height) / 2, size.Width, size.Height);
            g.DrawString(strStar, fnt, new SolidBrush(FillColor), rectF, strFmt);

            #endregion

            #region BaseString 

            if (!string.IsNullOrEmpty(BaseString))
            {
                float[] fCharWidth = new float[BaseString.Length];
                float fTotalWidths = ComputeStringLength(BaseString, g, fCharWidth, 0, CharDirectionEnum.Center);
                float fLeftPos = (_rect.Width - fTotalWidths) / 2;
                PointF point;
                for (int i = 0; i < BaseString.Length; i++)
                {
                    point = new PointF(fLeftPos + fCharWidth[i] / 2, _rect.Height / 2 + size.Height / 2 + 10);
                    DrawRotatedText(g, BaseString[i].ToString(), 0, point);
                    fLeftPos += fCharWidth[i];
                }
            }

            #endregion
        }

        /// <summary>
        /// 绘制 横向字符
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="angle">旋转角度（范围）（单位：度）。</param>
        /// <param name="point">表示旋转中心</param>
        private void DrawRotatedText(Graphics g, string text, float angle, PointF point)
        {
            StringFormat strFmt = new StringFormat();
            strFmt.Alignment = StringAlignment.Center;
            strFmt.LineAlignment = StringAlignment.Center;

            GraphicsPath gp = new GraphicsPath(FillMode.Winding);
            gp.AddString(text, TextFont.FontFamily, (int)TextFont.Style, TextFont.Size, new Point((int)point.X, (int)point.Y), strFmt);


            Matrix m = new Matrix();
            m.RotateAt(angle, point);
            g.Transform = m;
            g.DrawPath(new Pen(FillColor), gp);
            g.FillPath(new SolidBrush(FillColor), gp);
        }


        /// <summary>
        /// 计算字符串长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="g"></param>
        /// <param name="fCharWidth"></param>
        /// <param name="fIntervalWidth"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private float ComputeStringLength(string text, Graphics g, float[] fCharWidth, float fIntervalWidth, CharDirectionEnum direction)
        {
            StringFormat strFmt = new StringFormat();
            strFmt.Trimming = StringTrimming.None;
            strFmt.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap | StringFormatFlags.LineLimit;

            SizeF size = g.MeasureString(text, TextFont, (int)TextFont.Size);
            RectangleF rect = new RectangleF(0f, 0f, size.Width, size.Height);
            CharacterRange[] crs = new CharacterRange[text.Length];
            for (int i = 0; i < text.Length; i++)
                crs[i] = new CharacterRange(i, 1);
            strFmt.FormatFlags = StringFormatFlags.NoClip;
            strFmt.SetMeasurableCharacterRanges(crs);
            strFmt.Alignment = StringAlignment.Near;

            Region[] regs = g.MeasureCharacterRanges(text, TextFont, rect, strFmt);

            float fTotalWidth = 0f;
            for (int i = 0; i < regs.Length; i++)
            {
                if (direction == CharDirectionEnum.Center || direction == CharDirectionEnum.OutSide)
                    fCharWidth[i] = regs[i].GetBounds(g).Width;
                else
                    fCharWidth[i] = regs[i].GetBounds(g).Height;
                fTotalWidth += fCharWidth[i] + fIntervalWidth;
            }
            fTotalWidth -= fIntervalWidth;
            return fTotalWidth;
        }


        private void ComputeCharPos(float[] charWidth, PointF[] recChars, double[] charAngle, double startAngle)
        {
            double fSweepAngle, fCircleLength;
            fCircleLength = _rectcircle.Width * Math.PI;
            for (int i = 0; i < charWidth.Length; i++)
            {
                fSweepAngle = charWidth[i] * 360 / fCircleLength;
                charAngle[i] = startAngle + fSweepAngle / 2;

                if (charAngle[i] < 270f)
                {
                    recChars[i] = new PointF(
                        _rectcircle.X + _rectcircle.Width / 2 - (float)(_rectcircle.Width / 2 * Math.Sin(Math.Abs(charAngle[i] - 270) * Math.PI / 180)),
                        _rectcircle.Y + _rectcircle.Width / 2 - (float)(_rectcircle.Width / 2 * Math.Cos(Math.Abs(charAngle[i] - 270) * Math.PI / 180))
                        );
                }
                else
                {
                    recChars[i] = new PointF(
                        _rectcircle.X + _rectcircle.Width / 2 + (float)(_rectcircle.Width / 2 * Math.Sin(Math.Abs(charAngle[i] - 270) * Math.PI / 180)),
                        _rectcircle.Y + _rectcircle.Width / 2 - (float)(_rectcircle.Width / 2 * Math.Cos(Math.Abs(charAngle[i] - 270) * Math.PI / 180))
                        );
                }

                fSweepAngle = (charWidth[i] + LetterSpace) * 360 / fCircleLength;
                startAngle += fSweepAngle;
            }
        }
    }

    /// <summary>
    /// 印章字符方向
    /// </summary>
    public enum CharDirectionEnum
    {
        /// <summary>
        /// 内部
        /// </summary>
        Center = 0,

        /// <summary>
        /// 外部
        /// </summary>
        OutSide = 1,

        /// <summary>
        /// 顺时针
        /// </summary>
        ClockWise = 2,

        /// <summary>
        /// 逆时针
        /// </summary>
        AntiClockWise = 3
    }
}
