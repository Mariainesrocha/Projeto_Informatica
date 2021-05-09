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
    public class TipoEscolasController : Controller
    {
        private readonly EscolasContext _context;

        public TipoEscolasController(EscolasContext context)
        {
            _context = context;
        }

        // GET: TipoEscolas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoEscolas.ToListAsync());
        }

        // GET: TipoEscolas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEscola = await _context.TipoEscolas
                .FirstOrDefaultAsync(m => m.IdTipoEscola == id);
            if (tipoEscola == null)
            {
                return NotFound();
            }

            return View(tipoEscola);
        }

        // GET: TipoEscolas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoEscolas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoEscola,TipoEscola1,DescricaoTipoEscola")] TipoEscola tipoEscola)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoEscola);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoEscola);
        }

        // GET: TipoEscolas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEscola = await _context.TipoEscolas.FindAsync(id);
            if (tipoEscola == null)
            {
                return NotFound();
            }
            return View(tipoEscola);
        }

        // POST: TipoEscolas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoEscola,TipoEscola1,DescricaoTipoEscola")] TipoEscola tipoEscola)
        {
            if (id != tipoEscola.IdTipoEscola)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoEscola);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoEscolaExists(tipoEscola.IdTipoEscola))
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
            return View(tipoEscola);
        }

        // GET: TipoEscolas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEscola = await _context.TipoEscolas
                .FirstOrDefaultAsync(m => m.IdTipoEscola == id);
            if (tipoEscola == null)
            {
                return NotFound();
            }

            return View(tipoEscola);
        }

        // POST: TipoEscolas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoEscola = await _context.TipoEscolas.FindAsync(id);
            _context.TipoEscolas.Remove(tipoEscola);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoEscolaExists(int id)
        {
            return _context.TipoEscolas.Any(e => e.IdTipoEscola == id);
        }
    }
}
