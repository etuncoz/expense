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
    public class ExpenseItemsController : ApiController
    {
        private ExpenseAppDataEntities _db;

        public ExpenseItemsController()
        {
            _db = new ExpenseAppDataEntities();
        }
        // GET /api/expenseitems
        public IHttpActionResult GetExpenseItems()
        {
            return Ok(_db.ExpenseItems.ToList().Select(Mapper.Map<ExpenseItem, ExpenseItemDto>));
        }

        // GET /api/expenseitems/1
        public IHttpActionResult GetExpenseItem(int id)
        {
            var expenseItemsWithId = _db.ExpenseItems.Where(e => e.ExpenseId == id);

            if (!expenseItemsWithId.Any())
                return NotFound();
            return Ok(expenseItemsWithId.Select(Mapper.Map<ExpenseItem, ExpenseItemDto>));
        }

        // POST /api/expenseitems
        [HttpPost]
        public IHttpActionResult CreateExpenseItem(IEnumerable<ExpenseItemDto> expenseItemsDto ) //ExpenseItem comes from the request body
        {
            if (!ModelState.IsValid)
                return BadRequest();
            foreach (var item in expenseItemsDto)
            {
                var expenseItem = Mapper.Map<ExpenseItemDto, ExpenseItem>(item);
                _db.ExpenseItems.Add(expenseItem);
                _db.SaveChanges();
            }

            //var expenseItem = Mapper.Map<ExpenseItemDto, ExpenseItem>(ExpenseItemDto);
            //_db.ExpenseItems.Add(expenseItem);
            //_db.SaveChanges();

            //ExpenseItemDto.ID = expenseItem.ID;

            return Ok();
        }

        // PUT /api/expenseitems/1
        [HttpPut]
        public IHttpActionResult UpdateExpenseItem(int id, ExpenseItemDto ExpenseItemDto) //ExpenseItem comes from the request body
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var expenseItemInDb = _db.ExpenseItems.SingleOrDefault(e => e.ID == id);

            if (expenseItemInDb == null)
                return NotFound();

            Mapper.Map(ExpenseItemDto, expenseItemInDb);

            _db.SaveChanges();

            return Ok();
        }

        // DELETE /api/expenseitems/1
        [HttpDelete]
        public IHttpActionResult DeleteExpenseItem(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();



            var expenseItemInDb = _db.ExpenseItems.SingleOrDefault(e => e.ID == id);

            if (expenseItemInDb == null)
                return NotFound();

            _db.ExpenseItems.Remove(expenseItemInDb);
            _db.SaveChanges();
            return Ok();
        }
    }
}
