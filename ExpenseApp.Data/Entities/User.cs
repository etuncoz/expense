using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data.Entities
{
    public partial class User
    {
        public User()
        {
            this.Expenses = new HashSet<Expense>();
        }

        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int UserRoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
