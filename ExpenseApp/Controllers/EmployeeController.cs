using System.Web.Mvc;
using ExpenseApp.Data;

namespace ExpenseApp.Controllers
{
    public class EmployeeController : Controller
    {
        private ExpenseAppEntities _db;

        public EmployeeController()
        {
            _db = new ExpenseAppEntities();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        //public ActionResult Index()
        //{
            
        //    var expenses = _db.Expenses.ToList();

        //    var viewModel = new ExpenseViewModel()
        //    {
        //        Expenses = expenses
        //    };
             
        //    return View("Index",viewModel);
        //}

        public ActionResult CreateExpense()
        {
            return View(); 
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