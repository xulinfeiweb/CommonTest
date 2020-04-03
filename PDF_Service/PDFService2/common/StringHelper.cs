using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class StringHelper
    {
        /// <summary>
        /// 获取字符串长度   中文字符长度：2  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (str.Length == 0)
                return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        /// <summary>
        /// 获取字符串实际下标
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int GetIndex(string str, int length)
        {
            if (str.Length == 0)
                return 0;
            if (length == 0)
                return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                if (tempLen >= length)
                {
                    return i;
                }
            }

            return 0;
        }


        /// <summary>
        /// 拆分英文句子、段落(不截断单词)
        /// </summary>
        /// <param name="max">每行最大长度</param>
        /// <param name="str">拆分英文字符串</param>
        /// <returns>拆分得到的字符串</returns>
        public static List<string> TruncationEnglishString(int max = 20, string str = "I'm taken by a nursery rhyme.I want to make a ray of sunshine and never leave home")
        {
            List<string> list = new List<string>();
            var arr = str.Split(new char[] { ',', ';', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int length = 0;
            int index = 0;
            foreach (var s in arr)
            {
                if (s == arr.First())
                {
                    length = arr[0].Length + 1;
                    continue;
                }

                if (length + s.Length == max)
                {
                    list.Add(str.Substring(index, max + 1));
                    index += max + 1;
                    length = 0;
                }
                else if (length + s.Length > max)
                {
                    list.Add(str.Substring(index, length));
                    index += length;
                    length = s.Length + 1;
                }
                else
                {
                    length += (s.Length + 1);
                    if (length == max)
                    {
                        list.Add(str.Substring(index, length));
                        index += length;
                        length = 0;
                    }
                }

                if (s == arr.Last())
                {
                    list.Add(str.Substring(index));
                }
            }

            return list;
        }


        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        //public static List<string> SplitString(string text)
        //{
        //    if (string.IsNullOrEmpty(text))
        //    {
        //        return new List<string> { "" };
        //    }

        //    int firstLen = 40;//第一行长度
        //    int secondLen = 60;//第二行长度
        //    List<string> list = new List<string>();

        //    int len = GetLength(text);
        //    int rows = 0;

        //    if (len <= firstLen)
        //        rows = 1;
        //    else if (len > firstLen && len <= firstLen+secondLen)
        //        rows = 2;
        //    else if (len > firstLen + secondLen && len <= firstLen + secondLen*2)
        //        rows = 3;
        //    else if (len > firstLen + secondLen*2 && len <= firstLen + secondLen * 3)
        //        rows = 4;
        //    else
        //        rows = 5;

        //    int index = 0;
        //    int sub = 0;
        //    for (int i = 0; i < rows; i++)
        //    {
        //        string str = "";
        //        if (i == 0)
        //            sub = GetIndex(text, firstLen * (i + 1));
        //        else
        //            sub = GetIndex(text, firstLen + secondLen * i);

        //        if (i == rows - 1)
        //        {
        //            str = text.Substring(index);
        //        }
        //        else
        //        {
        //            str = text.Substring(index, sub-index);
        //        }
        //        list.Add(str);
        //        index += (sub - index);
        //    }

        //    return list;
        //}
        public static string GetDateCHString(string date)
        {
            string dateStr = "";
            if (string.IsNullOrWhiteSpace(date))
                dateStr = "年     月     日";
            else
            {
                DateTime dt = Convert.ToDateTime(date);
                dateStr = dt.Year + " 年 " + dt.Month + " 月 " + dt.Day + " 日";
            }
            return dateStr;
        }
        public static string GetDateCHString2(string date)
        {
            string dateStr = "";
            if (string.IsNullOrWhiteSpace(date))
                dateStr = "年     月     日";
            else
            {
                DateTime dt = Convert.ToDateTime(date);
                dateStr = dt.Year + "年" + dt.Month + "月" + dt.Day + "日";
            }
            return dateStr;
        }
    }
}
