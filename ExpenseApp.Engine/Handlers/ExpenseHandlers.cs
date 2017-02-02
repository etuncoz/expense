using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseHandlers
    {
        
        public static void CreateExpenseByObj(IEnumerable<ExpenseItemDto> expenseItemsDto)
        {
            ExpenseAppEntities _db = new ExpenseAppEntities();

            Expense expense = new Expense();
            _db.Expenses.Add(expense);
            _db.SaveChanges();

            foreach (var item in expenseItemsDto)
            {
                ExpenseItem expenseItem = new ExpenseItem();
                expenseItem.ExpenseId = expense.ID;
                _db.ExpenseItems.Add(expenseItem);
                _db.SaveChanges();

                expenseItem.ExpenseItemDate = item.ExpenseItemDate;
                expenseItem.Description = item.Description;
                expenseItem.Amount = item.Amount;

                expense.TotalAmount += item.Amount;

                // expenseItem = Mapper.Map<ExpenseItemDto, ExpenseItem>(item);             

            }
            _db.SaveChanges();
        }

        public static bool DeleteById(int id)
        {
            ExpenseAppEntities _db = new ExpenseAppEntities();
            _db = new ExpenseAppEntities();

            var expenseInDb = _db.Expenses.SingleOrDefault(e => e.ID == id);

            if (expenseInDb == null)
                return false;

            _db.Expenses.Remove(expenseInDb);
            _db.SaveChanges();
            return true;
        }
    }
}
