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
    public class FeedbackApiController : ApiController
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackApiController(IFeedbackService feedbackService)
        {
            this._feedbackService = feedbackService;
        }

        [HttpPost]
        [Route("api/feedback/create")]
        public HttpResponseMessage Create(FeedbackDto feedbackDto)
        {
            var feedback = Mapper.Map<FeedbackDto, Feedback>(feedbackDto);
            var success = true;
            var message = "Gửi feedback thành công";
            try
            {
                _feedbackService.Create(feedback);
            } catch(Exception ex)
            {
                success = false;
                message = ex.Message + "\n" + ex.StackTrace;
            }
            var result = new
            {
                Success = success,
                Message = message
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

    }
}
