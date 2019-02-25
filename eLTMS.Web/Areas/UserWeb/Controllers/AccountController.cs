using eLTMS.BusinessLogic.Services;
using eLTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        // GET: /UserWeb/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login", "_Layout");
        }

        // GET: /UserWeb/Logout
        public ActionResult Logout()
        {
            Session.Remove(ConstantManager.SESSION_PATIENT_ACCOUNT);
            return RedirectToAction("", "HomePage");
        }

        // GET: /UserWeb/Register
        public ActionResult Register()
        {
            return View("Register", "_Layout");
        }

        //[ValidateAntiForgeryToken]
        //public ActionResult RegisterPatient(FormCollection fc)
        //{
        //    var respObj = this._accountService.RegisterPatient();
        //    return View("Register", "_Layout");
        //}

        [HttpPost]
        public ActionResult CheckLoginPatient(FormCollection fc)
        {
            var phoneNumber = fc["phoneNumber"];
            var password = fc["password"];
            var returnUrl = fc["returnUrl"];

            var respObj = this._accountService.CheckLoginPatientForWeb(phoneNumber, password);
            var errorMessage = "";
            if (respObj.Success)
            {
                Session[ConstantManager.SESSION_PATIENT_ACCOUNT] = respObj.Data;
                if (returnUrl != null && returnUrl != "")
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "HomePage");
                } // end if has returnUrl
            } // end if has account
            else
            {
                errorMessage = "Số điện thoại hoặc mật khẩu không đúng.";
            }

            // Require Login again
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = errorMessage;
            return View("Login", "_Layout");
        }
    }
}