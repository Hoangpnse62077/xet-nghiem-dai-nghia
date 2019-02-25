using eLTMS.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class ExportPDFController : BaseController
    {
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConvertHTMLToPDF(string html)
        {
           
            var Renderer = new IronPdf.HtmlToPdf();
           
            var PDF = Renderer.RenderHtmlAsPdf(html);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", $"attachment;filename=\"Result.pdf\"");
            // edit this line to display ion browser and change the file name
            Response.BinaryWrite(PDF.BinaryData);
            Response.Flush();
            Response.End();
            return null;
        }
    }
}
