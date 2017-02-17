using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Engine.Response
{
    public class ShouldCheckEmailResponse : BaseResponse
    {
        public bool IsNeeded { get; set; }
    }
}
