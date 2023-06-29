using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DepartmentManager.Data;
using DepartmentManager.Models;

namespace DepartmentManager.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DepartmentManagerContext _context;

        public DepartmentsController(DepartmentManagerContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int page = 1, int limit = 5)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Department'  is null.");
            }
            var size = await _context.Department.CountAsync();
            var departments = await _context.Department
                                .Include(d => d.employees)
                                .OrderBy(d => d.Name)
                                .Skip(limit*(page-1)).Take(limit)
                                .ToListAsync();

            ViewBag.Pages = ((int) size / limit) + 1;         
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {
            department.Created = DateTime.Now;
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    department.Modified = DateTime.Now;
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }*/

        // POST: Departments/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Department'  is null.");
            }
            var department = await _context.Department.FindAsync(id);
            if (department != null)
            {
                _context.Department.Remove(department);
                await _context.SaveChangesAsync();
            }

            return Json(new { department });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItems(int[]? ids)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Department'  is null.");
            }
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var department = await _context.Department.FindAsync(id);
                    if (department != null)
                    {
                        _context.Department.Remove(department);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return Json(ids);
        }

        [HttpGet("/Departments/{departmentId}/Members")]
        public IActionResult MemberDepartment([FromRoute(Name = "departmentId")] int? id)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Department'  is null.");
            }

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            /*var employees = from e in _context.Employee
                            where e.DepartmentId == id
                            select e;*/
            /*var department = _context.Department.Find(id);
            ViewBag.Department = department;*/
            var department = _context.Department
                            .Where(d => d.Id == id)
                            .Include(d1 => d1.employees).FirstOrDefault();

            return View(department);
        }

        /*[HttpGet("/Departments?page={page}&limit={limit}")]
        public async Task<IActionResult> Pagination(int page = 1, int limit = 5)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Department'  is null.");
            }

            return RedirectToAction(nameof(Index));
        }*/

        private bool DepartmentExists(int id)
        {
          return (_context.Department?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
