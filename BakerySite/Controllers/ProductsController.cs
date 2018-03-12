using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySite.Models;

namespace BakerySite.Controllers
{
    public class ProductsController : Controller
    {
        private BakeryEntities db = new BakeryEntities();
        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
    }
}