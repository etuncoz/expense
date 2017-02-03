using System.Collections;
using System.Collections.Generic;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Request
{
    public class ExpenseCreateRequest
    {
        //public int ID { get; set; }
        //public int ExpenseId { get; set; }
        //public string Description { get; set; }
        //public decimal Amount { get; set; }
        //public System.DateTime ExpenseItemDate { get; set; }

        public IEnumerable<ExpenseItemDto> ExpenseItemsDto { get; set; }

    }
}
