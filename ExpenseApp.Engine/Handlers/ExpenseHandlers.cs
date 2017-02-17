using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Enum;
using ExpenseApp.Engine.Response;
using ExpenseApp.Engine.Request;
using System.Data.Entity;
using log4net;


namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseHandlers
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static BaseResponse DeleteExpenseById(IdRequest request)
        {
            BaseResponse response = new BaseResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                var expenseInDb = entity.Expenses.FirstOrDefault(e => e.ID == request.ID);

                if (expenseInDb == null)
                {
                    response.IsSuccess = false;
                    return response;
                }

                var expenseItemsInDb = from e in entity.ExpenseItems
                                       where e.ExpenseId == request.ID
                                       select e;

                if (!expenseItemsInDb.Any())
                {
                    DeleteExpenseHistory(expenseInDb, entity);
                    DeleteExpense(expenseInDb, entity);

                    response.IsSuccess = true;
                    return response;
                }

                foreach (var item in expenseItemsInDb)
                {
                    entity.ExpenseItems.Remove(item);
                }

                DeleteExpenseHistory(expenseInDb, entity);
                DeleteExpense(expenseInDb, entity);
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Delete Expense Unsuccessful", ex);
            }
            finally
            {
                entity.Dispose();
            }

            return response;
        }

        public static GetExpensesResponse GetExpenseByUserId(int userId)
        {
            GetExpensesResponse response = new GetExpensesResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                response.ExpenseDto = from expense in entity.Expenses.Where(e=>e.UserId == userId)
                                      //where expense.UserId == userId
                                      //from user in Entity.Users.Where(user => user.ID == expense.UserId)
                                      //from history in Entity.ExpenseHistories.Where(history => history.ExpenseStatusId == expense.LastExpenseActionId)
                                      from status in entity.ExpenseStatus.Where(status => status.ID == expense.LastExpenseActionId)
                                      select new ExpenseDto
                                      {
                                          ID = expense.ID,
                                          CreatedDate = expense.CreatedDate,
                                          TotalAmount = expense.TotalAmount,
                                          UserId = expense.UserId,
                                          LastExpenseActionId = expense.LastExpenseActionId,
                                          CurrentStatus = status.StatusName
                                      };

                var user = (from u in entity.Users
                            where u.ID == userId
                            select u).Single();
                response.UserId = user.ID;

                //test
                //var expense = Entity.Expenses
                //            .Include(e => e.User)
                //            .Include(e => e.ExpenseHistories)
                //            .FirstOrDefault(x => x.ID == 4);
                //var a = expense.ExpenseHistories.LastOrDefault().ExpenseStatusId;

                response.IsSuccess = response.ExpenseDto == null ? false : true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Get Expense By UserId Unsuccessful", ex);
            }
            return response;
        }

        public static GetExpensesResponse GetExpenseByActionId(IdRequest request)
        {
            GetExpensesResponse response = new GetExpensesResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                response.ExpenseDto = from expense in entity.Expenses
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
                log.Error("Get Expense By UserId Unsuccessful", ex);
            }

            return response;
        }

        public static GetExpenseItemsResponse GetExpenseItemsByExpenseId(int expenseId)
        {
            GetExpenseItemsResponse response = new GetExpenseItemsResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                response.ExpenseItemDto = from expenseItem in entity.ExpenseItems
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
                log.Error("Get ExpenseItems By ExpenseId Unsuccessful", ex);
            }
            return response;
        }
        public static SaveExpenseResponse SaveExpense(ExpenseSaveRequest request)
        {
            SaveExpenseResponse response = new SaveExpenseResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                if (request.ExpenseId == 0)
                {
                    CreateExpense(request.UserId, request.ExpenseItemsDto, entity, response);
                    CreateExpenseHistory(response.ExpenseId, entity, null);
                }
                else
                {
                    UpdateExpense(request, entity, response);
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Save Expense Unsuccessful", ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        private static void CreateExpense(int userId, IEnumerable<ExpenseItemDto> expenseItemsDto, ExpenseAppEntities entity, SaveExpenseResponse response)
        {
            Expense expense = new Expense();
            expense.CreatedDate = DateTime.Now;
            expense.UserId = userId;
            entity.Expenses.Add(expense);
            entity.SaveChanges();

            foreach (var item in expenseItemsDto)
            {
                CreateExpenseItem(expense, item, entity);
                expense.TotalAmount += item.Amount;

            }
            expense.LastExpenseActionId = (int)StatusEnum.Ongoing;
            entity.SaveChanges();
            response.ExpenseId = expense.ID;
            response.IsSuccess = true;
        }

        private static void UpdateExpense(ExpenseSaveRequest request, ExpenseAppEntities entity, SaveExpenseResponse response)
        {
            var expenseInDb = (from e in entity.Expenses
                               where e.ID == request.ExpenseId
                               select e).FirstOrDefault();
            if (expenseInDb == null)
            {
                response.IsSuccess = false;
                return;
            }

            expenseInDb.TotalAmount = 0;

            if (request.DeletedExpenseItems.Any())
                DeleteExpenseItem(request.DeletedExpenseItems, entity, response);

            foreach (var item in request.ExpenseItemsDto)
            {
                expenseInDb.TotalAmount += item.Amount;

                var expenseItemInDb = (from e in entity.ExpenseItems
                                       where e.ID == item.ID
                                       select e).FirstOrDefault();

                if (expenseItemInDb == null) 
                {
                    CreateExpenseItem(expenseInDb, item, entity);
                }
                else                        
                {
                    UpdateExpenseItem(expenseItemInDb, item, entity);
                }
            }
            expenseInDb.LastExpenseActionId = (int)StatusEnum.Ongoing;
            entity.SaveChanges();
            response.IsSuccess = true;
        }

        private static void CreateExpenseItem(Expense expenseInDb, ExpenseItemDto expenseItemDto, ExpenseAppEntities entity)
        {
            ExpenseItem expenseItem = new ExpenseItem();
            expenseItem.ExpenseId = expenseInDb.ID;
            entity.ExpenseItems.Add(expenseItem);

            expenseItem.ExpenseItemDate = expenseItemDto.ExpenseItemDate;
            expenseItem.Description = expenseItemDto.Description;
            expenseItem.Amount = expenseItemDto.Amount;

            entity.SaveChanges();
        }

        private static void DeleteExpenseItem(IEnumerable<IdDto> deletedExpenseItems, ExpenseAppEntities entity, SaveExpenseResponse response)
        {
            foreach (var item in deletedExpenseItems)
            {
                var itemInDb = (from e in entity.ExpenseItems
                                where e.ID == item.ID
                                select e).FirstOrDefault();

                if (itemInDb == null)
                {
                    response.IsSuccess = false;
                    return;
                }
                entity.ExpenseItems.Remove(itemInDb);
            }
        }

        private static void UpdateExpenseItem(ExpenseItem expenseItemInDb, ExpenseItemDto expenseItemDto, ExpenseAppEntities entity)
        {
            expenseItemInDb.ExpenseItemDate = expenseItemDto.ExpenseItemDate;
            expenseItemInDb.Description = expenseItemDto.Description;
            expenseItemInDb.Amount = expenseItemDto.Amount;
            entity.SaveChanges();
        }

        private static void DeleteExpense(Expense expense, ExpenseAppEntities entity)
        {
            entity.Expenses.Remove(expense);
            entity.SaveChanges();
        }

        private static void DeleteExpenseHistory(Expense expense, ExpenseAppEntities entity)
        {
            var expenseHistories = (from histories in entity.ExpenseHistories
                                    where histories.ExpenseId == expense.ID
                                    select histories);
            foreach (var history in expenseHistories)
            {
                entity.ExpenseHistories.Remove(history);
            }
            entity.SaveChanges();
        }

        public static void CreateExpenseHistory(int expenseId, ExpenseAppEntities entity, string rejectReason)
        {
            var expense = (from e in entity.Expenses
                           where e.ID == expenseId
                           select e).First();

            ExpenseHistory expenseHistory = new ExpenseHistory();
            expenseHistory.ExpenseId = expense.ID;
            expenseHistory.CreatedBy = expense.UserId;
            expenseHistory.CreatedDate = DateTime.Now;

            switch (expense.LastExpenseActionId)
            {
                case 1:
                    expenseHistory.ExpenseStatusId = (int)StatusEnum.Ongoing;
                    break;
                case 2:
                    expenseHistory.ExpenseStatusId = (int)StatusEnum.WaitingForManagerApproval;
                    break;
                case 3:
                    expenseHistory.ExpenseStatusId = (int)StatusEnum.WaitingForAccountantApproval;
                    break;
                case 4:
                    expenseHistory.ExpenseStatusId = (int)StatusEnum.Completed;
                    break;
                case 5:
                    expenseHistory.ExpenseStatusId = (int)StatusEnum.Rejected;
                    expenseHistory.RejectReason = rejectReason;
                    break;
            }
            entity.SaveChanges();

            entity.ExpenseHistories.Add(expenseHistory);
            entity.SaveChanges();
        }

        public static GetRejectReasonResponse GetRejectReason(ExpenseGetRequest request)
        {
            GetRejectReasonResponse response = new GetRejectReasonResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();
            try
            {
                response.RejectReason = (from eh in entity.ExpenseHistories
                                         where eh.ExpenseId == request.ExpenseId && eh.ExpenseStatusId == (int)StatusEnum.Rejected
                                         orderby eh.CreatedDate descending
                                         select eh.RejectReason).FirstOrDefault();

                if (response.RejectReason == null)
                {
                    return response;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Get Reject Reason Unsuccessful", ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;

        }
    }
}
