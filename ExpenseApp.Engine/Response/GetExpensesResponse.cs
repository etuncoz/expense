using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Response
{
    public class GetExpensesResponse : BaseResponse
    {
        public IEnumerable<ExpenseDto> ExpenseDto { get; set; }
        public int UserId { get; set; }
    }
}
