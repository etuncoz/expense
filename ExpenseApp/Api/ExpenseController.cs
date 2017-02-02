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
        private ExpenseAppEntities _db;

        public ExpensesController()
        {
            _db = new ExpenseAppEntities();
        }

        // GET /api/expenses
        public IHttpActionResult GetExpenses()
        {
            return Ok(_db.Expenses.ToList().Select(Mapper.Map<Expense, ExpenseDto>));
        }

        // GET /api/expenses/1
        public IHttpActionResult GetExpense(int id)
        {   
            //UserId'ye göre GET ??
            return Ok();
        }

        // POST /api/expenses
        [HttpPost]
        public IHttpActionResult CreateExpense(ExpenseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            ExpenseHandlers.CreateExpenseByObj(request.ExpenseItemsDto);

            return Ok();
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
        public IHttpActionResult DeleteExpense(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if(!ExpenseHandlers.DeleteById(id))
                return NotFound();
            return Ok();

        }
    }
}
