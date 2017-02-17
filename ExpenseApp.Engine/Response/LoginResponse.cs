using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Engine.Response
{
    public class LoginResponse : BaseResponse
    {
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
    }
}
