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
        public static BaseResponse DeleteExpenseById(int expenseId)
        {
            BaseResponse response = new BaseResponse(); 
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                var expenseInDb = expenseAppEntities.Expenses.FirstOrDefault(e => e.ID == expenseId);

                if (expenseInDb == null)
                {
                    response.IsSuccess = false;
                    return response;
                }

                var expenseItemsInDb = from e in expenseAppEntities.ExpenseItems
                    where e.ExpenseId == expenseId
                    select e;

                if (!expenseItemsInDb.Any())
                {
                    expenseAppEntities.Expenses.Remove(expenseInDb);
                    expenseAppEntities.SaveChanges();

                    response.IsSuccess = true;
                    return response;                 //return yerine throw mu etmeli?
                }

                foreach (var item in expenseItemsInDb)
                {
                    expenseAppEntities.ExpenseItems.Remove(item);
                }
                expenseAppEntities.Expenses.Remove(expenseInDb);
                expenseAppEntities.SaveChanges();
                response.IsSuccess = true;
                return response;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }

            return response;
        }

        public static BaseResponse DeleteExpenseItemById(int expenseItemId)
        {
            BaseResponse response = new BaseResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                var expenseItemInDb = expenseAppEntities.ExpenseItems.FirstOrDefault(e => e.ID == expenseItemId);

                if (expenseItemInDb == null)
                {
                    response.IsSuccess = false;
                    return response;
                }

                expenseAppEntities.ExpenseItems.Remove(expenseItemInDb);
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

                        expense.
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

                    expenseInDb.TotalAmount = 0;

                    foreach (var item in expenseItemsDto)
                    {
                        expenseInDb.TotalAmount += item.Amount;
                         
                        var expenseItemInDb = (from e in expenseAppEntities.ExpenseItems
                                               where e.ID == item.ID
                                               select e).FirstOrDefault();
                        if (expenseItemInDb == null) // new expense item must be created
                        {
                            ExpenseItem expenseItem = new ExpenseItem();
                            expenseItem.ExpenseId = expenseInDb.ID;
                            expenseAppEntities.ExpenseItems.Add(expenseItem);
                            expenseAppEntities.SaveChanges();

                            expenseItem.ExpenseItemDate = item.ExpenseItemDate;
                            expenseItem.Description = item.Description;
                            expenseItem.Amount = item.Amount;
                        }
                        else // expense item exists, will be updated
                        {/*------totalamount düzenle---*/
                            expenseItemInDb.ExpenseItemDate = item.ExpenseItemDate;
                            expenseItemInDb.Description = item.Description;
                            expenseItemInDb.Amount = item.Amount;
                            expenseAppEntities.SaveChanges();
                        }



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
