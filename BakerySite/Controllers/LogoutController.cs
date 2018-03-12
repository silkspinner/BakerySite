using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySite.Models;

namespace BakerySite.Controllers
{
    public class LogoutController : Controller
    {
        // Double check logout
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "")]Person p)
        {

            //get username
            String first = (String)Session["personName"];
            Session.RemoveAll();
            Session.Clear();

            Message m = new Message();
            m.MessageTitle = "Logout";
            m.MessageText = "Thank you, " + first + " for visiting our site today.";
            return RedirectToAction("Result", m);
        }

        public ActionResult Result(Message m)
        {
            return View(m);
        }
    }
}