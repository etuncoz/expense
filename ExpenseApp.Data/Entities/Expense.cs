using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public class Expense
    {
        public Expense()
        {
            this.ExpenseHistories = new HashSet<ExpenseHistory>();
            this.ExpenseItems = new HashSet<ExpenseItem>();
        }

        public int ID { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? LastExpenseActionId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ExpenseHistory> ExpenseHistories { get; set; }
        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; }
    }
}
