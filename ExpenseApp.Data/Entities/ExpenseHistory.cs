using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public partial class ExpenseHistory
    {
        public int ID { get; set; }
        public int ExpenseId { get; set; }
        public int ExpenseStatusId { get; set; }
        public string RejectReason { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual Expense Expense { get; set; }
        public virtual ExpenseStatus ExpenseStatu { get; set; }
    }
}
