using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public partial class GetUnApprovedExpense
    {
        public int ID { get; set; }
        public DateTime? ExpenseCreatedDate { get; set; }
        public int ExpenseId { get; set; }
    }
}
