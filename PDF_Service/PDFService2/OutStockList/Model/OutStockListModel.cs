using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class OutStockListModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string CID { get; set; }

        /// <summary>
        /// 项号
        /// </summary>
        public string H2000index { get; set; } 
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; } 
        /// <summary>
        /// 账册备件号
        /// </summary>
        public string ProductCodeC { get; set; } 

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal totalQty { get; set; } 

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; } 

        /// <summary>
        /// 实物备件号
        /// </summary>
        public string ProductCode { get; set; } 

        /// <summary>
        /// 件数
        /// </summary>
        public decimal LegalClearQty { get; set; } 
        /// <summary>
        /// 净重
        /// </summary>
        public decimal NetWeight { get; set; } 
        /// <summary>
        /// 货值
        /// </summary>
        public decimal Amount { get; set; } 
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime OutDate { get; set; }
        /// <summary>
        /// LineIndex
        /// </summary>
        public int LineIndex { get; set; }
        /// <summary>
        /// newLineIndex
        /// </summary>
        public decimal newLineIndex { get; set; }
        /// <summary>
        /// 原产国
        /// </summary>
        public string MadeInEN { get; set; } 
        /// <summary>
        /// 数量单位
        /// </summary>
        public string LegalUnitEN { get; set; } 
        /// <summary>
        /// 重量单位
        /// </summary>
        public string WeightUnitEN { get; set; } 
        /// <summary>
        /// 币制
        /// </summary>
        public string CurrencyEN { get; set; } 
        /// <summary>
        /// 运输代码
        /// </summary>
        public string ShipCode { get; set; } 
        /// <summary>
        /// 发票号
        /// </summary>
        public string PONumber { get; set; } 
        /// <summary>
        /// 分运单号
        /// </summary>
        public string HAWB { get; set; }
    }
}
