using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class ResultController : Controller
    {
        // GET: Result
        
        public ResultController()
        {
            
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}