using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class DataHeader
    {
        /// <summary>
        /// 经营单位
        /// </summary>
        public string wTradeName { get; set; }
        /// <summary>
        /// 贸易方式
        /// </summary>
        public string TradeName { get; set; }
        /// <summary>
        /// 结关方式
        /// </summary>
        public string ClearTypeName { get; set; }
        /// <summary>
        /// CIF,FOB,EXW
        /// </summary>
        public string TranscName { get; set; }
        /// <summary>
        /// 审批编号
        /// </summary>
        public string CID { get; set; }
        /// <summary>
        /// 报关单号
        /// </summary>
        public string CustomsNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> OutDateFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> OutDateTo { get; set; }
        /// <summary>
        /// 申报日期
        /// </summary>
        public Nullable<DateTime> ReportDate { get; set; }
        /// <summary>
        /// 审批日期
        /// </summary>
        public Nullable<DateTime> ExportDate { get; set; }
        /// <summary>
        /// 电子帐册
        /// </summary>
        public string H2000BookM { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string H2KOperator { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string CKQDTitle { get; set; }
        /// <summary>
        /// h0
        /// </summary>
        public string h0 { get; set; }
        /// <summary>
        /// h1
        /// </summary>
        public string h1 { get; set; }
        /// <summary>
        /// h2
        /// </summary>
        public string h2 { get; set; }
        /// <summary>
        /// f0
        /// </summary>
        public string f0 { get; set; }
        /// <summary>
        /// f1
        /// </summary>
        public string f1 { get; set; }
        /// <summary>
        /// f2
        /// </summary>
        public string f2 { get; set; }
        /// <summary>
        /// f3
        /// </summary>
        public string f3 { get; set; }
        /// <summary>
        /// f4
        /// </summary>
        public string f4 { get; set; }

        /// <summary>
        /// t0
        /// </summary>
        public string t0 { get; set; }
        /// <summary>
        /// t1
        /// </summary>
        public string t1 { get; set; }
        /// <summary>
        /// t2
        /// </summary>
        public string t2 { get; set; }
        /// <summary>
        /// t3
        /// </summary>
        public string t3 { get; set; }
        /// <summary>
        /// t4
        /// </summary>
        public string t4 { get; set; }
        /// <summary>
        /// b0
        /// </summary>
        public string b0 { get; set; }
        /// <summary>
        /// b1
        /// </summary>
        public string b1 { get; set; }
        /// <summary>
        /// b2
        /// </summary>
        public string b2 { get; set; }
        /// <summary>
        /// b3
        /// </summary>
        public string b3 { get; set; }
        /// <summary>
        /// b4
        /// </summary>
        public string b4 { get; set; }

        /// <summary>
        /// remark
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// invoicehead1
        /// </summary>
        public string invoicehead1 { get; set; }
        /// <summary>
        /// invoicehead2
        /// </summary>
        public string invoicehead2 { get; set; }
        /// <summary>
        /// invoicehead3
        /// </summary>
        public string invoicehead3 { get; set; }
        /// <summary>
        /// invoicehead4
        /// </summary>
        public string invoicehead4 { get; set; }

        /// <summary>
        /// GrossWt
        /// </summary>
        public decimal GrossWt { get; set; }
        /// <summary>
        /// ClearDNPO
        /// </summary>
        public string ClearDNPO { get; set; }
        /// <summary>
        /// SHIPTO
        /// </summary>
        public string SHIPTO { get; set; }
        /// <summary>
        /// SOLDTO
        /// </summary>
        public string SOLDTO { get; set; }
        /// <summary>
        /// SELLERFROM
        /// </summary>
        public string SELLERFROM { get; set; }
        
    }
}
