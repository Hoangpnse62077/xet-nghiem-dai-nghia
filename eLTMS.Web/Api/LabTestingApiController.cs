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
    public class LabTestingApiController : ApiController
    {
        private readonly ILabTestingService _labTestingService;
        public LabTestingApiController(ILabTestingService labTestingService)
        {
            this._labTestingService = labTestingService;
        }

        [HttpGet]
        [Route("api/labtesting/get-all")]
        public HttpResponseMessage GetAllLabTesting()
        {
            var labTesting = _labTestingService.GetAll();
            var labTestingDtos = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(labTesting);
            var response = Request.CreateResponse(HttpStatusCode.OK, labTestingDtos);
            return response;
        }
    }
}