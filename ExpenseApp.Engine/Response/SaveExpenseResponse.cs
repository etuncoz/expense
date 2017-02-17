using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Response
{
    public class SaveExpenseResponse : BaseResponse
    {
        public int ExpenseId { get; set; }
        public IEnumerable<ExpenseItemDto> ExpenseItemsDto { get; set; }
    }
}
