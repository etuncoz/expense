using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Response
{
    public class GetExpenseItemsResponse : BaseResponse
    {
        public IEnumerable<ExpenseItemDto> ExpenseItemDto { get; set; }
    }
}
