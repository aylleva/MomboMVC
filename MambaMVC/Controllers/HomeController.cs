using MambaMVC.DAL;
using MambaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
         _context = context;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM vm= new HomeVM()
            {
                Employees=await _context.Employees.Include(E=>E.Position).Include(e=>e.EmployeeImages).Take(8).ToListAsync()
            };

            return View(vm);
        }
    }
}
