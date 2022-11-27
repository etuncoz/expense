using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public partial class ExpenseStatus
    {
        public ExpenseStatus()
        {
            this.ExpenseHistories = new HashSet<ExpenseHistory>();
        }

        public int ID { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<ExpenseHistory> ExpenseHistories { get; set; }
    }
}
