using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Request;
using ExpenseApp.Engine.Handlers;

namespace ExpenseApp.Api
{
    public class ExpensesController : ApiController
    {
        // POST /api/expenses/getExpenseByUserId
        [System.Web.Http.HttpPost]
        public IHttpActionResult GetExpenseByUserId(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();
            return Ok(ExpenseHandlers.GetExpenseByUserId(request.UserId));
        }

        //POST /api/expenses/saveexpense
        [System.Web.Http.HttpPost]
        public IHttpActionResult SaveExpense(ExpenseSaveRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            ExpenseHandlers.SaveExpense(request.ExpenseItemsDto,request.ExpenseId);
            return Ok();
        }

        //POST /api/expenses/getExpenseItemByExpenseId
        [System.Web.Http.HttpPost]
        public IHttpActionResult GetExpenseItemByExpenseId(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();
            return Ok(ExpenseHandlers.GetExpenseItemsByExpenseId(request.ExpenseId));
        }

        // DELETE /api/expenses/deleteexpense
        [System.Web.Http.HttpPost]
        public IHttpActionResult DeleteExpense(IdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!ExpenseHandlers.DeleteExpenseById(request.ID).IsSuccess)
                return NotFound();
            return Ok();

        }
    }
}
