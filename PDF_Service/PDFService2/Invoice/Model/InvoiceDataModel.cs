using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class InvoiceDataModel
    {
        public DataHeader Header { get; set; }
        public int Flag { get; set; }
        /// <summary>
        /// 印章的图片路径
        /// </summary>
        public string ChapterPaths { get; set; }
        public decimal WeightWrite { get; set; }
        /// <summary>
        /// 盖章
        /// </summary>
        public string Chapter { get; set; }
        public List<InvoiceModel> List { get; set; }
        /// <summary>
        /// 箱数
        /// </summary>
        public string Box { get; set; }
        /// <summary>
        /// 合计   数量
        /// </summary>
        public decimal Sum
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Where(s => s.IsFather != 1).Sum(x => x.ClearQty);
                }
                return sum;
            }
        }

        /// <summary>
        /// 合计   数量
        /// </summary>
        public decimal Sum2
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Where(s => s.IsFather != 1).Sum(x => x.ClearQty);
                }
                return sum;
            }
        }

        /// <summary>
        /// 合计  重量
        /// </summary>
        public decimal SumWeight
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => (x.ClearQty * x.NetWeight));
                }
                return sum;
            }
        }
        /// <summary>
        /// 合计   收到数量
        /// </summary>
        public decimal SumAmount2
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Where(s => s.IsFather != 1).Sum(x => (x.ClearQty * x.UnitPrice));
                }
                return sum;
            }
        }
        public decimal SumAmount
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Where(s => s.IsFather != 1).Sum(x => (x.ClearQty * x.UnitPrice));
                }
                return sum;
            }
        }
    }

}
