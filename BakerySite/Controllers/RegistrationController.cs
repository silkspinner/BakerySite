using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySite.Models;


namespace BakerySite.Controllers
{
    public class RegistrationController : Controller
    {
        BakeryEntities be = new BakeryEntities();
        
        // GET: Registration
        public ActionResult Index()
        {
            if (Session["personKey"] != null)
            {
                Message m = new Message();
                m.MessageTitle = "Registration";
                m.MessageText = "You are already registered.";
                return RedirectToAction("Result", m);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "PersonLastName, PersonFirstName, PersonEmail, PersonPhone, PersonDateAdded")]Person np)
        {
            np.PersonDateAdded = DateTime.Now;
            be.People.Add(np);
            be.SaveChanges();

            Message m = new Message();
            m.MessageTitle = "Registration";
            m.MessageText = "Thank you, your registration is successful.";
            return RedirectToAction("Result", m);
        }

        public ActionResult Result(Message m)
        {
            return View(m);
        }
    }
}