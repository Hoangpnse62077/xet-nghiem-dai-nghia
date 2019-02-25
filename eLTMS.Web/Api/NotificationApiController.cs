using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using eLTMS.Web.Utils;
using eLTMS.Models;

namespace eLTMS.Web.Api
{
    public class NotificationApiController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        public NotificationApiController(IAppointmentService appointmentService)
        {
            this._appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("api/notification/send")] 
        public IHttpActionResult SendMessage(string message)
        {
            var tokens = _appointmentService.GetAllTokens();
            foreach (var token in tokens)
            {
                var data = new
                {
                    to = token.TokenString,
                    data = new
                    {
                        message = message,
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
            }
            return Ok();
        }

    }
}
