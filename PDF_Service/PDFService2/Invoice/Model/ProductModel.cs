using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ProductModel
    {
        /// <summary>
        /// ProductCode
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// VendorName
        /// </summary>
        public string VendorName { get; set; }
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// ProductDescrEN
        /// </summary>
        public string ProductDescrEN { get; set; }
        /// <summary>
        /// UnitPrice
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// QuantityUnitEN
        /// </summary>
        public string QuantityUnitEN { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// CurrencyEN
        /// </summary>
        public string CurrencyEN { get; set; }
        /// <summary>
        /// MadeInEN
        /// </summary>
        public string MadeInEN { get; set; }
        /// <summary>
        /// HSCode
        /// </summary>
        public string HSCode { get; set; }
    }
}
