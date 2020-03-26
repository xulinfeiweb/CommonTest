using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TradeApprovalDataModel
    {
        public DataHeader Header { get; set; }
        public List<TradeApprovalModel> List { get; set; }
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
                    sum = List.Sum(x => x.totalQty);
                }
                return sum;
            }
        }

        /// <summary>
        /// 合计   总金额
        /// </summary>
        public decimal SumPrice
        {
            get
            {
                decimal sum = 0;
                if (List != null || List.Count > 0)
                {
                    sum = List.Sum(x => x.Amount);
                }
                return sum;
            }
        }

    }

}
