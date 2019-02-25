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
    public class SampleApiController: ApiController
    {
        private readonly ISampleService _sampleService;
        public SampleApiController(ISampleService sampleService)
        {
            this._sampleService = sampleService;
        }

        [HttpGet]
        [Route("api/sample/get-all")]
        public HttpResponseMessage GetAllSampleDtos()
        {
            var sampleDtos = _sampleService.GetAllSampleDtos();
            var response = Request.CreateResponse(HttpStatusCode.OK, sampleDtos);
            return response;
        }
        
        [HttpGet]
        [Route("api/sample/getSampleById")]
        public HttpResponseMessage GetSampleById(int id)
        {
            var sample = _sampleService.GetSampleById(id);
            var sampleDtos = Mapper.Map<SampleDto>(sample);//chuyen data sang dto
            var response = Request.CreateResponse(HttpStatusCode.OK, sampleDtos);
            return response;
        }
    }
}