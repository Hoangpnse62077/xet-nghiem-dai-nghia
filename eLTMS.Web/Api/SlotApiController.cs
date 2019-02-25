using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class SlotApiController : ApiController
    {
        private readonly ISlotService _slotService;
        public SlotApiController(ISlotService slotService)
        {
            this._slotService = slotService;
        }

        [HttpGet]
        [Route("api/slot/get-by-date-and-sampleid")]
        public HttpResponseMessage GetByDateAndSampleId(DateTime date, int sampleId)
        {
            var slots = _slotService.GetByDateAndSampleId(date, sampleId);
            var slotDtos = Mapper.Map<IEnumerable<Slot>, IEnumerable<SlotDto>>(slots);
            var response = Request.CreateResponse(HttpStatusCode.OK, slotDtos);
            return response;
        }
        
        [HttpGet]
        [Route("api/slot/get-available-slots")]
        public HttpResponseMessage GetAvailableSlots()
        {
            var slots = _slotService.GetAvailableSlots();
            var slotDtos = Mapper.Map<IEnumerable<Slot>, IEnumerable<SlotDto>>(slots);
            var response = Request.CreateResponse(HttpStatusCode.OK, slotDtos);
            return response;
        }

        [HttpGet]
        [Route("api/slot/creat-new-slots-for-a-month")]
        public HttpResponseMessage CreateNewSlotsForAMonth(int year, int month)
        {
            var success = _slotService.CreateNewSlotsForAMonth(year, month);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        [HttpGet]
        [Route("api/slot/get-all")]
        public HttpResponseMessage GetAll()
        {
            var slots = _slotService.GetAllSlots();
            var response = Request.CreateResponse(HttpStatusCode.OK, slots);
            return response;
        }

        [HttpGet]
        [Route("api/slot/get-slot-options")]
        public HttpResponseMessage GetSlotOptions(string gettingDate, int sampleGroupId)
        {
            var slotOptions = _slotService.GetSlotOptions(gettingDate, sampleGroupId);
            var response = Request.CreateResponse(HttpStatusCode.OK, slotOptions);
            return response;
        }
    }
}
