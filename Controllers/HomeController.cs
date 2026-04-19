using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context )
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Expense()
        {
            var AllExpenses = _context.Expenses.ToList();
            var TotalExpenses = AllExpenses.Sum(x => x.Value);
            ViewBag.TotalExpenses = TotalExpenses;

            return View(AllExpenses);
        }
        public IActionResult CreateEditExpense(int? id)
        {
            if (id == null)
            {
                return View(new Expense());
            }
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);

            return View();
        }
        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInDb);  
            _context.SaveChanges();
            return RedirectToAction("Expense");
        }
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if(model.Id == 0)
            {
                _context.Expenses.Add(model);
            }
            else
            {
                _context.Expenses.Update(model);    

            }
            _context.SaveChanges();
            return RedirectToAction("Expense");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
