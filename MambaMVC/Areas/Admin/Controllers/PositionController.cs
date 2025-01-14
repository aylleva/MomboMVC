using MambaMVC.Areas.ViewModels;
using MambaMVC.DAL;
using MambaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDBContext _context;

        public PositionController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
          List<GetPositionVM> vm=await _context.Positions.Include(p=>p.Employees)
                .Select(p=> new GetPositionVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    EmployeeCount=p.Employees.Count,
                }).ToListAsync();
            return View(vm);    
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            bool result=await _context.Positions.AnyAsync(p=>p.Name == vm.Name);
            if(result)
            {
                ModelState.AddModelError(nameof(CreatePositionVM.Name), "This Position is already exists");
                return View(vm);
            }
            Position position  = new Position()
            {
                Name= vm.Name,
            };
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Position? position = await _context.Positions.FirstOrDefaultAsync(p=>p.Id == Id);
            if(position == null) return NotFound();

            UpdatePositionVM vm=new UpdatePositionVM() { Name=position.Name};
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePositionVM vm,int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Position? existed = await _context.Positions.FirstOrDefaultAsync(p => p.Id == Id);
            if (existed == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool result=await _context.Positions.AnyAsync(p=>p.Name.Trim()==vm.Name.Trim() && p.Id!=Id);
            if (result)
            {
                ModelState.AddModelError(nameof(UpdatePositionVM.Name), "This Position is already exists");
                return View(vm);
            }

            existed.Name = vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Position? position = await _context.Positions.FirstOrDefaultAsync(e => e.Id == Id);
            if (position == null) return NotFound();

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
