using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySite.Models;

namespace BakerySite.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "PersonEmail")]Person lp)
        {
            BakeryEntities be = new BakeryEntities();

            var loginKey = (from p in be.People
                       where p.PersonEmail.Equals(lp.PersonEmail)
                       select p.PersonKey).FirstOrDefault();
            
            if (loginKey == 0)
            {
                Message bad = new Message();
                bad.MessageTitle = "Login";
                bad.MessageText = "Sorry, you need to register before you can login.";
                return RedirectToAction("Result", bad);

            }

            // store user personKey in Session
            int pKey = (int)loginKey;
            Session["personKey"] = pKey;

            //get username
            String first = (from p in be.People
                            where p.PersonKey.Equals(pKey)
                            select p.PersonFirstName).FirstOrDefault();
            Session["personName"] = first;

            Message m = new Message();
            m.MessageTitle = "Login";
            m.MessageText = "Thank you, " + first + " your login was successful.";
            return RedirectToAction("Result", m);
        }

        public ActionResult Result(Message m)
        {
            return View(m);
        }
    }
}