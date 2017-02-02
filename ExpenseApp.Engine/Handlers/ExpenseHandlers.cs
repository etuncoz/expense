using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Response;

namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseHandlers
    {
        public static CreateExpenseResponse CreateExpense(IEnumerable<ExpenseItemDto> expenseItemsDto)
        {
            CreateExpenseResponse response = new CreateExpenseResponse();
            try
            {
                ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();
                Expense expense = new Expense();
                expenseAppEntities.Expenses.Add(expense);
                expenseAppEntities.SaveChanges();

                foreach (var item in expenseItemsDto)
                {
                    ExpenseItem expenseItem = new ExpenseItem();
                    expenseItem.ExpenseId = expense.ID;
                    expenseAppEntities.ExpenseItems.Add(expenseItem);
                    expenseAppEntities.SaveChanges();

                    expenseItem.ExpenseItemDate = item.ExpenseItemDate;
                    expenseItem.Description = item.Description;
                    expenseItem.Amount = item.Amount;

                    expense.TotalAmount += item.Amount;

                    // expenseItem = Mapper.Map<ExpenseItemDto, ExpenseItem>(item);             

                }
                expenseAppEntities.SaveChanges();
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw;
            }

            return response;
        }

        public static bool DeleteExpenseById(int id)
        {
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();
            expenseAppEntities = new ExpenseAppEntities();

            var expenseInDb = expenseAppEntities.Expenses.First(e => e.ID == id);

            if (expenseInDb == null)
                return false;

            expenseAppEntities.Expenses.Remove(expenseInDb);
            expenseAppEntities.SaveChanges();

            return true;
        }
    }
}
