using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Request;
using ExpenseApp.Engine.Handlers;
using ExpenseApp.Engine.Enum;

namespace ExpenseApp.Api
{
    public class ExpensesController : ApiController
    {
        //Used in employee index page
        // api/expenses/GetExpenseByUserId
        [HttpPost]
        public IHttpActionResult GetExpenseByUserId(ExpenseGetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = ExpenseHandlers.GetExpenseByUserId(request.UserId);

            if (!response.IsSuccess)
                return NotFound();
            return Ok(response);
        }

        // Used in manager & accountant index pages
        // api/expenses/GetExpenseByActionId
        [HttpPost]
        public IHttpActionResult GetExpenseByActionId(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var response = ExpenseHandlers.GetExpenseByActionId(request);

            if (!response.IsSuccess)
                return NotFound();
            return Ok(response);
        }

        //Used for employees to send their expense for a manager approval
        // api/expense/SendExpenseForApproval
        public IHttpActionResult SendExpenseForApproval(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = ExpenseActionHandlers.SendExpenseForApproval(request);

            if (!response.IsSuccess)
                return NotFound();

            return Ok(response);
        }

        //Used for employees to create or update their expenses
        // api/expenses/SaveExpense
        [HttpPost]
        public IHttpActionResult SaveExpense(ExpenseSaveRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(ExpenseHandlers.SaveExpense(request));
        }

        //Used in employee's saveexpense page
        // api/expenses/GetExpenseItemByExpenseId
        [HttpPost]
        public IHttpActionResult GetExpenseItemByExpenseId(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();

            var response = ExpenseHandlers.GetExpenseItemsByExpenseId(request.ExpenseId);

            if (!response.IsSuccess) 
                return NotFound();
            return Ok(response);
        }

        // api/expenses/DeleteExpense
        [HttpPost]
        public IHttpActionResult DeleteExpense(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = ExpenseHandlers.DeleteExpenseById(request);

            if (!response.IsSuccess)
                return NotFound();
            return Ok(response);

        }

        // api/expenses/ApproveOrRejectExpense
        [HttpPost]
        public IHttpActionResult ApproveOrRejectExpense(ExpenseApprovalRequest request)  
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = ExpenseActionHandlers.ApproveOrRejectExpense(request);

            if (response.ApprovalStatus == (int)ApprovalStatusEnum.Unknown)
                return NotFound();
            return Ok(response);
        }

        // api/expenses/PayExpense
        [HttpPost]
        public IHttpActionResult PayExpense(IdRequest request)
        {
            if (request == null)
                return BadRequest();

            var response = ExpenseActionHandlers.PayExpense(request);

            if (!response.IsSuccess)
                return NotFound();
            return Ok(response);
        }

        // api/expenses/GetExpenseRejectReason
        [HttpPost]
        public IHttpActionResult GetExpenseRejectReason(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();
            var response = ExpenseHandlers.GetRejectReason(request);

            if (!response.IsSuccess)
                return NotFound();
            return Ok(response);
        }
    }
}