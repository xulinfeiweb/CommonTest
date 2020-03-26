using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class InvoiceModel
    {
        public string rowindex { get; set; }
        public int RecordClearHeadID { get; set; }
        /// <summary>
        /// LineIndex
        /// </summary>
        public int LineIndex { get; set; }

        /// <summary>
        /// ProductCode
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// ProductDescrEN
        /// </summary>
        public string ProductDescrEN { get; set; }
        /// <summary>
        /// H2000Index
        /// </summary>
        public string H2000Index { get; set; }
        /// <summary>
        /// HSCode
        /// </summary>
        public string HSCode { get; set; }
        /// <summary>
        /// ClearQty
        /// </summary>
        public decimal ClearQty { get; set; }
        /// <summary>
        /// Unit Price
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// CurrencyEN
        /// </summary>
        public string CurrencyEN { get; set; }
        /// <summary>
        /// QuantityUnitEN
        /// </summary>
        public string QuantityUnitEN { get; set; }
        /// <summary>
        /// LegalClearQty
        /// </summary>
        public decimal LegalClearQty { get; set; }
        /// <summary>
        /// LegalUnitEN
        /// </summary>
        public string LegalUnitEN { get; set; }
        /// <summary>
        /// MadeInEN
        /// </summary>
        public string MadeInEN { get; set; }
        /// <summary>
        /// Net Weight
        /// </summary>
        public decimal NetWeight { get; set; }
        /// <summary>
        /// WeightUnitEN
        /// </summary>
        public string WeightUnitEN { get; set; }
        /// <summary>
        /// TotalNet
        /// </summary>
        public decimal TotalNet { get; set; }

        /// <summary>
        /// Allocate
        /// </summary>
        public decimal Allocate { get; set; }
        /// <summary>
        /// PO
        /// </summary>
        public string PO { get; set; }
        /// <summary>
        /// DN
        /// </summary>
        public string DN { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// SerialNumber
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// POTypeName
        /// </summary>
        public string POTypeName { get; set; }

        public decimal Amount { set; get; }

        /// <summary>
        /// 是否母备件 1：是，0：否，-1：表示：该母备件下最后一个子备件
        /// </summary>
        public int IsFather { set; get; }
    }
}
