using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BakerySite.Properties;
using System.Web.Mvc;
using BakerySite.Models;

namespace BakerySite.Controllers
{

    public class PurchaseController : Controller
    {
        BakeryEntities db = new BakeryEntities();
        // GET: Puchase
        public ActionResult Index()
        {
            if (Session["personKey"] == null)
            {
                Message m = new Message();
                m.MessageTitle = "Make Purchase";
                m.MessageText = "Must Be logged in to make a purchase";
                return RedirectToAction("Result", m);
            }
            ViewBag.ProductKey = new SelectList(db.Products, "ProductKey", "ProductName", "ProductPrice");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "CustomerKey, Date, FirstName, LastName, Phone,  Email, ProductKey, ProductName, "+
                "PriceCharged, Quantity, Discount, SalesTaxPercent, SalesTax, EatInTax, IsSenior, IsEatIn")]Order o)
        {
            //Populate order for receipt, create Sale and SaleDetail records
            o.Date = DateTime.Now;
            o.CustomerKey = (int)Session["personKey"];
            o.FirstName = (String)Session["personName"];
            o.Phone = (String)Session["personPhone"];
            o.Email = (String)Session["personEmail"];

            //get product price
            decimal priceEach = (from p in db.Products
                                 where p.ProductKey.Equals((int)o.ProductKey)
                                 select p.ProductPrice).FirstOrDefault();

            o.ProductName = (from p in db.Products
                             where p.ProductKey.Equals((int)o.ProductKey)
                             select p.ProductName).FirstOrDefault();

            o.LastName = (from p in db.People
                          where p.PersonKey.Equals(o.CustomerKey)
                          select p.PersonLastName).FirstOrDefault();

            /*Calculate order amounts where
             * Price = Product price each
             * PriceCharged = Total order price, before any discounts or additions
             * Discount = Total amount of discounts applied
             * EatInTax = total amount of premium added to PriceCharged for Dine-In privilege
             * SalesTax = Total amount of sales tax
             * SalesTaxPercent = Applicable Sales Tax rate (if area code 206 salesTax = 105 else salesTax = 0%)
             * Total = Final receipt total, after all discounts or additions
            */


            o.Price = priceEach;
            o.PriceCharged = o.Quantity * priceEach ;

            // give 15% discount if Senior
            if (o.IsSenior)
            {
                o.Discount = o.Quantity * priceEach * Settings.Default.SeniorDiscountPercent;
            }

            // charge 25% premium for Dine-In orders
            if (o.IsEatIn)
            {
                o.EatInTax = (o.PriceCharged - o.Discount) * Settings.Default.EatInPremium;
            }

            // charge 10% sales tax if phone starts with 206
            if (o.Phone.Substring(0, 3) == Settings.Default.SalesTaxArea)
            {
                o.SalesTaxPercent = Settings.Default.SalesTax206;
                o.SalesTax = (o.PriceCharged + o.EatInTax - o.Discount) * o.SalesTaxPercent;
            }
            else
            {
                o.SalesTaxPercent = (decimal)0.0;
                o.SalesTax = (decimal)0.0;
            }

            o.Total = o.PriceCharged + o.EatInTax - o.Discount + o.SalesTax;

            //create, populate and add new Sale record
            Sale s = new Sale()
            {
                SaleDate = o.Date,
                CustomerKey = o.CustomerKey
            };
            db.Sales.Add(s);

            //create, populate add new SaleDetail record
            SaleDetail sd = new SaleDetail()
            {
                Sale = s,
                ProductKey = o.ProductKey,
                SaleDetailQuantity = o.Quantity,
                SaleDetailPriceCharged = o.PriceCharged,
                SaleDetailDiscount = o.Discount,
                SaleDetailSaleTaxPercent = o.SalesTaxPercent,
                SaleDetailEatInTax = o.EatInTax
            };
            db.SaleDetails.Add(sd);

            //update database
            db.SaveChanges();

            //use Result for now, TODO add receipt page
            Message m = new Message();
            m.MessageTitle = "Purchase Checkout";
            m.MessageText = "Thank you, your purchase was successful and we will begin preparing your order.";
            return RedirectToAction("Receipt", o);
        }

        public ActionResult Result(Message m)
        {
            return View(m);
        }

        public ActionResult Receipt(Order o)
        {
            return View(o);
        }

    }
}