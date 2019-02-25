using eLTMS.DataAccess.Models;
using eLTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected bool ValidRole(params int[] args)
        {
            if (args == null)
            {
                return false;
            }
            var account = Session[ConstantManager.SESSION_PATIENT_ACCOUNT];
            if (account != null)
            {
                var roleId = ((Account)account).RoleId;
                if (args.Contains((int)roleId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}