using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //service url=http://bsp-ois.sit.sf-express.com:9080/bsp-ois/ws/expressService?wsdl
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                                <Response service='QuerySFWaybillService' lang='zh_CN'>
                                  <Head>OK</Head>
                                  <Body>
                                    <Waybill orderId='QIAO-20180524002' waybillNo='SF1011470276085' meterageWeightQty='1.0' customerAcctCode='9999999999' limitTypeCode='T6' consignedTm='2019-10-11 11:30:24' realWeightQty='1.0' productName='顺丰特惠' expressTypeCode='B1' addresseeAddr='广东省广州市海珠区海珠区宝芝林大厦701室' addresseePhone='33992159' addresseeContName='风一样的旭哥' dProvince='广东省' dCity='广州市' consignorAddr='福田区新洲十一街万基商务大厦26楼' consignorPhone='15012345678' consignorMobile='15012345678' consignorContName='丰哥' jProvince='广东省' jCity='深圳市' consignorCompName='顺丰镖局' addresseeCompName='神罗科技'>
                                      <Fee type='1' name='主运费' value='9.0' paymentTypeCode='3' settlementTypeCode='2' customerAcctCode='9999999999'/>
                                    </Waybill>
                                  </Body>
                                </Response>";
                XmlDocument XmlLoad = new XmlDocument();
                XmlLoad.LoadXml(xml);

                string da = "DE##EwA9Tr76gZrY%2BRivrmj35u7yGj%2FW3PigTxr5%2F7QhE%2FM9KMSGgRfyQqN%2FL%2Fek%2FyoABPzvBDzpC4EDZL4eVg4PLevRPpA%3D";
                XmlNode Response = XmlLoad.SelectSingleNode("Response/Head");
                XmlNode Waybill = XmlLoad.SelectSingleNode("Response/Body/Waybill");
                Dictionary<string, string> retList = new Dictionary<string, string>();
                foreach (XmlAttribute item in Waybill.Attributes)
                {
                    retList.Add(item.Name, item.Value);
                }
                string jsonstr = JsonUntity.SerializeDictionaryToJsonString(retList);
                richTextBox1.Text = jsonstr;
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message;
            }
        }
        public string GetNumberOfStr(string str)
        {
            return new Regex("[^\\d.\\d]").Replace(str, "");
        }
        //https://www.cnblogs.com/senyier/p/6783555.html
        private void button2_Click(object sender, EventArgs e)
        {
            string result = null;
            richTextBox1.Text = string.Empty;
            result = SFOrder();
            richTextBox1.Text = result;
        }
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { // 总是接受
            return true;
        }
        public string SFOrder()
        {
            string xml = @"<Request service='RouteService' lang='zh-CN'>
                          <Head>SHZTGJWL,K21ZMKG3U0v9</Head>
                          <Body>
                            <RouteRequest tracking_type='1' method_type='1' tracking_number='235523224478' check_phoneNo='1373'/>
                          </Body>
                        </Request>";

            xml = @"<Request service='OrderService' lang='zh-CN'>
              <Head>SHZTGJWL,K21ZMKG3U0v9</Head>
              <Body>
                <Order
                          orderid='QIAO-20180524002'
                              express_type='2'
                                j_province='广东省'
                                j_city='深圳市'
                                j_company='顺丰镖局'
                                j_contact='丰哥'
                                j_tel='15012345678'
                                j_address='福田区新洲十一街万基商务大厦26楼'
                       d_province='广东省'
                       d_city='广州市'
                       d_county='海珠区'
                       d_company='神罗科技'
                       d_contact='风一样的旭哥'
                       d_tel='33992159'
                       d_address='宝芝林大厦701室'
                                parcel_quantity='1'
                                pay_method='3'
                                custid ='9999999999'
                                customs_batchs=''
                                  is_unified_waybill_no='1'>
                </Order>
              </Body>
            </Request>";

            xml = @"<Request service='QuerySFWaybillService' lang='zh-CN'> 
                  <Head>SHZTGJWL</Head>  
                  <Body> 
                    <Waybill type='2' waybillNo='SF1011470276085' orderId='' phone='2159' /> 
                  </Body> 
                </Request>";
 
            string Checkword = "K21ZMKG3U0v9";
            string verifyCode = MD5ToBase64String(xml + Checkword);
            string requestUrl = "http://bsp-oisp.sf-express.com/bsp-oisp/sfexpressService";//开发环境地址
            requestUrl = "http://218.17.248.244:11080/bsp-oisp/sfexpressService";
            //requestUrl = "http://218.17.248.244:11080/bsp-oisp/ws/sfexpressService";
            //requestUrl= "http://bsp-oisp.sf-express.com/bsp-oisp/sfexpressService";
            return DoPost(requestUrl, xml, verifyCode);//这就得到了返回结果，解析部分就不记了，想起来也没什么小点了
        }
        public string MD5ToBase64String(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] MD5 = md5.ComputeHash(Encoding.UTF8.GetBytes(str));//MD5(注意UTF8编码)
            string result = Convert.ToBase64String(MD5);//Base64
            return result;
        }
        public string DoPost(string Url, string xml, string verifyCode)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                string postData = string.Format("xml={0}&verifyCode={1}", xml, verifyCode);
                //请求
                WebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.Timeout = 5000;
                request.Proxy = null;
                request.ContentLength = Encoding.UTF8.GetByteCount(postData);
                byte[] postByte = Encoding.UTF8.GetBytes(postData);
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(postByte, 0, postByte.Length);
                reqStream.Close();

                //读取
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static class JsonUntity
        {
            /// <summary>
            /// 将字典类型序列化为json字符串
            /// </summary>
            /// <typeparam name="TKey">字典key</typeparam>
            /// <typeparam name="TValue">字典value</typeparam>
            /// <param name="dict">要序列化的字典数据</param>
            /// <returns>json字符串</returns>
            public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
            {
                if (dict.Count == 0)
                    return "";

                string jsonStr = JsonConvert.SerializeObject(dict);
                return jsonStr;
            }

            /// <summary>
            /// 将json字符串反序列化为字典类型
            /// </summary>
            /// <typeparam name="TKey">字典key</typeparam>
            /// <typeparam name="TValue">字典value</typeparam>
            /// <param name="jsonStr">json字符串</param>
            /// <returns>字典数据</returns>
            public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
            {
                if (string.IsNullOrEmpty(jsonStr))
                    return new Dictionary<TKey, TValue>();

                Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

                return jsonDict;

            }
             
        }
    }
}
