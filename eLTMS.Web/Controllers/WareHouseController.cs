using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models.dto;
using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class WareHouseController : BaseController
    {
        // GET: WareHouse
        private readonly ISupplyService _supplyService;

        private readonly IExportPaperService _exportPaperService;
        private readonly IImportPaperService _importPaperService;
        public WareHouseController(ISupplyService supplyService, IImportPaperService importPaperService, IExportPaperService exportPaperService)
        {
            this._supplyService = supplyService;
            this._importPaperService = importPaperService;
            this._exportPaperService = exportPaperService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Supplies()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.WarehouseKeeper))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
      
        public ActionResult Inventory()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.WarehouseKeeper))
            {
                var supplier = _exportPaperService.GetAllExportPapers("").LastOrDefault();
                if (supplier != null)
                {
                    ViewBag.PKK = "PKK" + (supplier.ExportPaperId + 1);
                }
                else
                {
                    ViewBag.PKK = "PKK1";
                }
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });            
        }

        public ActionResult ImportSupplies()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.WarehouseKeeper))
            {
                var supplier = _importPaperService.GetAllImportPapers("").LastOrDefault();
                if (supplier != null)
                {
                    ViewBag.PNK = "PNK" + (supplier.ImportPaperId + 1);
                }
                else
                {
                    ViewBag.PNK = "PNK1";
                }
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });            
        }

        public ActionResult ExportSupplies()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.WarehouseKeeper))
            {
                var supplier = _exportPaperService.GetAllExportPapers("").LastOrDefault();
                if (supplier != null)
                {
                    ViewBag.PXK = "PXK" + (supplier.ExportPaperId + 1);
                }
                else
                {
                    ViewBag.PXK = "PXK1";
                }
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });            
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllSupplies(string suppliesCode = "",int page = 1,int pageSize = 20)
        {
            var queryResult = _supplyService.GetAllSupplies(suppliesCode);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Supply>, IEnumerable<SupplyDto>>(queryResult.Skip((page-1) * pageSize).Take(pageSize)) ;
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllImportPapers(string createDate = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _importPaperService.GetAllImportPapers(createDate);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<ImportPaper>, IEnumerable<ImportPaperDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadPaperImportDetailId(int id)
        {
                     var queryResult = _importPaperService.GetImportPaperById(id);
            var importPaper = Mapper.Map<ImportPaper, ImportPaperDto>(queryResult);
            return Json(new
            {
                success = true,
                data = importPaper,
                            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddSupply(Supply supply)
        {
            var result =  _supplyService.AddSupply(supply);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpGet]
        public JsonResult SupplyDetail(int id)
        {
            var result = _supplyService.GetSupplyById(id);
            var supply = Mapper.Map<Supply,SupplyDto>(result);
            return Json(new
            {
                sucess = true,
                data = supply
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateSupply(Supply supply)
        {
            var result = _supplyService.Update(supply.SuppliesId, supply.SuppliesCode, supply.SuppliesName,supply.SuppliesTypeId.Value,supply.Unit,supply.Note);
            return Json(new
            {
                sucess = result
            });
        }

        [HttpGet]
        public JsonResult GetAllSupply()

        {
            var queryResult = _supplyService.GetAllSupplies(string.Empty);

            var result = queryResult.Select(x => new { x.SuppliesId, x.SuppliesName, x.SuppliesCode , x.Unit, x.Quantity}).ToList();
            return Json(new
            {
                success = true,
                data = result, 
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddImportPaper(ImportPaper importPaper)
        {
            var result = _importPaperService.AddImportPaper(importPaper);
            return Json(new
            {
                success = true,
                data = result,
            });
        }

        [HttpPost]
        public JsonResult AddExportPaper(ExportPaper exportPaper)
        {
            var result = _exportPaperService.AddExportPaper(exportPaper);
            return Json(new
            {
                success = true,
                data = result,
            });
        }

        [HttpGet]
        public JsonResult GetAllExportPapers(string createDate = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _exportPaperService.GetAllExportPapers(createDate);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<ExportPaper>, IEnumerable<ExportPaperDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllInventorys(string createDate = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _exportPaperService.GetAllInventorys(createDate);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<ExportPaper>, IEnumerable<ExportPaperDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadPaperExportDetailId(int id)
        {
            var queryResult = _exportPaperService.GetExportPaperById(id);
            var exportPaper = Mapper.Map<ExportPaper, ExportPaperDto>(queryResult);
            return Json(new
            {
                success = true,
                data = exportPaper,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int supplyId)
        {
            var result = _supplyService.Delete(supplyId);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteImportPaper(int importId)
        {
            var result = _importPaperService.Delete(importId);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteExportPaper(int importId)
        {
            var result = _exportPaperService.Delete(importId);
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public FileResult DownloadImportPaperTemplate()
        { 
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/templates/ImportPaperTemplate.xlsx"));
            string fileName = "ImportPaperTemplate.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public FileResult GetExportFile()
        {
            var allSupply = _supplyService.GetAllSupplies(string.Empty);
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("Danh sách vật tư");
            
            DataTable dt = new DataTable();

            dt.Columns.Add("STT", typeof(int));        
            dt.Columns.Add("Mã vật tư", typeof(string));
            dt.Columns.Add("Tên vật tư", typeof(string));
            dt.Columns.Add("Số lượng ", typeof(int));
            dt.Columns.Add("Số lượng thực tế ", typeof(int));
            dt.Columns.Add("Chênh lệch ", typeof(int));
            dt.Columns.Add("Ghi chú", typeof(string));
            ws.Columns[0].SetWidth(30, LengthUnit.Pixel);
            ws.Columns[1].SetWidth(100, LengthUnit.Pixel);
            ws.Columns[2].SetWidth(250, LengthUnit.Pixel);
            ws.Columns[3].SetWidth(100, LengthUnit.Pixel);
            ws.Columns[4].SetWidth(150, LengthUnit.Pixel);
            ws.Columns[5].SetWidth(100, LengthUnit.Pixel);
            ws.Columns[6].SetWidth(200, LengthUnit.Pixel);
            for (int i = 0; i < allSupply.Count; i++)
            {
                var item = allSupply[i];
                dt.Rows.Add(new object[] { i+1, item.SuppliesCode, item.SuppliesName, item.Quantity});

            }           
            ws.Cells[0, 0].Value = "Danh sách vật tư trong kho";

            // Insert DataTable into an Excel worksheet.
            ws.InsertDataTable(dt,
                new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                    StartRow = 2
                });
            for (int i = 0; i < allSupply.Count; i++)
            {
                ws.Cells["F" + ( i + 4)+""].Formula = $"=(D{i+4}-E{i+4})";
                
                ws.Cells["E" + (i + 4) + ""].Style.Locked = false;
                ws.Cells["G" + (i + 4) + ""].Style.Locked = false;
                ws.Cells["F" + (i + 4) + ""].Calculate();
            }

           

            ws.Protected = true;
            var protectionSettings = ws.ProtectionSettings;
            protectionSettings.AllowInsertingColumns = true;
            protectionSettings.SetPassword("123456");

            ws.Calculate();
            ws.Parent.Calculate();
            return File(GetBytes(ef, SaveOptions.XlsxDefault), SaveOptions.XlsxDefault.ContentType);
        }

        private static byte[] GetBytes(ExcelFile file, SaveOptions options)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                file.Save(stream, options);
                return stream.ToArray();
            }
        }
    }
}