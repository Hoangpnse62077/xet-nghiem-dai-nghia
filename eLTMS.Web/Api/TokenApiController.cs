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
    public class TokenApiController : ApiController
    {
        private readonly ITokenService _TokenService;
        public TokenApiController(ITokenService TokenService)
        {
            this._TokenService = TokenService;
        }

        [HttpPost]
        [Route("api/token/create")]
        public HttpResponseMessage Create(TokenDto tokenDto)
        {
            var success = _TokenService.Create(tokenDto.TokenString);
            var result = new
            {
                success = success
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

    }
}
