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
    public class EmployeesController : Controller
    {
        private readonly DepartmentManagerContext _context;

        public EmployeesController(DepartmentManagerContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int page = 1, int limit = 5)
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            /*var departmentManagerContext = _context.Employee.Include(e => e.department);
            return View(await departmentManagerContext.ToListAsync());*/

            var size = await _context.Employee.CountAsync();
            var employees = await _context.Employee
                                .Include(d => d.department)
                                .OrderBy(d => d.Fullname)
                                .Skip(limit * (page - 1)).Take(limit)
                                .ToListAsync();

            ViewBag.Pages = ((int)size / limit) + 1;
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentName"] = new SelectList(_context.Department, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,Age,Salary,DepartmentId")] Employee employee)
        {
            employee.Created = DateTime.Now;
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /*if (ModelState.IsValid)
            {
                
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", employee.DepartmentId);
            return View(employee);*/
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentName"] = new SelectList(_context.Department, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fullname,Age,Salary,Modified,DepartmentId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            try
            {
                employee.Modified = DateTime.Now;
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            /*ViewData["DepartmentName"] = new SelectList(_context.Department, "Id", "Name", employee.DepartmentId);
            return View(employee);*/
        }

        // GET: Employees/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine(id);
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            *//*var employee = await _context.Employee
                .Include(e => e.department)
                .FirstOrDefaultAsync(m => m.Id == id);*//*

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }*/

        // POST: Employees/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return Json(new { employee });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItems(int[]? ids)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'DepartmentManagerContext.Employee'  is null.");
            }
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var employee = await _context.Employee.FindAsync(id);
                    if (employee != null)
                    {
                        _context.Employee.Remove(employee);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return Json(ids);
        }
        private bool EmployeeExists(int id)
        {
          return (_context.Employee?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
