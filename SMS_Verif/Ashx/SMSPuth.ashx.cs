using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace SMS_Verif
{
    /// <summary>
    /// SMSPuth 的摘要说明
    /// </summary>
    public class SMSPuth : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// API接口
        /// </summary>
        private const string WebAPI = "http://192.168.192.22/WechatApi/jointac/wechat/private/";
        /// <summary>
        /// 接口访问验证key
        /// </summary>
        private const string ApiKey = "54B417B0-463D-4F2B-8075-0A20EEDB773B";
        /// <summary>
        /// 短信发送
        /// </summary>
        private const string getsms = "getsms";
        /// <summary>
        /// 手机号查快递信息
        /// </summary>
        private const string waybilllist = "waybilllist";
        /// <summary>
        /// 运单跟踪信息
        /// </summary>
        private const string waybilltrack = "waybilltrack";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            object obj = null;
            try
            {
                context.Response.ContentType = "text/plain";
                string param = context.Request.Params["param"];
                switch (param)
                {
                    case "sendsms":
                        obj = SendSmsMsg(context);
                        break;
                    case "chkIphone":
                        obj = ChkIphone(context);
                        break;
                    case "puthmsg":
                        obj = puthMsg(context);
                        break;
                    case "gettrack":
                        obj = GetWayBilltrack(context);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                obj = new
                {
                    msg = "系统错误",
                    Success = false
                };
                obj = Json.ToJson(obj);
            }
            context.Response.Write(obj);
        }
        public object SendSmsMsg(HttpContext context)
        {
            object obj = null;
            string miphone = context.Request.Params["Miphone"];
            string code = GetVsCode();
            string url1 = WebAPI + waybilllist;
            Dictionary<string, string> dic1 = new Dictionary<string, string>();
            dic1.Add("key", ApiKey);
            dic1.Add("MobilePhone", miphone);
            dic1.Add("Security", "");
            dic1.Add("Page", "1");
            dic1.Add("PageSize", "10");
            string result = Post(url1, dic1);
            ServiceResult sr = result.ToObject<ServiceResult>();
            if (sr.code != 0)
            {
                obj = new
                {
                    msg = "非付费用户，请联系客服。",
                    code = -1
                };
                return Json.ToJson(obj);
            }
            context.Session["sms"] = code;
            context.Session["iphone"] = miphone;
            context.Session.Timeout = 5;//5分钟时效
            string text = code + "（上海展通国际物流有限公司，手机号验证码，300秒内有效）【回复6666可屏蔽】";
            string url = WebAPI + getsms;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("key", ApiKey);
            dic.Add("to", miphone);
            dic.Add("text", text);
            return Post(url, dic);
        }
        public object ChkIphone(HttpContext context)
        {
            object obj = null;
            string vscode = context.Request.Params["vsCode"];
            string miphone = context.Request.Params["Miphone"];
            string smsCode = context.Request.Params["smsCode"];
            //if (null == context.Session["vscode"] || vscode != context.Session["vscode"].ToString())
            //{
            //    obj = new
            //    {
            //        msg = "图片验证码错误。",
            //        code = -1
            //    };
            //    return Json.ToJson(obj);
            //}
            if (null == context.Session["sms"] || smsCode != context.Session["sms"].ToString())
            {
                obj = new
                {
                    msg = "短信验证码错误。",
                    code = -1
                };
                return Json.ToJson(obj);
            }
            if (null == context.Session["iphone"] || miphone != context.Session["iphone"].ToString())
            {
                obj = new
                {
                    msg = "请重新验证手机号。",
                    code = -1
                };
                return Json.ToJson(obj);
            }
            string url = WebAPI + waybilllist;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("key", ApiKey);
            dic.Add("MobilePhone", miphone);
            dic.Add("Security", "");
            dic.Add("Page", "1");
            dic.Add("PageSize", "10");
            return Post(url, dic);
        }
        public object puthMsg(HttpContext context)
        {
            object obj = null;
            string miphone = context.Request.Params["Miphone"];
            string WaybillNumber = context.Request.Params["WaybillNumber"];
            string ExpressCompany = context.Request.Params["ExpressCompany"];
            string url1 = WebAPI + waybilltrack;
            if (null == context.Session["iphone"] || miphone != context.Session["iphone"].ToString())
            {
                obj = new
                {
                    msg = "请重新验证手机号。",
                    code = -1
                };
                return Json.ToJson(obj);
            }
            Dictionary<string, string> dic1 = new Dictionary<string, string>();
            dic1.Add("key", ApiKey);
            dic1.Add("ExpressCompany", ExpressCompany);
            dic1.Add("WaybillNumber", WaybillNumber);
            string result = Post(url1, dic1);
            ServiceResult sr = result.ToObject<ServiceResult>();
            string text = "";
            if (sr.code == 0)
            {
                List<wayBillEntity> list = Json.ToObject<List<wayBillEntity>>(sr.data.ToString());
                text = list[0].scan;
            }
            else
            {
                obj = new
                {
                    msg = "抱歉，运单：" + WaybillNumber + "暂无跟踪信息，无法推送。",
                    code = -1
                };
                return Json.ToJson(obj);
            }
            string url2 = WebAPI + getsms;
            Dictionary<string, string> dic2 = new Dictionary<string, string>();
            dic2.Add("key", ApiKey);
            dic2.Add("to", miphone);
            dic2.Add("text", text);
            return Post(url2, dic2);
        }
        public static string GetVsCode()
        {
            string chkCode = "";
            char[] character = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 6; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            return chkCode;
        }
        public object GetWayBilltrack(HttpContext context)
        {
            string WaybillNumber = context.Request.Params["WaybillNumber"];
            string ExpressCompany = context.Request.Params["ExpressCompany"];

            string url = WebAPI + waybilltrack;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("key", ApiKey);
            dic.Add("ExpressCompany", ExpressCompany);
            dic.Add("WaybillNumber", WaybillNumber);
            return Post(url, dic);
        }
        public static string RemoveSpecialCharacter(string hexData)
        {
            //"[ \\[ \\] \\^ \\-_*×――(^)$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]"
            return Regex.Replace(hexData, "[ \\[ \\] \\^ \\-_*×――(^)$%~!@#$…&%￥—+=<>《》•`·\"-]", "").ToUpper();
        }
        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        public class ServiceResult
        {
            public int code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }
        public class wayBillEntity
        {
            public string waybillnumber { get; set; }
            public string describe { get; set; }
            public string location { get; set; }
            public string scan { get; set; }
            public DateTime datetime { get; set; }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}