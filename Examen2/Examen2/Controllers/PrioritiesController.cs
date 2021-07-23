using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen2.Data;
using Examen2.Models;
using Examen2.Common;

namespace Examen2.Controllers
{
    public class PrioritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrioritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Priorities
        private readonly int RecordsPerPages = 10;

        private Pagination<Priority> PaginationPriories;
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int totalRecords = 0;



            if (search == null)
            {
                search = "";
            }
            totalRecords = await _context.Homeworks.CountAsync(
                    d => d.Description.Contains(search));

            //Obtener datos

            var Priority = await _context.Priority.Where(d => d.PriorityDescription.Contains(search)).ToListAsync();

            var PriorityResult = Priority.OrderBy(o => o.PriorityDescription)
                .Skip((page - 1) * RecordsPerPages).Take(RecordsPerPages);//Obtener el total de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPages);

            //Instanciar la clase depaginación


            PaginationPriories = new Pagination<Priority>()
            {
                RecordsPerPage = this.RecordsPerPages,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Search = search,
                Result = PriorityResult
            };

            return View(PaginationPriories);
        }

        // GET: Priorities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priority = await _context.Priority
                .FirstOrDefaultAsync(m => m.IdPriority == id);
            if (priority == null)
            {
                return NotFound();
            }

            return View(priority);
        }

        // GET: Priorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Priorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPriority,PriorityDescription")] Priority priority)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priority);
        }

        // GET: Priorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priority = await _context.Priority.FindAsync(id);
            if (priority == null)
            {
                return NotFound();
            }
            return View(priority);
        }

        // POST: Priorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPriority,PriorityDescription")] Priority priority)
        {
            if (id != priority.IdPriority)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriorityExists(priority.IdPriority))
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
            return View(priority);
        }

        // GET: Priorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priority = await _context.Priority
                .FirstOrDefaultAsync(m => m.IdPriority == id);
            if (priority == null)
            {
                return NotFound();
            }

            return View(priority);
        }

        // POST: Priorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priority = await _context.Priority.FindAsync(id);
            _context.Priority.Remove(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriorityExists(int id)
        {
            return _context.Priority.Any(e => e.IdPriority == id);
        }
    }
}
