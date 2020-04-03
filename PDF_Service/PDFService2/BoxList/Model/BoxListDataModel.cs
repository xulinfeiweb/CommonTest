using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 商检发票
    /// </summary>
    public class BoxListDataModel
    {
        public DataHeader Header { get; set; }
        public List<InvoiceModel> List { get; set; }
        public decimal WeightWrites { get; set; }
        /// <summary>
        /// 箱数
        /// </summary>
        public string Boxs { get; set; }
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
        public decimal SumTotalNet
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => x.TotalNet);
                }
                return sum;
            }
        }
    }
}
