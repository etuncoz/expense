using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Request;
using ExpenseApp.Engine.Response;

namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseActionHandlers
    {
        public static GetCurrentExpenseResponse GetCurrentExpenseStatus(int expenseId)
        {
            GetCurrentExpenseResponse response = new GetCurrentExpenseResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                response.LastExpenseActionId = (from expense in entity.Expenses
                                            where expense.ID == expenseId
                                            select expense.LastExpenseActionId).FirstOrDefault();

                if (response.LastExpenseActionId == null)
                {
                    response.IsSuccess = false;
                    return response;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        //Not needed at the moment
        //public static GetCurrentUserResponse GetCurrentUserId(int expenseId)
        //{
        //    GetCurrentUserResponse response = new GetCurrentUserResponse();
        //    ExpenseAppEntities entity = new ExpenseAppEntities();

        //    try
        //    {
        //        var expense = (from e in entity.Expenses
        //                    where e.ID == expenseId
        //                    select e).FirstOrDefault();

        //        if (expense == null)
        //        {
        //            response.IsSuccess = false;
        //            return response;
        //        }
        //        response.UserId = expense.UserId;
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        entity.Dispose();
        //    }
        //    return response;
        //}

        public static BaseResponse SendExpenseForApproval(IdRequest request)
        {
            BaseResponse response = new BaseResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                var expenseToBeSentForApproval = (from expense in entity.Expenses
                                          where expense.ID == request.ID
                                          select expense).FirstOrDefault();
                if (expenseToBeSentForApproval == null)
                {
                    response.IsSuccess = false;
                    return response;
                }

                expenseToBeSentForApproval.LastExpenseActionId = 2;
                ExpenseHandlers.CreateExpenseHistory(request.ID, entity);
                response.IsSuccess = true;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess=false;
                throw ex;
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        public static ApprovalExpenseResponse ApproveOrRejectExpense(ExpenseApprovalRequest request) 
        {
            ExpenseAppEntities entity = new ExpenseAppEntities();
            ApprovalExpenseResponse response = new ApprovalExpenseResponse();

            try
            {
                if (request.IsApproved) 
                {
                    ApproveExpense(request, entity, response);
                    ExpenseHandlers.CreateExpenseHistory(request.ExpenseId, entity);
                }
                else
                {
                    RejectExpense(request, entity, response);
                    ExpenseHandlers.CreateExpenseHistory(request.ExpenseId, entity);
                }

            }   
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        private static void ApproveExpense(ExpenseApprovalRequest request,ExpenseAppEntities entity, ApprovalExpenseResponse response)
        {
            var expenseToBeApproved = (from expense in entity.Expenses
                          where expense.ID == request.ExpenseId
                          select expense).FirstOrDefault();
            if (expenseToBeApproved == null)
            {
                response.IsSuccess = false;
                return;
            }
            expenseToBeApproved.LastExpenseActionId = 3;
            entity.SaveChanges();
            response.IsSuccess = true;
        }
        private static void RejectExpense(ExpenseApprovalRequest request, ExpenseAppEntities entity, ApprovalExpenseResponse response)
        {
            var expenseToBeRejected = (from expense in entity.Expenses
                                       where expense.ID == request.ExpenseId
                                       select expense).FirstOrDefault();
            if (expenseToBeRejected == null)
            {
                response.IsSuccess = false;
                return;
            }
            expenseToBeRejected.LastExpenseActionId = 5;
            entity.SaveChanges();
            response.IsSuccess = true;
        }

        public static BaseResponse PayExpense(IdRequest request)
        {
            BaseResponse response = new BaseResponse();
            ExpenseAppEntities entity = new ExpenseAppEntities();

            try
            {
                var expenseToBePaid = (from expense in entity.Expenses
                                    where expense.ID == request.ID
                                    select expense).FirstOrDefault();
                if (expenseToBePaid == null)
                {
                    response.IsSuccess = false;
                    return response;
                }
                expenseToBePaid.LastExpenseActionId = 4;
                ExpenseHandlers.CreateExpenseHistory(request.ID, entity);
                entity.SaveChanges();
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                throw ex;
            }
            finally
            {

            }
            return response;
        }
    }
}
