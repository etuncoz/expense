using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseApp.Engine.Response;
using ExpenseApp.Engine.Handlers;
using ExpenseApp.Data;

namespace ExpenseApp.Api
{
    public class UsersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult CheckUserActivity()
        {
            if (!UserHandlers.CheckIfUserIsActive().IsSuccess)
                return BadRequest();

            return Ok(UserHandlers.CheckIfUserIsActive());
        }
    }
}
