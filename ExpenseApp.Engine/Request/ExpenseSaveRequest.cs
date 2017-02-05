using System.Collections.Generic;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Request
{
    public class ExpenseSaveRequest
    {
        public IEnumerable<ExpenseItemDto> ExpenseItemsDto { get; set; }
        public int ExpenseId { get; set; }
    }
}
