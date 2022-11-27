using ExpenseApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApp.Data
{
    public partial class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext()
            : base("name=ExpenseDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }

        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseHistory> ExpenseHistories { get; set; }
        public virtual DbSet<ExpenseItem> ExpenseItems { get; set; }
        public virtual DbSet<ExpenseStatus> ExpenseStatus { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<GetUnApprovedExpense> GetUnApprovedExpenses { get; set; }
    }
}
