using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Web.Filter;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    [TypeFilter(typeof(AccessLogFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class TodoController : Controller
    {
        private readonly ToDoDbContext _context;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ToDoDbContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Todos.ToListAsync());
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,Priority,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,DueDate")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Description,IsCompleted,Priority,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,DueDate")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
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
            return View(todo);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(long id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ExportCsv()
        {
            var list = await _context.Todos.ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine($"\"Title\",\"Description\",\"completeStatus\",\"Priority\",\"CreatedAt\",\"DueDate\"");

            foreach (var item in list)
            {
                var completeStatus = item.IsCompleted ? "完了" : "未完了";
                sb.AppendLine($"\"{item.Title}\",\"{item.Description}\",\"{completeStatus}\",\"{item.Priority}\",\"{item.CreatedAt}\",\"{item.DueDate}\"");
            }

            string fileName = $"Task_{DateTime.Now:yyyyMMddHHmmss}.csv";

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
        }

        public async Task<IActionResult> ExportCsvByCsvHelper()
        {
            var list = await _context.Todos.ToListAsync();

            using var memory = new MemoryStream();
            using var writer = new StreamWriter(memory);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                ShouldQuote = (context) => true,
            };
            using var csv = new CsvWriter(writer, config);

            csv.WriteRecords(list);
            writer.Flush();

            string fileName = $"Task_{DateTime.Now:yyyyMMddHHmmss}.csv";

            return File(memory.ToArray(), "text/csv", fileName);
        }
    }
}
