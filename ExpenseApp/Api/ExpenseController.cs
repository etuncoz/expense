using System.Collections.Generic;
using System.Linq;
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
        // GET /api/expenses
        //public IHttpActionResult GetExpenses()
        //{
        //    return Ok(_db.Expenses.ToList().Select(Mapper.Map<Expense, ExpenseDto>));
        //}


        // POST /api/expenses/getExpenseByUserId
        [HttpPost]
        public IHttpActionResult GetExpenseByUserId(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();
            return Ok(ExpenseHandlers.GetExpenseByUserId(request.UserId));
        }

        // POST /api/expenses/createexpense
        [HttpPost]
        public IHttpActionResult CreateExpense(ExpenseCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            ExpenseHandlers.CreateExpense(request.ExpenseItemsDto);

            return Ok();
        }

        //POST /api/expenses/getExpenseItemByExpenseId
        [HttpPost]
        public IHttpActionResult GetExpenseItemByExpenseId(ExpenseGetRequest request)
        {
            if (request == null)
                return BadRequest();
            return Ok(ExpenseHandlers.GetExpenseItemsByExpenseId(request.ExpenseId));
        }

        // PUT /api/expenses/1
        //[HttpPut]
        //public IHttpActionResult UpdateExpense (fooClass request)//(int id, ExpenseDto ExpenseDto) 
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var expenseInDb = _db.ExpenseItems.SingleOrDefault(e => e.ID == id);

        //    if (expenseInDb == null)
        //        return NotFound();

        //    Mapper.Map(ExpenseDto, expenseInDb);

        //    _db.SaveChanges();

        //    return Ok();
        //}

        // DELETE /api/expenses/1
        [HttpDelete]
        public IHttpActionResult DeleteExpense(ExpenseGetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!ExpenseHandlers.DeleteExpenseById(request.ExpenseId).IsSuccess)
                return NotFound();
            return Ok();

        }
    }
}
