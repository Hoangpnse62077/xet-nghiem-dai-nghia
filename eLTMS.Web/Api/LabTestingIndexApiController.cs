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
    public class LabTestingIndexApiController : ApiController
    {
        private readonly ILabTestingIndexService _labTestingIndexService;
        public LabTestingIndexApiController(ILabTestingIndexService labTestingIndexService)
        {
            this._labTestingIndexService = labTestingIndexService;
        }

        [HttpPost]
        [Route("api/labtesting/add-lab-testing-indexes")]
        public HttpResponseMessage AddLabTestingIndexes(List<LabTestingIndexDto> ltiDtos)
        {
            var ltis = Mapper.Map<IEnumerable<LabTestingIndexDto>, IEnumerable<LabTestingIndex>>(ltiDtos).ToList();
            var result = _labTestingIndexService.AddLabTestingIndexes(ltis);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}