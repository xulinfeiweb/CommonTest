using LeaRun.Util;
using mshtml;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    [ComVisible(true)]
    public partial class TestWeb : Form
    {
        public TestWeb(string[] args)
        {
            InitializeComponent();
            webBrowserTest.ObjectForScripting = this;
            SHDocVw.WebBrowser wb = (SHDocVw.WebBrowser)webBrowserTest.ActiveXInstance;
            wb.BeforeScriptExecute += Wb_BeforeScriptExecute;
        }

        private void TestWeb_Load(object sender, EventArgs e)
        {

            //Console.WriteLine("12" ?? "");
            //Config.SetValue("abc", "11111111111111");
        }

        private void Wb_BeforeScriptExecute(object pDispWindow)
        {
            string text = webBrowserTest.DocumentText;
            IHTMLDocument2 vDocument = (IHTMLDocument2)webBrowserTest.Document.DomDocument;
            try
            {
                string WantedPath = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf(@"\"));
                string WantedPath2 = WantedPath.Substring(0, WantedPath.LastIndexOf(@"\"));
                string path = WantedPath2 + ConfigurationManager.AppSettings["scriptPath"];

                string script = File.ReadAllText(path);
                #region C#中字符串的编解码和乱码问题 (OK)
                //StringBuilder sb = new StringBuilder();
                //string source = "hello 浣犲ソ";

                //foreach (var e1 in Encoding.GetEncodings())
                //{
                //    foreach (var e2 in Encoding.GetEncodings())
                //    {
                //        byte[] unknow = Encoding.GetEncoding(e1.CodePage).GetBytes(source);
                //        string result = Encoding.GetEncoding(e2.CodePage).GetString(unknow);
                //        sb.AppendLine(string.Format("{0} => {1} : {2}", e1.CodePage, e2.CodePage, result));
                //    }
                //}
                //File.WriteAllText(WantedPath2 + "\\js\\test.js", sb.ToString());

                //int CURRENT_CODE_PAGE = Encoding.Default.CodePage;
                //int TARGET_CODE_PAGE = Encoding.UTF8.CodePage;
                //byte[] raw = Encoding.GetEncoding(CURRENT_CODE_PAGE).GetBytes(script);
                //string newText = Encoding.GetEncoding(TARGET_CODE_PAGE).GetString(raw);
                //File.WriteAllText(WantedPath2 + "\\js\\login1.js", newText);
                #endregion

                vDocument.parentWindow.execScript(script, "javascript");
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }
        public string GetAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        private void webBrowserTest_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // 定义脚本.
            //HtmlElement ele = webBrowserTest.Document.CreateElement("script");
            //ele.SetAttribute("type", "text/javascript");
            //ele.SetAttribute("text", script3);
            //webBrowserTest.Document.Body.AppendChild(ele);
        }
        public void sendToCSharp(string data)
        {
            string s = data;

            MessageBox.Show("12312");
        }
        private void btn_click_Click(object sender, EventArgs e)
        {
            //根据franme的name得到对象
            //HtmlElement elem = webBrowserTest.Document.Window.Frames["iframeId"].Document.CreateElement("script");
            //elem.SetAttribute("type", "text/javascript");
            //elem.SetAttribute("text", script3);
            //webBrowserTest.Document.Window.Frames["iframeId"].Document.Body.AppendChild(elem);
        }
    }
}
