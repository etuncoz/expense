using System.Web.Mvc;
using ExpenseApp.Data;
using ExpenseApp.Engine.Domain.ViewModels;

namespace ExpenseApp.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateExpense()
        {
            return View(); 
        }

        public ActionResult EditExpense(int id)
        {
            var viewModel = new ExpenseViewModel
            {
                ID = id
            };
            return View(viewModel);
        }
    }
}