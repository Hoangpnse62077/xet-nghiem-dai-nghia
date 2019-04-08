using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class LabTestResultController : Controller
    {
        private readonly ILabTestResultService _labTestResultService;
        private readonly IPatientService _patientService;
        public LabTestResultController(ILabTestResultService labTestResultService, IPatientService patientService)
        {
            this._labTestResultService = labTestResultService;
            this._patientService = patientService;
        }
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var account = Session[ConstantManager.SESSION_ACCOUNT];
            if (account == null)
            {
                var returnUrl = Request.Url.AbsoluteUri;
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var result = new List<Patient>();
            var data = _patientService.GetAllPatientByName("")
                .Select(x => new
            PatientDto(){
               PatientId =  x.PatientId,
                FullName = x.FullName,
                HomeAddress = x.HomeAddress,
            }).ToList();
            TempData["allPatients"] = data;
            TempData["allLabTestTypes"] = GetLabTestTypes();
            if (Session["PatientIdRef"] != null)
            {
                TempData["patientIdRef"] = int.Parse(Session["PatientIdRef"].ToString());
                Session["PatientIdRef"] = null;
                
            }
            return View();
        }
        /// <summary>
        /// Gets all lab test.
        /// </summary>
        /// <param name="searchDto">The search dto.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllLabTest(LabTestResultSearchDto searchDto)
        {
            var result = _labTestResultService.GetAllLabTestResult(searchDto);
            
            return Json(new
            {
                success = true,
                data = result,
                total = result[0].TotalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the type of all lab test.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllLabTestType()
        {
            var result = this._labTestResultService.GetLabTestTypes();
            var tempList = new List<LabTestType>();
            foreach (var item in result)
            {
                foreach (var a in item.LabTestDetails)
                {
                    a.LabTestType = null;
                }
            }
            return Json(new
            {
                success = true,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }
        private List<LabTestType> GetLabTestTypes()
        {
            var result = this._labTestResultService.GetLabTestTypes();
            var tempList = new List<LabTestType>();
            return result;
        }
        /// <summary>
        /// Adds the lab test result.
        /// </summary>
        /// <param name="labTestResult">The lab test result.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddLabTestResult(LabTestResult labTestResult)
        {
            try
            {
                var result = this._labTestResultService.AddLabTestResult(labTestResult);
                if (result != null)
                {
                    return Json(new
                    {
                        success = true,
                        labTestResultId = result.LabTestResultId
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Đã xảy ra lỗi trong quá trình tạo mới"
                    });
                }
                
                    
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = true,
                    message = ex.Message
                });

            }

        }

        /// <summary>
        /// Gets the lab test result detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetLabTestResultDetail(int id)
        {
            var result = _labTestResultService.GetLabTestResultById(id);
            //result.LabTestResultDetails.ToList().ForEach(x =>
            //{
            //    x.LabTestResult = null;
            //    x.LabTestDetail = null;
            //});
            var data = new
            {
                LabTestResultId = result.LabTestResultId,
                PatientId = result.PatientId,
                //PatientCode = result.Patient.PatientCode,
                Comment = result.Comment,
                //HomeAddress = result.Patient.HomeAddress,
                //PhoneNumber = result.Patient.PhoneNumber,
                //Gender = result.Patient.Gender,
                // DateOfBirth = result.Patient.DateOfBirth.Value.
                LabTestResultDetails = result.LabTestResultDetails.Select(x => new {
                    x.LabTestDetailId,x.LabTestResultId,
                    Value = (x.Value != null ? x.Value : "") ,
                    IsCombobox = x.LabTestDetail.AverageValue?.Contains("Âm tính")
                }).ToList(),
                CreatedDate = result.CreatedDate.Value.ToString("dd-MM-yyyy HH:mm")
            };
            return Json(new
            {
                success = true,
                data = data,
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Updates the lab test result.
        /// </summary>
        /// <param name="labTestResult">The lab test result.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateLabTestResult(LabTestResult labTestResult)
        {
            try
            {
                var result = this._labTestResultService.UpdateLabTestResult(labTestResult);
                return Json(new
                {
                    success = true
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = true,
                    message = ex.Message
                });

            }

        }///
        public ActionResult ExportLabTestResultToPdf(int labTestResultId, int labTestTypeId)
        {
            var allData = string.Empty;
            if (labTestTypeId != 4)
            {
                allData =  System.IO.File.ReadAllText(Server.MapPath("~/template-pdf/result.html"));
            }
            else
            {
                allData = System.IO.File.ReadAllText(Server.MapPath("~/template-pdf/result-other.html"));
            }
             
            var labTestResult = this._labTestResultService.GetLabTestResultById(labTestResultId);
            var patientName = labTestResult.Patient.FullName;
            var homeAddress = labTestResult.Patient.HomeAddress;
            var createdDate = labTestResult.CreatedDate.Value.ToString("dd/MM/yyyy");
            var gender = labTestResult.Patient.Gender;
            allData = allData.Replace("{{PatientName}}", patientName.ToUpper());
            allData = allData.Replace("{{HomeAddress}}", homeAddress);
            allData = allData.Replace("{{Gender}}", gender == "Male" ? "Nam" : "Nữ");
            allData = allData.Replace("{{CreatedDate}}", createdDate);
            
            allData = allData.Replace(" {{Age}}", " " + labTestResult.Patient.Age);
            StringBuilder sb = new StringBuilder();
            //if (labTestTypeId == 1)
            //{
            //    allData = allData.Replace("{{LabTestType}}", "XÉT NGHIỆM HUYẾT HỌC");
            //}
            //else  if (labTestTypeId == 2)
            //{
            //    allData = allData.Replace("{{LabTestType}}", "XÉT NGHIỆM NƯỚC TIỂU");
            //}
            //else
            //{
            //    allData = allData.Replace("{{LabTestType}}", "XÉT NGHIỆM SINH HÓA");
            //}

            allData = allData.Replace("{{LabTestType}}", "XÉT NGHIỆM");
            if (labTestTypeId != 4)
            {
                foreach (var item in labTestResult.LabTestResultDetails.Where(x => x.LabTestDetail.LabTestTypeId == labTestTypeId))
                {
                    sb.AppendLine("<tr class='item' style='font-size:16px'>");
                    sb.AppendLine($"<td align='right' style='font-size:16px'>{item.LabTestDetail.Name}</td>");
                    sb.AppendLine($"<td align='center' style='font-size:16px'>{item.Value}</td>");
                    sb.AppendLine($"<td  align='left' style='font-size:16px'>{item.LabTestDetail.AverageValue + " " + item.LabTestDetail.Unit }</td>");
                    sb.AppendLine("</tr>");
                }
                allData = allData.Replace(" {{LabTestDetails}}", sb.ToString());
            }
            else
            {;
                var comment = labTestResult.LabTestResultDetails.FirstOrDefault(x => x.LabTestDetail.LabTestTypeId == labTestTypeId);
                allData = allData.Replace("{{LabTestDetails}}", comment?.Value);
            }
           
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(allData, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", $"attachment;filename=\"KET_QUA_XET_NGHIEM_{patientName}.pdf\"");
            
            Response.BinaryWrite(res);
            Response.Flush();
            Response.End();
            return null;
        }
        public ActionResult Test(string id)
        {
            var Renderer = new IronPdf.HtmlToPdf();
            var allData = System.IO.File.ReadAllText(Server.MapPath("~/template-pdf/result.html"));
            
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
        public JsonResult SaveRefPatientId(int patienId)
        {
            Session["PatientIdRef"] = patienId;
            return Json(new
            {
                success = true,
            });
        }
        
        [HttpPost]
        public JsonResult DeleteLabTest(int id)
        {
            var result = _labTestResultService.DeleteLabTestResult(id);
            return Json(new
            {
                success = result,
            });
        }


    }
}