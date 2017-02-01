using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseApp.Data.Models;
using System.Data.Entity;
using AutoMapper;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp.Api
{
    public class ExpensesController : ApiController
    {
        private ExpenseAppDataEntities _db;

        public ExpensesController()
        {
            _db = new ExpenseAppDataEntities();
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
        public IHttpActionResult CreateExpense(ExpenseDto ExpenseDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //-----------BU KISIM HANDLERDA OLACAK-------------
            var expense = Mapper.Map<ExpenseDto, Expense>(ExpenseDto);
            _db.Expenses.Add(expense);
            _db.SaveChanges();
            ExpenseDto.ID = expense.ID;
            //-------------------------------
            //return Created(new Uri(Request.RequestUri + "/" + ExpenseDto.ID),ExpenseDto); //yeni bir linke yönlendirilecekse
            return Ok();
        }

        // PUT /api/expenses/1
        [HttpPut]
        public IHttpActionResult UpdateExpense (fooClass request)//(int id, ExpenseDto ExpenseDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var expenseInDb = _db.ExpenseItems.SingleOrDefault(e => e.ID == id);

            if (expenseInDb == null)
                return NotFound();

            Mapper.Map(ExpenseDto, expenseInDb);

            _db.SaveChanges();

            return Ok();
        }

        // DELETE /api/expenses/1
        [HttpDelete]
        public IHttpActionResult DeleteExpense(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();



            var expenseInDb = _db.Expenses.SingleOrDefault(e => e.ID == id);

            if (expenseInDb == null)
                return NotFound();

            _db.Expenses.Remove(expenseInDb);
            _db.SaveChanges();
            return Ok();
        }
    }
}
