using eLTMS.BusinessLogic.Services;
using eLTMS.Models;
using eLTMS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove(ConstantManager.SESSION_ACCOUNT);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult CheckLogin(FormCollection fc)
        {
            var phoneNumber = fc["phoneNumber"];
            var password = fc["password"];
            var returnUrl = fc["returnUrl"];

            var account = this._accountService.checkLogin(phoneNumber, password);
            var errorMessage = "";
            if (account != null)
            {
                Session[ConstantManager.SESSION_ACCOUNT] = account;
                if (returnUrl != null && returnUrl != "")
                {
                    return Redirect(returnUrl);
                } else
                {
                    // Redirect Default Page of each Role
                    switch(account.RoleId)
                    {
                        case (int)RoleEnum.Receptionist:
                            return Redirect("/Patient/Patients");
                        case (int)RoleEnum.WarehouseKeeper:
                            return Redirect("/Patient/Patients");
                        case (int)RoleEnum.LabTechnician:
                            return Redirect("/Patient/Patients");
                        case (int)RoleEnum.Doctor:
                            return Redirect("/Patient/Patients");
                        case (int)RoleEnum.Manager:
                            return Redirect("/Patient/Patients");
                        case (int)RoleEnum.Nurse:
                            return Redirect("/Patient/Patients");
                        case (int)RoleEnum.Patient:
                            return Redirect("/Patient/Patients");
                    }
                } // end if has returnUrl
            } // end if has account
            else
            {
                errorMessage = "Sai tên hoặc mật khẩu";
            }

            // Require Login again
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = errorMessage;
            return View("Login");
        }

    }
}