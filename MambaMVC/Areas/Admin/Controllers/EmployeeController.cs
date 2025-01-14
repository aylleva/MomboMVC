using MambaMVC.Areas.ViewModels.Employee;
using MambaMVC.DAL;
using MambaMVC.Models;
using MambaMVC.Utilities.Enums;
using MambaMVC.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MambaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string roots = Path.Combine("assets", "img", "team");
        public EmployeeController(AppDBContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetEmployeeVM> vm = await _context.Employees.Include(e => e.Position).Include(e => e.EmployeeImages)
                .Select(e => new GetEmployeeVM
                {
                    Name = e.Name,
                    Image = e.EmployeeImages[0].Image,
                    PositionName = e.Position.Name,
                    Id = e.Id

                }).ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM vm = new CreateEmployeeVM() { Positions = await _context.Positions.ToListAsync() };
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeevm)
        {
            employeevm.Positions = await _context.Positions.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(employeevm);
            }

            if (!employeevm.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "Wrong Type");
                return View(employeevm);
            }
            if (!employeevm.Photo.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "Wrong Size");
                return View(employeevm);
            }

            bool result = await _context.Positions.AnyAsync(p => p.Id == employeevm.PositionId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.PositionId), "Position does not exits");
                return View(employeevm);
            }

            EmployeeImage image = new EmployeeImage()
            {
                Image = await employeevm.Photo.CreateFileAsync(_env.WebRootPath,roots)

            };

            Employee employee = new Employee()
            {
                Name = employeevm.Name,
                PositionId = employeevm.PositionId,
                EmployeeImages=new List<EmployeeImage> { image }
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == Id);
            if (employee == null) return NotFound();

            UpdateEmployeeVM vM = new UpdateEmployeeVM()
            {
                Name = employee.Name,
                PositionId = employee.PositionId,
                Positions = await _context.Positions.ToListAsync(),

                Images = employee.EmployeeImages
            };
            return View(vM);


        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeVM employeevm, int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Employee? existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == Id);
            if (existed == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(employeevm);
            }

            if(employeevm.Photo is not null)
            {
                if (!employeevm.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Photo), "Wrong Type");
                    return View(employeevm);
                }
                if (!employeevm.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Photo), "Wrong Size");
                    return View(employeevm);
                }
            }

            if (employeevm.PositionId != existed.PositionId)
            {
                bool result=await _context.Employees.AnyAsync(e=>e.Id == existed.Id);
                if(!result)
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.PositionId), "Position does not exits");
                    return View(employeevm);
                }
            }

            if(employeevm.Photo is not null)
            {
                string file = await employeevm.Photo.CreateFileAsync(_env.WebRootPath,roots);
                
                EmployeeImage image=existed.EmployeeImages.FirstOrDefault();
                image.Image.DeleteAsync(_env.WebRootPath, roots);
                existed.EmployeeImages.Remove(image);


                existed.EmployeeImages.Add(new EmployeeImage
                {
                    Image = file
                });
            }

            existed.Name= employeevm.Name;
            existed.PositionId= existed.PositionId;
             
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == Id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
