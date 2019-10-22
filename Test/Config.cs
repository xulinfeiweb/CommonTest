using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace LeaRun.Util
{
    /// <summary>
    /// Config文件操作
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }
        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            string WantedPath = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf(@"\"));
            string WantedPath2 = WantedPath.Substring(0, WantedPath.LastIndexOf(@"\"))+ "\\App.config";
            xDoc.Load(WantedPath2);
            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(WantedPath2);
        }
    }
}
