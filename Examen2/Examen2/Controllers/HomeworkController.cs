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
    public class HomeworkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeworkController(ApplicationDbContext context)
        {
            _context = context;

        }

         private readonly int RecordsPerPages = 10;

        private Pagination<Homework> PaginationHomeworks;

        // GET: Homework
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int totalRecords = 0;



            if (search == null)
            {
                search = "";
            }

            //obtener registros totales

            totalRecords = await _context.Homeworks.CountAsync(
                    d => d.Description.Contains(search));

            //Obtener datos

            var homework = await _context.Homeworks.Where(d => d.Description.Contains(search)).Include(h => h.Category).Include(h => h.Priority).ToListAsync();

            var HomeworkResult = homework.OrderBy(o => o.Description)
                .Skip((page - 1) * RecordsPerPages).Take(RecordsPerPages);

            //Obtener el total de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPages);

            //Instanciar la clase depaginación


            PaginationHomeworks = new Pagination<Homework>()
            {
                RecordsPerPage = this.RecordsPerPages,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Search = search,
                Result = HomeworkResult
            };

            return View(PaginationHomeworks);

}
        public async Task<IActionResult> TaskList()
        {
            var applicationDbContext = _context.Homeworks.Include(h => h.Category).Include(h => h.Priority);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Homework/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homework = await _context.Homeworks
                .Include(h => h.Category)
                .Include(h => h.Priority)
                .FirstOrDefaultAsync(m => m.IdHomework == id);
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // GET: Homework/Create
        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "NameCategory");
            ViewData["IdPriority"] = new SelectList(_context.Priority, "IdPriority", "PriorityDescription");
            return View();
        }

        // POST: Homework/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHomework,Description,IdPriority,IdCategory,StartDate,FinishDate")] Homework homework)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homework);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "NameCategory", homework.IdCategory);
            ViewData["IdPriority"] = new SelectList(_context.Priority, "IdPriority", "PriorityDescription", homework.IdPriority);
            return View(homework);
        }

        // GET: Homework/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "NameCategory", homework.IdCategory);
            ViewData["IdPriority"] = new SelectList(_context.Priority, "IdPriority", "PriorityDescription", homework.IdPriority);
            return View(homework);
        }

        // POST: Homework/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHomework,Description,IdPriority,IdCategory,StartDate,FinishDate")] Homework homework)
        {
            if (id != homework.IdHomework)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homework);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeworkExists(homework.IdHomework))
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
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "NameCategory", homework.IdCategory);
            ViewData["IdPriority"] = new SelectList(_context.Priority, "IdPriority", "PriorityDescription", homework.IdPriority);
            return View(homework);
        }

        // GET: Homework/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homework = await _context.Homeworks
                .Include(h => h.Category)
                .Include(h => h.Priority)
                .FirstOrDefaultAsync(m => m.IdHomework == id);
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // POST: Homework/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeworkExists(int id)
        {
            return _context.Homeworks.Any(e => e.IdHomework == id);
        }
    }
}
