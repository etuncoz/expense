using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApp.Engine.Response
{
    public class GetCurrentUserResponse : BaseResponse
    {
        public int UserId { get; set; }
    }
}