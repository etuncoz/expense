using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Engine.Response
{
    public class LoginResponse : BaseResponse
    {
        public string RedirectionUrl { get; set; }
    }
}
