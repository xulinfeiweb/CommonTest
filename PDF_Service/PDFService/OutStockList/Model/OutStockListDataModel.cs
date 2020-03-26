using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 出库清单
    /// </summary>
    public class OutStockListDataModel
    {
        public DataHeader Header { get; set; }
        public int Flag { get; set; }
        /// <summary>
        /// 盖章
        /// </summary>
        public string Chapter { get; set; }
        /// <summary>
        /// 印章的图片路径
        /// </summary>
        public string ChapterPaths { get; set; }
        public List<OutStockListModel> List { get; set; }

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
                    sum = List.Sum(x => x.totalQty);
                }
                return sum;
            }
        }
        /// <summary>
        /// 总货值
        /// </summary>
        public decimal SumTotalAmount
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => x.TotalAmount);
                }
                return sum;
            }
        }
        /// <summary>
        /// 总件数
        /// </summary>
        public decimal SumLegalClearQty
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => x.LegalClearQty);
                }
                return sum;
            }
        }
        /// <summary>
        /// 总净重
        /// </summary>
        public decimal SumNetWeight
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => (x.LegalClearQty * x.NetWeight));
                }
                return sum;
            }
        }
        /// <summary>
        ///  总金额
        /// </summary>
        public decimal SumAmount
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => x.TotalAmount);
                }
                return sum;
            }
        }

    }
    
}
