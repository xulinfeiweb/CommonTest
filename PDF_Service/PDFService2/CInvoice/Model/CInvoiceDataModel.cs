using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 商检发票
    /// </summary>
    public class CInvoiceDataModel
    {
        public DataHeader Header { get; set; }
        public List<InvoiceModel> List { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal Sum
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => x.ClearQty);
                }
                return sum;
            }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal SumAmount
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => (x.ClearQty * x.UnitPrice));
                }
                return sum;
            }
        }
    }
}
