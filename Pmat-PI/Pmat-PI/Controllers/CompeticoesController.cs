using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pmat_PI.Models;

namespace Pmat_PI.Views
{
    public class CompeticoesController : Controller
    {
        private readonly ApplicationDbContextAlmostFinal _context;

        public CompeticoesController(ApplicationDbContextAlmostFinal context)
        {
            _context = context;
        }

        // GET: Competicoes
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            var comps = from c in _context.Competicaos select c;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                comps = comps.Where(t => t.Nome.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 25;
            return View(await PaginatedList<Competicao>.CreateAsync(comps.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Competicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competicao = await _context.Competicaos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competicao == null)
            {
                return NotFound();
            }

            return View(competicao);
        }

        // GET: Competicoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Competicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataInicio,DataFim,Etiqueta")] Competicao competicao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(competicao);
        }

        // GET: Competicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competicao = await _context.Competicaos.FindAsync(id);
            if (competicao == null)
            {
                return NotFound();
            }
            return View(competicao);
        }

        // POST: Competicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataInicio,DataFim,Etiqueta")] Competicao competicao)
        {
            if (id != competicao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompeticaoExists(competicao.Id))
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
            return View(competicao);
        }

        // POST: Competicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competicao = await _context.Competicaos.FindAsync(id);
            _context.Competicaos.Remove(competicao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompeticaoExists(int id)
        {
            return _context.Competicaos.Any(e => e.Id == id);
        }
    }
}
