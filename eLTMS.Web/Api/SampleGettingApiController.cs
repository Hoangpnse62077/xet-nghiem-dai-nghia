using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class SampleGettingApiController : ApiController
    {
        private readonly ISampleGettingService _sampleGettingService;
        public SampleGettingApiController(ISampleGettingService sampleGettingService)
        {
            this._sampleGettingService = sampleGettingService;
        }

        [HttpGet]
        [Route("api/sample-getting/get-by-code-for-nurse")]
        public HttpResponseMessage GetByCodeForNurse(string code)
        {
            code = code.Trim();
            var result = this._sampleGettingService.GetByCodeForNurse(code);
            var dtos = Mapper.Map<SampleGetting, SampleGettingForNurseDto>(result);
            var response = Request.CreateResponse(HttpStatusCode.OK, dtos);
            return response;
        }

        [HttpPut]
        [Route("api/sample-getting/update-status-nurse-done")]
        public HttpResponseMessage UpdateStatusNurseDone(int sampleGettingId)
        {
            var success = this._sampleGettingService.UpdateStatusNurseDone(sampleGettingId);
            var obj = new
            {
                Success = success,
                Message = "Cập nhật thành công"
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }
    }
}