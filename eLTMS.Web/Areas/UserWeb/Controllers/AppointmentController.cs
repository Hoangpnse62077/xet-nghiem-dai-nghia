using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        public AppointmentController(IAppointmentService appointmentService, IPatientService patientService)
        {
            this._appointmentService = appointmentService;
            this._patientService = patientService;
        }

        // GET: UserWeb/Appointment
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserWeb/Appointment/Create
        public ActionResult Create()
        {
            return View("Create", "_Layout");
        }

        // GET: UserWeb/Appointment/ViewAppointments
        public ActionResult ViewAppointments()
        {
            if (base.ValidRole((int)RoleEnum.Patient))
            {
                return View("ViewAppointments", "_Layout");
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        [HttpGet]
        public JsonResult GetAppointmentsByAccountId(int accountId, int page = 1, int pageSize = 20,
            bool sttNew = true, bool sttProcess = true, bool sttDone = true)
        {
            var queryResult = _appointmentService.GetAppointmentsByAccountId(accountId); 
            var totalRows = queryResult.Count();
            if (sttNew == false)
            {
                queryResult = queryResult.Where(x => !(x.Status == "NEW")).ToList();
            }
            if (sttProcess == false)
            {
                queryResult = queryResult.Where(x => !(x.Status != "NEW" && x.Status != "DOCTORDONE")).ToList();
            }
            if (sttDone == false)
            {
                queryResult = queryResult.Where(x => !(x.Status == "DOCTORDONE")).ToList();
            }
            var result = queryResult.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                success = true,
                data = result,
                //patient = patient,
                //patientDob = (patient.DateOfBirth!=null)?patient.DateOfBirth.Value.ToString("dd-MM-yyyy"):"",
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AppDetail(string app)
        {
            var result = _appointmentService.GetSingleByCode(app);
            var appointment = Mapper.Map<Appointment, AppointmentDto>(result);
            return Json(new
            {
                sucess = true,
                data = appointment
            }, JsonRequestBehavior.AllowGet);
        }
        // GET: UserWeb/Appointment/Edit/{apId}
        public ActionResult Edit(int appointmentId)
        {
            Appointment app = _appointmentService.GetSingleById(appointmentId);
            AppointmentDto appDto = Mapper.Map<Appointment, AppointmentDto>(app);
            ViewBag.appDto = appDto;
            return View("Edit", "_Layout");
        }

        // GET: UserWeb/Appointment/Result/{apId}
        public ActionResult Result(int appointmentId)
        {
            Appointment app = _appointmentService.GetResultDoneByAppointmentId(appointmentId);
            ResultOfAppointmentDto dto = Mapper.Map<Appointment, ResultOfAppointmentDto>(app);
            ViewBag.ResultDto = dto;
            return View("Result", "_Layout");
        }

        public ActionResult Suggest()
        {
            return View("Suggest", "_Layout");
        }

    } 
}