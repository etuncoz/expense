using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Response;
using AutoMapper;

namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseHandlers
    {
        public static SaveExpenseResponse CreateExpense(IEnumerable<ExpenseItemDto> expenseItemsDto)
        {
            SaveExpenseResponse response = new SaveExpenseResponse();
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

                }
                expenseAppEntities.SaveChanges();
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }

            return response;
        }

        public static BaseResponse DeleteExpenseById(int expenseId)
        {
            BaseResponse response = new BaseResponse(); 
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                var expenseInDb = expenseAppEntities.Expenses.First(e => e.ID == expenseId);

                if (expenseInDb == null)
                    response.IsSuccess = false;

                expenseAppEntities.Expenses.Remove(expenseInDb);
                expenseAppEntities.SaveChanges();

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }

            return response;
        }

        public static GetExpensesResponse GetExpenseByUserId(int userId) { // write overloads for expenseId,lastexpenseactionid if needed

            GetExpensesResponse response = new GetExpensesResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                response.ExpenseDto = from expense in expenseAppEntities.Expenses
                                   where expense.UserId == userId
                                    select new ExpenseDto {
                                        ID = expense.ID,
                                        CreatedDate = expense.CreatedDate,
                                        UserId = expense.UserId,
                                        TotalAmount = expense.TotalAmount
                                    };

                if (response.ExpenseDto == null)
                    response.IsSuccess = false;

                response.IsSuccess = true;
                    
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            return response;
        }

        public static GetExpenseItemsResponse GetExpenseItemsByExpenseId(int expenseId)
        { 
            GetExpenseItemsResponse response = new GetExpenseItemsResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                response.ExpenseItemDto = from expenseItem in expenseAppEntities.ExpenseItems
                                      where expenseItem.ExpenseId == expenseId
                                      select new ExpenseItemDto
                                      {
                                          ID = expenseItem.ID,
                                          ExpenseItemDate = expenseItem.ExpenseItemDate,
                                          Description = expenseItem.Description,
                                          Amount = expenseItem.Amount,
                                          ExpenseId = expenseItem.ExpenseId
                                      };

                if (response.ExpenseItemDto == null)
                    response.IsSuccess = false;

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            return response;
        }
        public static SaveExpenseResponse SaveExpense(IEnumerable<ExpenseItemDto> expenseItemsDto, int expenseId)
        {
            SaveExpenseResponse response = new SaveExpenseResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            if (expenseId == 0)
            {
                try
                {
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

                    }
                    expenseAppEntities.SaveChanges();
                    response.IsSuccess = true;

                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    throw ex;
                }
            } 
            if (expenseId > 0)
            {
                try
                {
                    var expenseInDb = (from e in expenseAppEntities.Expenses
                                      where e.ID == expenseId
                                      select e).First();
                    if (expenseInDb == null)
                    {
                        response.IsSuccess = false;
                        return response;
                    }

                    foreach (var item in expenseItemsDto) //expenseItem db de var mı diye kontrol edilecek mi?*****
                    {
                        var expenseItemInDb = (from e in expenseAppEntities.ExpenseItems
                                               where e.ID == item.ID
                                               select e).First();

                        expenseItemInDb.ExpenseItemDate = item.ExpenseItemDate;
                        expenseItemInDb.Description = item.Description;
                        expenseItemInDb.Amount = item.Amount;
                        expenseAppEntities.SaveChanges();
                        expenseInDb.TotalAmount += item.Amount;

                    }
                    expenseAppEntities.SaveChanges();
                    response.IsSuccess = true;

                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    throw ex;
                }
            }
            return response;
        }
    }
}
