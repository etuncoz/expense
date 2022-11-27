using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Request;
using ExpenseApp.Engine.Response;
using ExpenseApp.Engine.Enum;


namespace ExpenseApp.Engine.Handlers
{
    public class ExpenseActionHandlers
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static GetCurrentExpenseResponse GetCurrentExpenseStatus(int expenseId)
        {
            GetCurrentExpenseResponse response = new GetCurrentExpenseResponse();
            ExpenseDbContext entity = new ExpenseDbContext();

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
                log.Error("Get Current Expense Status Unsuccessful",ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }
        public static BaseResponse SendExpenseForApproval(IdRequest request)
        {
            BaseResponse response = new BaseResponse();
            ExpenseDbContext entity = new ExpenseDbContext();

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

                expenseToBeSentForApproval.LastExpenseActionId = (int)StatusEnum.WaitingForManagerApproval;
                ExpenseHandlers.CreateExpenseHistory(request.ID,entity,null);
                response.IsSuccess = true;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess=false;
                log.Error("Send Expense For Approval Unsuccessful", ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        public static ApprovalExpenseResponse ApproveOrRejectExpense(ExpenseApprovalRequest request) 
        {
            ExpenseDbContext entity = new ExpenseDbContext();
            ApprovalExpenseResponse response = new ApprovalExpenseResponse();

            try
            {
                //to do:switch
                if (request.IsApproved) 
                {
                    ApproveExpense(request, entity, response);
                    ExpenseHandlers.CreateExpenseHistory(request.ExpenseId, entity, request.RejectReason);
                    return response;
                }
                else if(!request.IsApproved)
                {
                    RejectExpense(request, entity, response);
                    ExpenseHandlers.CreateExpenseHistory(request.ExpenseId, entity, request.RejectReason);
                    return response;
                }
                else
                {
                    response.ApprovalStatus = (int)ApprovalStatusEnum.Unknown;
                    response.IsSuccess = false;
                    return response;
                }                
            }   
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Approve or Reject Expense Unsuccessful", ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }

        private static void ApproveExpense(ExpenseApprovalRequest request, ExpenseDbContext entity, ApprovalExpenseResponse response)
        {
            var expenseToBeApproved = (from expense in entity.Expenses
                          where expense.ID == request.ExpenseId
                          select expense).FirstOrDefault();
            if (expenseToBeApproved == null)
            {
                response.IsSuccess = false;
                return;
            }
            expenseToBeApproved.LastExpenseActionId = (int)StatusEnum.WaitingForAccountantApproval;
            entity.SaveChanges();
            response.ApprovalStatus = (int)ApprovalStatusEnum.Approved;
            response.IsSuccess = true;
        }
        private static void RejectExpense(ExpenseApprovalRequest request, ExpenseDbContext entity, ApprovalExpenseResponse response)
        {
            var expenseToBeRejected = (from expense in entity.Expenses
                                       where expense.ID == request.ExpenseId
                                       select expense).FirstOrDefault();
            if (expenseToBeRejected == null)
            {
                response.IsSuccess = false;
                return;
            }
            expenseToBeRejected.LastExpenseActionId = (int)StatusEnum.Rejected;
            entity.SaveChanges();
            response.ApprovalStatus = (int)ApprovalStatusEnum.Rejected;
            response.IsSuccess = true;
        }

        public static BaseResponse PayExpense(IdRequest request)
        {
            BaseResponse response = new BaseResponse();
            ExpenseDbContext entity = new ExpenseDbContext();

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
                expenseToBePaid.LastExpenseActionId = (int)StatusEnum.Completed;
                ExpenseHandlers.CreateExpenseHistory(request.ID, entity, null);
                entity.SaveChanges();
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                log.Error("Pay Expense",ex);
            }
            finally
            {
                entity.Dispose();
            }
            return response;
        }
    }
}
