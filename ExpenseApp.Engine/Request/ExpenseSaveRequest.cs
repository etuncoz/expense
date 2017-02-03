using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Request
{
    public class ExpenseSaveRequest
    {
        public IEnumerable<ExpenseItemDto> ExpenseItemsDto { get; set; }
        public int ExpenseId { get; set; }
    }
}
