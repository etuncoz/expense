using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Request;

namespace ExpenseApp.Api
{
    public class ExpenseItemsController : ApiController
    {
        private ExpenseAppEntities _db;

        public ExpenseItemsController()
        {
            _db = new ExpenseAppEntities();
        }
        // GET /api/expenseitems
        public IHttpActionResult GetExpenseItems()
        {
            return Ok(_db.ExpenseItems.ToList().Select(Mapper.Map<ExpenseItem, ExpenseItemDto>));
        }

        // POST /api/expenseitems
        [HttpPost]
        public IHttpActionResult CreateExpenseItem(IEnumerable<ExpenseItemDto> expenseItemsDto) //ExpenseItem comes from the request body
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
