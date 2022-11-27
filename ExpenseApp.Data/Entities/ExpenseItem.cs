using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public partial class ExpenseItem
    {
        public int ID { get; set; }
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime ExpenseItemDate { get; set; }

        public virtual Expense Expense { get; set; }
    }
}
