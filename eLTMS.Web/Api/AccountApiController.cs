using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class AccountApiController : ApiController
    {
        private readonly IAccountService _accountService;
        public AccountApiController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        /*
        [HttpPost]
        [Route("api/account/login")]
        public HttpResponseMessage Login(loginDto loginDto)
        {
            var phoneNumber = loginDto.PhoneNumber; // sure that not empty from client
            var password = loginDto.Password; // sure that not empty from client
            var account = this._accountService.checkLogin(phoneNumber, password);
            object loginResult = null;
            if (account != null)
            {
                //var patient = account.Patients.FirstOrDefault();
                loginResult = new
                {
                    Success = true,
                    //PatientId = patient.PatientId,
                    PhoneNumber = phoneNumber,
                    FullName = account.FullName,
                };
            }
            else
            {
                loginResult = new
                {
                    Success = account != null,
                    PatientId = -1,
                    PhoneNumber = phoneNumber,
                    FullName = ""
                };
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, loginResult);
            return response;
        }/**/

        [HttpPost]
        [Route("api/account/login-patient")]
        public HttpResponseMessage LoginPatient(LoginDto loginDto)
        {
            var phoneNumber = loginDto.PhoneNumber; // sure that not empty from client
            var password = loginDto.Password; // sure that not empty from client
            var respObj = this._accountService.CheckLoginPatient(phoneNumber, password);
            var response = Request.CreateResponse(HttpStatusCode.OK, respObj);
            return response;
        }

        [HttpPost]
        [Route("api/account/register-patient")]
        public HttpResponseMessage RegisterPatient(RegisterDto regDto)
        {
            var respObj = this._accountService.RegisterPatient(regDto);
            var response = Request.CreateResponse(HttpStatusCode.OK, respObj);
            return response;
        }

    }
}
