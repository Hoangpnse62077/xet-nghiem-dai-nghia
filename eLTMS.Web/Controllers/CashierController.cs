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
    public class CashierController : BaseController
    {
        private readonly ICashiertService _cashierService;

        public CashierController(ICashiertService cashierService)
        {
            this._cashierService = cashierService;
            //this._importPaperService = importPaperService;
        }
        public ActionResult Index()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.Cashier))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        
       

        [HttpGet]
        public JsonResult GetAppBySample(string search, DateTime date, int sampleId, int page=1, int pageSize=20)
        {
            var result = _cashierService.GetAllBySample(search, date, sampleId)/*.Skip((page - 1) * pageSize).Take(pageSize)*/;
            var totalRows = result.Count();
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddApp(AppointmentAddDto data, List<LabTestDto> labTests)
        {
            var result = _cashierService.Add(data, labTests);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckAndDeleteBlood(DateTime dateTime)
       {
            var result = _cashierService.CheckAndDeleteBlood(dateTime);
            if (result > 0)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Receptionist,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Xóa cuộc hẹn xét nghiêm máu"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckNDeleteUrine(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeleteUrine(dateTime);
            if (result > 0)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Receptionist,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Xóa cuộc hẹn xét nghiệm nước tiểu"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckNDeleteCell(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeleteCell(dateTime);
            if (result > 0)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Receptionist,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Xóa cuộc hẹn xét nghiệm tế bào"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckNDeleteMucus(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeleteMucus(dateTime);
            if (result > 0)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Receptionist,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Xóa cuộc hẹn xét nghiệm dịch"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckNDeletePhan(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeletePhan(dateTime);
            if (result > 0)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Receptionist,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Xóa cuộc hẹn xét nghiệm phân"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult IsPaid(int sampleGettingId)
        {
            var result = _cashierService.ChangeIsPaid(sampleGettingId);
            if (result == true)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                int[] roleIds = {
                    //(int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Nurse,
                    //(int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Thanh toán hoàn tất"
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetPriceByPatient(int sampleGettingId, int page = 1, int pageSize = 20)
        {
            var result = _cashierService.GetPrice(sampleGettingId);
            var totalRows = result.TotalPrice;
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

    }
}