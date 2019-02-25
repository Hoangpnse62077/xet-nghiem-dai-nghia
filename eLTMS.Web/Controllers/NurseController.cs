using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models.dto;
using eLTMS.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class NurseController : BaseController
    {
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            this._nurseService = nurseService;
        }

        public ActionResult Index()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.Nurse))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        

        [HttpGet]
        public JsonResult GetAppBySample(string search, DateTime date, int sampleId, int page=1, int pageSize=20)
        {
            //var queryResult = _receptionistService.GetAppBySample(sampleId);
            var result = _nurseService.GetAllBySample(search, date, sampleId).Skip((page - 1) * pageSize).Take(pageSize);
            var totalRows = result.Count();
            //var totalRows = queryResult.Count();
            //var result = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsGot(int sampleGettingId)
        {
            var result = _nurseService.ChangeIsGot(sampleGettingId);
            if (result == true)
            {
                var tokens = _nurseService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Nurse,
                    (int)RoleEnum.Cashier,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Hoàn tất lấy mẫu"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }
        
        
    }
}