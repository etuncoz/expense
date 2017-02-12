using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Request;
using ExpenseApp.Engine.Handlers;

namespace ExpenseApp.Api
{
    public class ExpensesController : ApiController
    {
        // api/expenses/GetExpenseByUserId
        [HttpPost]
        public IHttpActionResult GetExpenseByUserId(ExpenseGetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!ExpenseHandlers.GetExpenseByUserId(request.UserId).IsSuccess)
                return NotFound();
            return Ok(ExpenseHandlers.GetExpenseByUserId(request.UserId));
        }
        // api/expenses/GetExpenseByActionId
        [HttpPost]
        public IHttpActionResult GetExpenseByActionId(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if(!ExpenseHandlers.GetExpenseByActionId(request).IsSuccess)
                return NotFound();
            return Ok(ExpenseHandlers.GetExpenseByActionId(request));
        }

        // api/expense/SendExpenseForApproval
        public IHttpActionResult SendExpenseForApproval(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!ExpenseActionHandlers.SendExpenseForApproval(request).IsSuccess)
                return NotFound();
           return Ok(ExpenseActionHandlers.SendExpenseForApproval(request));
        }

        // api/expenses/SaveExpense
        [HttpPost]
        public IHttpActionResult SaveExpense(ExpenseSaveRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(ExpenseHandlers.SaveExpense(request));
        }

        // api/expenses/GetExpenseItemByExpenseId
        [HttpPost]
        public IHttpActionResult GetExpenseItemByExpenseId(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();
            if (!ExpenseHandlers.GetExpenseItemsByExpenseId(request.ExpenseId).IsSuccess)
                return NotFound();
            return Ok(ExpenseHandlers.GetExpenseItemsByExpenseId(request.ExpenseId));
        }

        // api/expenses/DeleteExpense
        [HttpPost]
        public IHttpActionResult DeleteExpense(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!ExpenseHandlers.DeleteExpenseById(request).IsSuccess)
                return NotFound();
            return Ok(ExpenseHandlers.DeleteExpenseById(request));

        }

        // api/expenses/ApproveOrRejectExpense
        [HttpPost]
        public IHttpActionResult ApproveOrRejectExpense(ExpenseApprovalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!ExpenseActionHandlers.ApproveOrRejectExpense(request).IsSuccess)
                return NotFound();
            return Ok(ExpenseActionHandlers.ApproveOrRejectExpense(request));
        }

        // api/expenses/GetCurrentExpenseStatus
        //[HttpPost]
        //public IHttpActionResult GetCurrentExpenseStatus(IdRequest request)
        //{
        //    if (request == null)
        //        return BadRequest();
        //    if (!ExpenseActionHandlers.GetCurrentExpenseStatus(request).IsSuccess)
        //        return NotFound();
        //    return Ok(ExpenseActionHandlers.GetCurrentExpenseStatus(request));
        //}

        // api/expenses/PayExpense
        [HttpPost]
        public IHttpActionResult PayExpense(IdRequest request)
        {
            if (request == null)
                return BadRequest();
            if (!ExpenseActionHandlers.PayExpense(request).IsSuccess)
                return NotFound();
            return Ok(ExpenseActionHandlers.PayExpense(request));
        }
    }
}
