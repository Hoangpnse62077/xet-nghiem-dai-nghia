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
using eLTMS.Models.Enums;

namespace eLTMS.Web.Api
{
    public class AppointmentApiController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ITokenService _tokenService;
        public AppointmentApiController(IAppointmentService appointmentService, ITokenService tokenService)
        {
            this._appointmentService = appointmentService;
            this._tokenService = tokenService;
        }

        [HttpPost]
        [Route("api/appointment/create")]
        public HttpResponseMessage Create(AppointmentDto appoinDto)
        {
            if (appoinDto.PatientId != null)
            {
                appoinDto.PatientId = appoinDto.PatientId;
            }
            // call to AppointmentService
            var result = _appointmentService.Create(appoinDto);

            // push noti
            if (result.Success)
            {
                var tokens = _tokenService.GetAll();
                int[] roleIds = {
                    (int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Có cuộc hẹn vừa được thêm."
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-appointments-by-patient-id")]
        public HttpResponseMessage GetAppointmentsByPatientId(int patientId) // DucBM
        {
            var appDtos = _appointmentService.GetAppointmentsByPatientId(patientId);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpPut]
        [Route("api/appointment/update-appointment")]
        public HttpResponseMessage UpdateAppointment(AppointmentDto appointmentDto)
        {
            
            var result = this._appointmentService.UpdateAppointment(appointmentDto.AppointmentId, appointmentDto.SampleGettingDtos);

            // push noti
            if (result.Success)
            {
                var tokens = _tokenService.GetAll();
                int[] roleIds = {
                    (int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Có cuộc hẹn vừa được chỉnh sửa."
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpDelete]
        [Route("api/appointment/delete-appointment")]
        public HttpResponseMessage DeleteAppointment(int appointmentId)
        {
            var result = this._appointmentService.DeleteAppointment(appointmentId);

            // send noti
            if (result.Success)
            {
                var tokens = _tokenService.GetAll();
                int[] roleIds = {
                    (int)RoleEnum.Receptionist,
                    (int)RoleEnum.Cashier,
                    (int)RoleEnum.Manager
                };
                var data = new
                {
                    roleIds,
                    message = "Có cuộc hẹn vừa được hủy."
                };
                SendNotificationUtils.SendNotification(data, tokens);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-results-by-patient-id")]
        public HttpResponseMessage GetResultByPatientId(int patientId)
        {
            var app = _appointmentService.GetResult(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetResultDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        // DucBM
        [HttpGet]
        [Route("api/appointment/get-results-by-account-id")]
        public HttpResponseMessage GetResultByAccountId(int accountId)
        {
            var appDtos = _appointmentService.GetAppointmentsByAccountId(accountId);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-result-by-appointment-code")]
        public HttpResponseMessage GetResultByAppCode(string appCode)
        {
            var app = _appointmentService.GetResultByAppCode(appCode);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetResultDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-result-by-appointment-by-phone-and-date")]
        public HttpResponseMessage GetAppByPhoneNDate(string phone)
        {
            var app = _appointmentService.GetAppByPhone(phone);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetByPhoneDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-result-by-appointment-id")]
        public HttpResponseMessage GetResultByAppointmentId(int appointmentId)
        {
            var app = _appointmentService.GetResultDoneByAppointmentId(appointmentId);
            var appDto = Mapper.Map<Appointment, ResultOfAppointmentDto>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDto);
            return response;
        }

    }
}
