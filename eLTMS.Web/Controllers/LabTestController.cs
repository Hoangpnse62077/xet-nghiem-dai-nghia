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
using System.Text;
using eLTMS.Models.Enums;
using eLTMS.Web.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace eLTMS.Web.Controllers
{
    public class LabTestController : BaseController
    {
        //private readonly IExportPaperService _exportPaperService;
        private readonly ISampleService _sampleService;
        private readonly ISampleGettingService _sampleGettingService;
        private readonly ISampleGroupService _sampleGroupService;
        private readonly ILabTestService _labTestService;
        private readonly IPatientService _patientService;
        private readonly ILabTestingService _labTestingService;
        private readonly ILabTestingIndexService _labTestingIndexService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHospitalSuggestionService _hospitalSuggestionService;
        private readonly ITokenService _tokenService;
        public LabTestController(ISampleGettingService sampleGettingService,IPatientService patientService, IHospitalSuggestionService hospitalSuggestionService, IAppointmentService appointmentService,ILabTestingIndexService labTestingIndexService, ILabTestingService labTestingService, ILabTestService labTestService, ISampleService sampleService, ISampleGroupService sampleGroupService, ITokenService tokenService)
        {
            this._labTestService = labTestService;
            this._appointmentService = appointmentService;
            this._labTestingService = labTestingService;
            this._labTestingIndexService = labTestingIndexService;
            this._sampleService = sampleService;
            this._sampleGettingService = sampleGettingService;
            this._patientService = patientService;
            this._sampleGroupService = sampleGroupService;
            this._hospitalSuggestionService = hospitalSuggestionService;
            this._tokenService = tokenService;
        }
        
        public ActionResult LabTests()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.LabTechnician))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        public ActionResult LabTesting()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.LabTechnician))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        public ActionResult LabTestingResult()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.LabTechnician, (int)RoleEnum.Receptionist, (int)RoleEnum.Doctor))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        public ActionResult LabTestingFail()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.LabTechnician, (int)RoleEnum.Doctor))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        public ActionResult LabTestingDone()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.LabTechnician, (int)RoleEnum.Doctor))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        public ActionResult SampleFail()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.LabTechnician, (int)RoleEnum.Doctor))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        [HttpGet]
        public JsonResult GetAllSamples( int page = 1, int pageSize = 20)
        {
            var queryResult = _sampleService.GetAll();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllSampleGroups(int page = 1, int pageSize = 20)
        {
            var queryResult = _sampleGroupService.GetAll();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<SampleGroup>, IEnumerable<SampleGroupDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLabTesting(int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllLabTesting();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingHaveAppointmentCode(string code = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllLabTestingHaveAppointmentCode(code);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingIndexHaveLabtestingId(int id)
        {
            var queryResult = _labTestingIndexService.GetAllLabTestingIndexHaveLabtestingId(id);
            var result = Mapper.Map<IEnumerable<LabTestingIndex>, IEnumerable<LabTestingIndexDto>>(queryResult);
            return Json(new
            {
                data = result,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingResult(int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllLabTestingResult();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllResult(string s,int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllResult(s);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingsFail()
        {
            var queryResult = _labTestingService.GetAllLabTestingFail();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult);
            return Json(new
            {
                success = true,
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestings()
        {
            var queryResult = _labTestingService.GetAllLabTesting();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult);
            return Json(new
            {
                success = true,
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingDate(string date)
        {
            var queryResult = _labTestingService.GetAllLabTestingDate(date);
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult);
            return Json(new
            {
                success = true,
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTests(string code = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestService.GetAllLabTests(code);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTest>, IEnumerable<LabTestDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateLabTest(LabTest labTest)
        {
            var result = _labTestService.Update(labTest);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult UpdateLabTestingFail(int id)
        {
            var result = _labTestingService.UpdateFail(id);
            var result1 = _labTestingService.GetLabTesting(id);
            //từ labtesting lấy ra samplegetting
            var s = result1.SampleGettingId; var sampleGetting = _sampleGettingService.GetSampleGetting(int.Parse(s.ToString()));
            //từ samplegetting lấy ra appointment
            var z = sampleGetting.AppointmentId; var appointment = _appointmentService.GetSingleById(int.Parse(z.ToString()));
            //từ samplegetting lấy ra sample
            var m = sampleGetting.SampleId; var sample = _sampleService.GetSampleById(int.Parse(m.ToString()));
            //từ appointment lấy ra patient
            var l = appointment.PatientId; var patient = _patientService.GetPatientById(int.Parse(l.ToString()));
            if (result==true)
            {
                var tokens = _tokenService.GetAll();
                int[] roleIds = {
                    (int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Bệnh nhân: " + patient.FullName + " ĐT:" + patient.PhoneNumber + " Cần làm lại xét nghiệm: " + sample.SampleName,
                };
                SendNotificationUtils.SendNotification(data, tokens);
                /*
                var tokens = _appointmentService.GetAllTokens();
                foreach (var token in tokens)
                {
                    var data = new
                    {
                        to = token.TokenString,
                        data = new
                        {
                            message = "Bệnh nhân: " + patient.FullName + " ĐT:" + patient.PhoneNumber + " Cần làm lại xét nghiệm: " + sample.SampleName,
                        }
                    };
                    try
                    {
                        SendNotificationUtils.SendNotification(data);
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }/**/
            }
            return Json(new
            {
                sucess = result
            });
        }

        [HttpPost]
        public JsonResult UpdateLabTesting(List<LabTesting> labTesting)
        {
            var result = _labTestingService.Update(labTesting);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteAll(List<LabTestingIndex> labTestingIndex)
        {
            var result = _labTestingIndexService.DeleteAll(labTestingIndex);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult UpdateStatus(List<LabTesting> labTesting)
        {
            var result = _labTestingService.UpdateStatus(labTesting);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult UpdateResult(string code,string con,string cmt)
        {
            var result = _appointmentService.Update(code,con,cmt);
            if (result == true)
            {
                var tokens = _tokenService.GetAll();
                int[] roleIds = {
                    (int)RoleEnum.Patient
                };
                var data = new
                {
                    roleIds,
                    message = "Bạn đã có kết quả xét nghiệm."
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult AddLabTest(LabTest labTest)
        {
            var result = _labTestService.AddLabTest(labTest);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult AddLabTestingIndex(List<LabTestingIndex> labTestingIndex)
        {
            var result = _labTestingIndexService.AddLabTestingIndex(labTestingIndex);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult AddSample(Sample sample)
        {
            var result = _sampleService.AddSample(sample);
            return Json(new
            {
                sucess = result
            });
        }

        [HttpPost]
        public JsonResult AddSampleGroup(SampleGroup sample)
        {
            var result = _sampleGroupService.AddSampleGroup(sample);
            return Json(new
            {
                sucess = result
            });
        }

        [HttpGet]
        public JsonResult LabTestDetail(int id)
        {
            var result = _labTestService.GetLabTestById(id);
            var labTest = Mapper.Map<LabTest, LabTestDto>(result);
            return Json(new
            {
                sucess = true,
                data = labTest
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLabTest(int id)
        {
            var result = _labTestService.Delete(id);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteLabTesting(int id)
        {
            var result = _labTestingService.Delete(id);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteLabTestingIndex(int id)
        {
            var result = _labTestingIndexService.Delete(id);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteSample(int id)
        {
            var result = _sampleService.Delete(id);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult DeleteSampleGroup(int id)
        {
            var result = _sampleGroupService.Delete(id);
            return Json(new
            {
                success = result
            });
        }


        [HttpPost]
        public ActionResult ExportDetailToPdf(string code)
        {
            //StringBuilder sb = new StringBuilder();
            //StringBuilder sb1 = new StringBuilder(); StringBuilder sb2 = new StringBuilder();
            //var queryResult2 = _appointmentService.GetResultByAppCode(code);
            //var result2 = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult2);
            //var queryResult1 = _labTestingService.GetAllLabTestingHaveAppointmentCode(code);
            //var result1 = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult1);
            //foreach (var item1 in result1)
            //{
            //    var queryResult = _labTestingIndexService.GetAllLabTestingIndexHaveLabtestingId(item1.LabTestingId);
            //    var result = Mapper.Map<IEnumerable<LabTestingIndex>, IEnumerable<LabTestingIndexDto>>(queryResult);
            //    var changeColor = "";
            //    sb.AppendLine($"<tr><td><h3>{item1.LabTestName}</h3><td></tr>");
            //    foreach (var item in result)
                    
            //    {
            //        if (item.LowNormalHigh.Contains("L")) changeColor = "'background-color: yellow;'";
            //        if (item.LowNormalHigh.Contains("H")) changeColor = "'background-color: #FF6A6A;'";
            //        if (item.LowNormalHigh.Contains("N")) changeColor = "'background-color: white;'";
            //        sb.AppendLine("<tr>");
            //        sb.AppendLine($"<td class='no'>{item.IndexName}</td>");
            //        sb.AppendLine($"<td class='colUnit' style= {changeColor}>{item.IndexValue}</td>");
            //        sb.AppendLine($"<td class='colUnit'style= {changeColor}>{item.LowNormalHigh}</td>");
            //        sb.AppendLine($"<td class='colUnit'style= {changeColor}>{item.NormalRange}</td>");
            //        sb.AppendLine($"<td class='colUnit'style= {changeColor}>{item.Unit}</td>");
            //        sb.AppendLine("</tr>");
            //    }
            //}
            var Renderer = new IronPdf.HtmlToPdf();
            var allData = System.IO.File.ReadAllText(Server.MapPath("~/template-pdf/result.html"));
            //foreach (var item2 in result2)
            //{
            //    allData = allData.Replace("{{InvoiceDate}}", $"{item2.Date}");
            //    sb2.AppendLine($"<tr><td class='no'><strong>Tên: </strong>{item2.PatientName}</td></tr>");
            //    sb2.AppendLine($"<tr><td class='colUnit'><strong>Ngày sinh: </strong>{item2.DateOB.ToString("dd-MM-yyyy")}</td></tr>");
            //    sb2.AppendLine($"<tr><td class='colUnit'><strong>Địa chỉ: </strong>{item2.Address}</td></tr>");
            //    sb2.AppendLine($"<tr><td class='colUnit'><strong>Điện thoại: </strong>{item2.Phone}</td></tr>");
            //    var gender = (item2.Gender == "Male") ? "Nam" : "Nữ";
            //    sb2.AppendLine($"<tr><td class='colUnit'><strong>Giới tính: </strong>{gender}</td></tr>");
            //    allData = allData.Replace("{{Con}}", $"<h2>{item2.Conclusion}</h2>");
            //    allData = allData.Replace("{{Cmt}}", item2.DoctorComment);
            //    string x = item2.Conclusion + "";
            //    var queryResult3 = _hospitalSuggestionService.GetAllHospitalSuggestions(x);
            //    var result3 = Mapper.Map<IEnumerable<HospitalSuggestion>, IEnumerable<HospitalSuggestionDto>>(queryResult3);
            //    foreach (var item3 in result3)
            //    {                 
            //        sb1.AppendLine($"<tr><td class='no'><strong>{item3.HospitalList}</strong></td></tr>");
            //        sb1.AppendLine($"<tr><td class='colUnit'><strong>Địa chỉ: </strong>{item3.HospitalAdd}</td></tr>");
            //        sb1.AppendLine($"<tr><td class='colUnit'><strong>Điện thoại: </strong>{item3.HospitalPhone}</td></tr>");
            //    }               
            //}

            //allData = allData.Replace("{{DataResult}}", sb.ToString());
            //allData = allData.Replace("{{DataResult1}}", sb1.ToString());
            //allData = allData.Replace("{{DataResult2}}", sb2.ToString());
            var PDF = Renderer.RenderHtmlAsPdf(allData);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", $"attachment;filename=\"Result.pdf\"");
            // edit this line to display ion browser and change the file name
            Response.BinaryWrite(PDF.BinaryData);
            Response.Flush();
            Response.End();
            return null;
        }
        [HttpPost]
        public ActionResult ViewDetailOnWeb(string code)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder(); StringBuilder sb2 = new StringBuilder();
            var queryResult2 = _appointmentService.GetResultByAppCode(code);
            var result2 = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult2);
            var queryResult1 = _labTestingService.GetAllLabTestingHaveAppointmentCode(code);
            var result1 = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult1);
            foreach (var item1 in result1)
            {
                var queryResult = _labTestingIndexService.GetAllLabTestingIndexHaveLabtestingId(item1.LabTestingId);
                var result = Mapper.Map<IEnumerable<LabTestingIndex>, IEnumerable<LabTestingIndexDto>>(queryResult);
                var changeColor = "";
                sb.AppendLine($"<tr><td><h3>{item1.LabTestName}</h3><td></tr>");
                foreach (var item in result)

                {
                    if (item.LowNormalHigh.Contains("L")) changeColor = "'background-color: yellow;'";
                    if (item.LowNormalHigh.Contains("H")) changeColor = "'background-color: #FF6A6A;'";
                    if (item.LowNormalHigh.Contains("N")) changeColor = "'background-color: white;'";
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td class='no'>{item.IndexName}</td>");
                    sb.AppendLine($"<td class='colUnit' style= {changeColor}>{item.IndexValue}</td>");
                    sb.AppendLine($"<td class='colUnit'style= {changeColor}>{item.LowNormalHigh}</td>");
                    sb.AppendLine($"<td class='colUnit'style= {changeColor}>{item.NormalRange}</td>");
                    sb.AppendLine($"<td class='colUnit'style= {changeColor}>{item.Unit}</td>");
                    sb.AppendLine("</tr>");
                }
            }
            var Renderer = new IronPdf.HtmlToPdf();
            var allData = System.IO.File.ReadAllText(Server.MapPath("~/template-pdf/resultPrint.html"));
            foreach (var item2 in result2)
            {
                allData = allData.Replace("{{InvoiceDate}}", $"{item2.Date}");
                sb2.AppendLine($"<tr><td class='no'><strong>Tên: </strong>{item2.PatientName}</td></tr>");
                sb2.AppendLine($"<tr><td class='colUnit'><strong>Ngày sinh: </strong>{item2.DateOB.ToString("dd-MM-yyyy")}</td></tr>");
                sb2.AppendLine($"<tr><td class='colUnit'><strong>Địa chỉ: </strong>{item2.Address}</td></tr>");
                sb2.AppendLine($"<tr><td class='colUnit'><strong>Điện thoại: </strong>{item2.Phone}</td></tr>");
                var gender = (item2.Gender == "Male") ? "Nam" : "Nữ";
                sb2.AppendLine($"<tr><td class='colUnit'><strong>Giới tính: </strong>{gender}</td></tr>");
                allData = allData.Replace("{{Con}}", $"<h3>{item2.Conclusion}</h3>");
                allData = allData.Replace("{{Cmt}}", item2.DoctorComment);
                string x = item2.Conclusion + "";
                var queryResult3 = _hospitalSuggestionService.GetAllHospitalSuggestions(x);
                var result3 = Mapper.Map<IEnumerable<HospitalSuggestion>, IEnumerable<HospitalSuggestionDto>>(queryResult3);
                foreach (var item3 in result3)
                {
                    sb1.AppendLine($"<tr><td class='no'><strong>{item3.HospitalList}</strong></td></tr>");
                    sb1.AppendLine($"<tr><td class='colUnit'><strong>Địa chỉ: </strong>{item3.HospitalAdd}</td></tr>");
                    sb1.AppendLine($"<tr><td class='colUnit'><strong>Điện thoại: </strong>{item3.HospitalPhone}</td></tr>");
                }
            }

            allData = allData.Replace("{{DataResult}}", sb.ToString());
            allData = allData.Replace("{{DataResult1}}", sb1.ToString());
            allData = allData.Replace("{{DataResult2}}", sb2.ToString());

            return new ContentResult {
        ContentType = "text/html",
        Content = allData
    };
        }

    }
}