using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eLTMS.Models.Enums;

namespace eLTMS.Web.Controllers
{
    public class EmployeeController : BaseController
    {
        //Nguyen Huu Lam
        // GET: Employee
        //Khai báo IEmployeeService
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }

        public ActionResult Employees()
        {
            if (base.ValidRole((int)RoleEnum.Manager))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        //Tạo page cho View Employee-lay tat ca employee show tren bang
        [HttpGet]
        public JsonResult GetAllEmployees(String fullName = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _employeeService.GetAllEmployees(fullName);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

   
        [HttpPost]
        public JsonResult UpdateEmployee(Employee employee)
        {
            var result = _employeeService.Update(employee);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpGet]
        public JsonResult EmployeeDetail(int id)
        {
            var result = _employeeService.getEmployeeById(id);
            var supply = Mapper.Map<Employee, EmployeeDto>(result);
            return Json(new
            {
                sucess = true,
                data = supply
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddEmployee(Employee employee)
        {
            var result = _employeeService.AddEmployee(employee);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult DeleteEmployee(int employeId)
        {
            var result = _employeeService.DeleteEmployee(employeId);
            return Json(new
            {
                success = result
            });
        }
    }
}