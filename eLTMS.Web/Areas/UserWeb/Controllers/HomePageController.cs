using eLTMS.BusinessLogic.Services;
using eLTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class HomePageController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index", "_Layout");
        }

        // GET: /UserWeb/About
        public ActionResult About()
        {
            return View("About", "_Layout");
        }

        // GET: /UserWeb/Contact
        public ActionResult Contact()
        {
            return View("Contact", "_Layout");
        }

        // GET: /UserWeb/Gallery
        public ActionResult Gallery()
        {
            return View("Gallery", "_Layout");
        }

        // GET: /UserWeb/FAQ
        public ActionResult FAQ()
        {
            return View("FAQ", "_Layout");
        }

        // GET: /UserWeb/Service
        public ActionResult Service()
        {
            return View("Service", "_Layout");
        }

        // GET: /UserWeb/ServiceDetails
        public ActionResult ServiceDetails()
        {
            return View("ServiceDetails", "_Layout");
        }

    }
}