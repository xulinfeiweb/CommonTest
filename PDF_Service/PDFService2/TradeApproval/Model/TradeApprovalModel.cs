using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TradeApprovalModel
    {
        /// <summary>
        /// rowindex
        /// </summary>
        public int rowindex { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 项号
        /// </summary>
        public string H2000index { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 备件号
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal totalQty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string LegalUnitEN { get; set; }
        /// <summary>
        /// 进库单价
        /// </summary>
        public decimal UnitPriceIn { get; set; }
        /// <summary>
        /// 币制
        /// </summary>
        public string CurrencyEN { get; set; }
        /// <summary>
        /// 出库单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal AmountIn { get; set; }
        /// <summary>
        /// MadeInCode
        /// </summary>
        public string MadeInCode { get; set; }
        /// <summary>
        /// LineIndex
        /// </summary>
        public int LineIndex { get; set; }

    }
}
