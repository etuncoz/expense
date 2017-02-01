using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseApp.Data.Models;
using ExpenseApp.Engine.Domain.ViewModels;
using System.Data.Entity.Validation;

namespace ExpenseApp.Controllers
{
    public class EmployeeController : Controller
    {
        private ExpenseAppDataEntities _db;

        public EmployeeController()
        {
            _db = new ExpenseAppDataEntities();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        public ActionResult Index()
        {
            
            var expenses = _db.Expenses.ToList();

            var viewModel = new ExpenseViewModel()
            {
                Expenses = expenses
            };
             
            return View("Index",viewModel);
        }

        public ActionResult CreateExpense()
        {


            var viewModel = new ExpenseItemViewModel
            {
                //ExpenseItems = _db.ExpenseItems.Where(e=>e.ExpenseId == 1).AsEnumerable(),
                //Expense = _db.Expenses.Single(e => e.ID == 1), // set to 1 for now
                MaxExpenseItem = _db.ExpenseItems.Max(e=>e.ID),
                MaxExpense = _db.Expenses.Max(e=>e.ID)

            };  

            return View(viewModel);
        }

        //[HttpPost]
        //public ActionResult SaveExpense(ExpenseItem ExpenseItem) 
        //{
        //    if (ExpenseItem.ID == 0)
        //    {
        //        _db.ExpenseItems.Add(ExpenseItem);
        //    }
        //    else
        //    {
        //        var expenseItemInDb = _db.ExpenseItems.Single(m => m.ID == ExpenseItem.ID);

        //        expenseItemInDb.Amount = ExpenseItem.Amount;
        //        expenseItemInDb.Description = ExpenseItem.Description;
        //        expenseItemInDb.ExpenseItemDate = ExpenseItem.ExpenseItemDate;
        //    }
        //    try
        //    {
        //        _db.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Console.WriteLine(e);//log.
        //    }
        //    return View("ExpenseCreate");
        //}
    }
}