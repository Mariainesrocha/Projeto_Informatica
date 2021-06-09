using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pmat_PI.Models;

namespace Pmat_PI
{
    public class ProvaEquipaEnunciadosController : Controller
    {
        private readonly ApplicationDbContextAlmostFinal _context;

        public ProvaEquipaEnunciadosController(ApplicationDbContextAlmostFinal context)
        {
            _context = context;
        }

        // GET: ProvaEquipaEnunciadoes
        public async Task<IActionResult> Index(string searchProva, string searchEnunciado, string searchEquipa, int? pageNumber)
        {
            ViewData["searchProva"] = searchProva;
            ViewData["searchEnunciado"] = searchEnunciado;
            ViewData["searchEquipa"] = searchEquipa;

            IQueryable<ProvaEquipaEnunciado> enunciados = _context.ProvaEquipaEnunciados.Include(p => p.IdEquipaNavigation).Include(p => p.IdProvaNavigation);

            if (!String.IsNullOrEmpty(searchProva))
            {
                enunciados = enunciados.Where(e => e.IdProva.ToString().Equals(searchProva));
            }

            if (!String.IsNullOrEmpty(searchEnunciado))
            {
                enunciados = enunciados.Where(e => e.Id.ToString().Equals(searchEnunciado));
            }

            if (!String.IsNullOrEmpty(searchEquipa))
            {
                enunciados = enunciados.Where(e => e.IdEquipa.ToString().Equals(searchEquipa));
            }

            int pageSize = 30;
        
            return View(await PaginatedList<ProvaEquipaEnunciado>.CreateAsync(enunciados.AsNoTracking(), pageNumber ?? 1, pageSize) );
        }

        // GET: ProvaEquipaEnunciadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provaEquipaEnunciado = await _context.ProvaEquipaEnunciados
                .Include(p => p.IdEquipaNavigation)
                .Include(p => p.IdProvaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provaEquipaEnunciado == null)
            {
                return NotFound();
            }

            return View(provaEquipaEnunciado);
        }

        // GET: ProvaEquipaEnunciadoes/Create
        public IActionResult Create()
        {
            ViewData["IdEquipa"] = new SelectList(_context.Equipas, "Id", "Id");
            ViewData["IdProva"] = new SelectList(_context.Provas, "Id", "Id");
            return View();
        }

        // POST: ProvaEquipaEnunciadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdEquipa,IdProva,Data,Status,UltimoNivel,Tempo")] ProvaEquipaEnunciado provaEquipaEnunciado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(provaEquipaEnunciado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEquipa"] = new SelectList(_context.Equipas, "Id", "Id", provaEquipaEnunciado.IdEquipa);
            ViewData["IdProva"] = new SelectList(_context.Provas, "Id", "Id", provaEquipaEnunciado.IdProva);
            return View(provaEquipaEnunciado);
        }

        // GET: ProvaEquipaEnunciadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provaEquipaEnunciado = await _context.ProvaEquipaEnunciados.FindAsync(id);
            if (provaEquipaEnunciado == null)
            {
                return NotFound();
            }
            ViewData["IdEquipa"] = new SelectList(_context.Equipas, "Id", "Id", provaEquipaEnunciado.IdEquipa);
            ViewData["IdProva"] = new SelectList(_context.Provas, "Id", "Id", provaEquipaEnunciado.IdProva);
            return View(provaEquipaEnunciado);
        }

        // POST: ProvaEquipaEnunciadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEquipa,IdProva,Data,Status,UltimoNivel,Tempo")] ProvaEquipaEnunciado provaEquipaEnunciado)
        {
            if (id != provaEquipaEnunciado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(provaEquipaEnunciado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvaEquipaEnunciadoExists(provaEquipaEnunciado.Id))
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
            ViewData["IdEquipa"] = new SelectList(_context.Equipas, "Id", "Id", provaEquipaEnunciado.IdEquipa);
            ViewData["IdProva"] = new SelectList(_context.Provas, "Id", "Id", provaEquipaEnunciado.IdProva);
            return View(provaEquipaEnunciado);
        }

        // GET: ProvaEquipaEnunciadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provaEquipaEnunciado = await _context.ProvaEquipaEnunciados
                .Include(p => p.IdEquipaNavigation)
                .Include(p => p.IdProvaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provaEquipaEnunciado == null)
            {
                return NotFound();
            }

            return View(provaEquipaEnunciado);
        }

        // POST: ProvaEquipaEnunciadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var provaEquipaEnunciado = await _context.ProvaEquipaEnunciados.FindAsync(id);
            _context.ProvaEquipaEnunciados.Remove(provaEquipaEnunciado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvaEquipaEnunciadoExists(int id)
        {
            return _context.ProvaEquipaEnunciados.Any(e => e.Id == id);
        }
    }
}
