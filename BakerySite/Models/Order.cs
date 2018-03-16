using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BakerySite.Models
{
    public class Order
    {
        public int CustomerKey { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ProductKey { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceCharged { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal SalesTaxPercent { get; set; }
        public decimal SalesTax { get; set; }
        public decimal EatInTax { get; set; }
        public Boolean IsSenior { get; set; }
        public Boolean IsEatIn { get; set; }
    }
}