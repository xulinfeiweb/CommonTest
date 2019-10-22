using SHDocVw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Test
{
    /// <summary>
    /// 通过WebBrowser抓取网页数据
    /// WebBrowserCrawler  webBrowserCrawler=new WebBrowserCrawler();
    /// 示例：File.WriteAllText(Server.MapPath("sample.txt"),webBrowserCrawler.GetReult(http://www.in2.cc/sample/waterfalllab.htm));
    /// </summary>
    public class WebBrowserCrawler
    {
        // WebBrowser
        private SHDocVw.WebBrowser _WebBrowder;
        //最後結果
        private string _Result { get; set; }
        //網址
        private string _Path { get; set; }
        //当一直在抓取资料，允许等待的的最大秒数，超时时间（秒）
        private int _MaxWaitSeconds { get; set; }

        public delegate bool MyDelegate(object sender, TestEventArgs e);
        /// <summary>
        /// 是否达到停止加载条件
        /// </summary>
        public event MyDelegate IsStopEvent;

        /// <summary>
        /// 對外公開的Method
        /// </summary>
        /// <param name="url">URL Path</param>
        /// <param name="maxWaitSeconds">最大等待秒数</param>
        /// <returns></returns>
        public string GetReult(string url, int maxWaitSeconds = 60)
        {
            _Path = url;
            _MaxWaitSeconds = maxWaitSeconds <= 0 ? 60 : maxWaitSeconds;

            var mThread = new Thread(FatchDataToResult);
            //Apartment 是處理序當中讓物件共享相同執行緒存取需求的邏輯容器。 同一 Apartment 內的所有物件都能收到 Apartment 內任何執行緒所發出的
            //.NET Framework 並不使用 Apartment；Managed 物件必須自行以安全執行緒 (Thread-Safe) 的方式運用一切共
            //因為 COM 類別使用 Apartment，所以 Common Language Runtime 在 COM Interop 的狀況下呼叫出 COM 物件時必須建立 Apartment 並且加以初
            //Managed 執行緒可以建立並且輸入只容許一個執行緒的單一執行緒 Apartment (STA)，或者含有一個以上執行緒的多執行緒 Apartment (MT
            //只要把執行緒的 ApartmentState 屬性設定為其中一個 ApartmentState 列舉型別 (Enumeration)，即可控制所建立的 Apartment 屬於哪種
            //因為特定執行緒一次只能初始化一個 COM Apartment，所以第一次呼叫 Unmanaged 程式碼之後就無法再變更 Apartment
            //From : http://msdn.microsoft.com/zh-tw/library/system.threading.apartmentstate.
            mThread.SetApartmentState(ApartmentState.STA);
            mThread.Start();
            mThread.Join();

            return _Result;
        }

        /// <summary>
        /// Call _WebBrowder 抓取資料
        /// For thread Call
        /// </summary>
        private void FatchDataToResult()
        {
            _WebBrowder = new SHDocVw.WebBrowser();
            _WebBrowder.ScriptErrorsSuppressed = true;
            _WebBrowder.Navigate(_Path);
            DateTime firstTime = DateTime.Now;
            //處理目前在訊息佇列中的所有 Windows
            //如果在程式碼中呼叫 DoEvents，您的應用程式就可以處理其他事件。例如，如果您的表單將資料加入 ListBox 並將 DoEvents 加入程式碼中，則當另一個視窗拖到您的表單上時，該表單將重
            //如果您從程式碼移除 DoEvents，您的表單將不會重新繪製，直到按鈕按一下的事件處理常式執
            while ((DateTime.Now - firstTime).TotalSeconds <= _MaxWaitSeconds)
            {
                if (_WebBrowder.Document != null && _WebBrowder.Document.Body != null &&
                   !string.IsNullOrEmpty(_WebBrowder.Document.Body.OuterHtml) &&
                   this.IsStopEvent != null)
                {
                    string html = _WebBrowder.Document.Body.OuterHtml;
                    bool rs = this.IsStopEvent(null, new TestEventArgs(html));
                    if (rs)
                    {
                        this._Result = html;
                        break;
                    }
                }
                Application.DoEvents();
            }
            _WebBrowder.Dispose();
        }
    }
}
