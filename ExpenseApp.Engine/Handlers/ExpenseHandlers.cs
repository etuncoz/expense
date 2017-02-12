using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Response;
using ExpenseApp.Engine.Request;
using System.Data.Entity;

namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseHandlers
    {


        public static BaseResponse DeleteExpenseById(IdRequest request)
        {
            BaseResponse response = new BaseResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                var expenseInDb = expenseAppEntities.Expenses.FirstOrDefault(e => e.ID == request.ID);

                if (expenseInDb == null)
                {
                    response.IsSuccess = false;
                    return response;
                }

                var expenseItemsInDb = from e in expenseAppEntities.ExpenseItems
                                       where e.ExpenseId == request.ID
                                       select e;

                if (!expenseItemsInDb.Any())
                {
                    DeleteExpenseHistory(expenseInDb, expenseAppEntities);
                    DeleteExpense(expenseInDb, expenseAppEntities);

                    response.IsSuccess = true;
                    return response;
                }

                foreach (var item in expenseItemsInDb)
                {
                    expenseAppEntities.ExpenseItems.Remove(item);
                }

                DeleteExpenseHistory(expenseInDb, expenseAppEntities);
                DeleteExpense(expenseInDb, expenseAppEntities);
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }

            return response;
        }

        public static GetExpensesResponse GetExpenseByUserId(int userId)
        {
            GetExpensesResponse response = new GetExpensesResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {

                response.ExpenseDto = from expense in expenseAppEntities.Expenses
                                //from user in expenseAppEntities.Users.Where(user => user.ID == expense.UserId)
                                //from history in expenseAppEntities.ExpenseHistories.Where(history => history.ExpenseStatusId == expense.LastExpenseActionId)
                                 from status in expenseAppEntities.ExpenseStatus.Where(status => status.ID == expense.LastExpenseActionId)
                                 select new ExpenseDto
                                 { 
                                     ID                  = expense.ID,
                                     CreatedDate         = expense.CreatedDate,
                                     TotalAmount         = expense.TotalAmount,
                                     UserId              = expense.UserId,
                                     LastExpenseActionId = expense.LastExpenseActionId,
                                     CurrentStatus       = status.StatusName
                                 };

                var user = (from u in expenseAppEntities.Users
                                   where u.ID == userId
                                   select u).Single();
                response.UserId = user.ID;



                //deprecated
                //var expe = expenseAppEntities.Expenses
                //            .Include(e => e.User)
                //            .Include(e => e.ExpenseHistories)
                //            .FirstOrDefault(x => x.ID == 4);
                //var a = expe.ExpenseHistories.LastOrDefault().ExpenseStatusId;

                response.IsSuccess = response.ExpenseDto == null ? false : true;  
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            return response;
        }

        public static GetExpensesResponse GetExpenseByActionId(IdRequest request)
        {
            GetExpensesResponse response = new GetExpensesResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                response.ExpenseDto = from expense in expenseAppEntities.Expenses
                                      where expense.LastExpenseActionId == request.ID
                                      select new ExpenseDto
                                      {
                                          ID = expense.ID,
                                          CreatedDate = expense.CreatedDate,
                                          TotalAmount = expense.TotalAmount,
                                          UserId = expense.UserId,
                                          UserName = expense.User.FullName
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
        public static SaveExpenseResponse SaveExpense(ExpenseSaveRequest request)
        {
            SaveExpenseResponse response = new SaveExpenseResponse();
            ExpenseAppEntities expenseAppEntities = new ExpenseAppEntities();

            try
            {
                if (request.ExpenseId == 0)
                {
                    CreateExpense(request.UserId,request.ExpenseItemsDto, expenseAppEntities, response);
                    CreateExpenseHistory(response.ExpenseId, expenseAppEntities);
                }
                else
                {
                    UpdateExpense(request, expenseAppEntities, response);
                }
                response.RedirectionUrl = "/employee/index/" + request.UserId;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            finally
            {
                expenseAppEntities.Dispose();
            }

            return response;

        }

        private static void CreateExpense(int userId,IEnumerable<ExpenseItemDto> expenseItemsDto, ExpenseAppEntities expenseAppEntities, SaveExpenseResponse response)
        {
            Expense expense = new Expense(userId);//*09123**219*1209*0 degistir
            expenseAppEntities.Expenses.Add(expense);
            expenseAppEntities.SaveChanges();

            foreach (var item in expenseItemsDto)
            {
                CreateExpenseItem(expense, item, expenseAppEntities);
                expense.TotalAmount += item.Amount;

            }
            expense.LastExpenseActionId = 1;
            expenseAppEntities.SaveChanges();
            response.ExpenseId = expense.ID;
            response.IsSuccess = true;
        }

        private static void UpdateExpense(ExpenseSaveRequest request, ExpenseAppEntities expenseAppEntities, SaveExpenseResponse response)
        {
            var expenseInDb = (from e in expenseAppEntities.Expenses
                               where e.ID == request.ExpenseId
                               select e).FirstOrDefault();
            if (expenseInDb == null)
            {
                response.IsSuccess = false;
                return;
            }

            expenseInDb.TotalAmount = 0;

            if (request.DeletedExpenseItems.Any())
                DeleteExpenseItem(request.DeletedExpenseItems, expenseAppEntities, response);

            foreach (var item in request.ExpenseItemsDto)
            {
                expenseInDb.TotalAmount += item.Amount;

                var expenseItemInDb = (from e in expenseAppEntities.ExpenseItems
                                       where e.ID == item.ID
                                       select e).FirstOrDefault();

                if (expenseItemInDb == null) // Create expense item
                {
                    CreateExpenseItem(expenseInDb, item, expenseAppEntities);
                }
                else                       // Update expense item
                {
                    UpdateExpenseItem(expenseItemInDb, item, expenseAppEntities);
                }
            }

            expenseAppEntities.SaveChanges();
            response.IsSuccess = true;
        }

        private static void CreateExpenseItem(Expense expenseInDb, ExpenseItemDto expenseItemDto, ExpenseAppEntities expenseAppEntities)
        {
            ExpenseItem expenseItem = new ExpenseItem();
            expenseItem.ExpenseId = expenseInDb.ID;
            expenseAppEntities.ExpenseItems.Add(expenseItem);
            expenseAppEntities.SaveChanges();

            expenseItem.ExpenseItemDate = expenseItemDto.ExpenseItemDate;
            expenseItem.Description = expenseItemDto.Description;
            expenseItem.Amount = expenseItemDto.Amount;
        }

        private static void DeleteExpenseItem(IEnumerable<IdDto> deletedExpenseItems, ExpenseAppEntities expenseAppEntities, SaveExpenseResponse response)
        {
            foreach (var item in deletedExpenseItems)
            {
                var itemInDb = (from e in expenseAppEntities.ExpenseItems
                                where e.ID == item.ID
                                select e).FirstOrDefault();
                //var itemInDb = expenseAppEntities.ExpenseItems.FirstOrDefault(e => e.ID == item.ID);

                if (itemInDb == null)
                {
                    response.IsSuccess = false;
                    return;
                }
                expenseAppEntities.ExpenseItems.Remove(itemInDb);
            }
        }

        private static void UpdateExpenseItem(ExpenseItem expenseItemInDb, ExpenseItemDto expenseItemDto, ExpenseAppEntities expenseAppEntities)
        {
            expenseItemInDb.ExpenseItemDate = expenseItemDto.ExpenseItemDate;
            expenseItemInDb.Description = expenseItemDto.Description;
            expenseItemInDb.Amount = expenseItemDto.Amount;
            expenseAppEntities.SaveChanges();
        }

        private static void DeleteExpense(Expense expense, ExpenseAppEntities expenseAppEntities)
        {

            expenseAppEntities.Expenses.Remove(expense);
            expenseAppEntities.SaveChanges();
        }

        private static void DeleteExpenseHistory(Expense expense, ExpenseAppEntities expenseAppEntities)
        {
            var expenseHistories = (from histories in expenseAppEntities.ExpenseHistories
                                    where histories.ExpenseId == expense.ID
                                    select histories);
            foreach (var history in expenseHistories)
            {
                expenseAppEntities.ExpenseHistories.Remove(history);
            }
            expenseAppEntities.SaveChanges();
        }

        public static void CreateExpenseHistory(int expenseId, ExpenseAppEntities expenseAppEntities)
        {
            var expense = (from e in expenseAppEntities.Expenses
                           where e.ID == expenseId
                           select e).First();

            ExpenseHistory expenseHistory =  new ExpenseHistory(expense.ID,expense.UserId);//new ExpenseHistory(expense.ID, expense.UserId);

            if (expense.LastExpenseActionId == 1)                        // expense was created
                expenseHistory.ExpenseStatusId = 1;
            if (expense.LastExpenseActionId == 2)                        // expense was sent for management approval
                expenseHistory.ExpenseStatusId = 2;
            if (expense.LastExpenseActionId == 3)                        // expense was sent for accountant approval
                expenseHistory.ExpenseStatusId = 3;
            if (expense.LastExpenseActionId == 4)                        // transaction completed
                expenseHistory.ExpenseStatusId = 4;
            if (expense.LastExpenseActionId == 5)                        // expense rejected
                expenseHistory.ExpenseStatusId = 5;

            expenseAppEntities.SaveChanges();

            expenseAppEntities.ExpenseHistories.Add(expenseHistory);
            expenseAppEntities.SaveChanges();
        }
    }
}
