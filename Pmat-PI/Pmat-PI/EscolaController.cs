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
    public class EscolaController : Controller
    {
        private readonly EscolasContext _context;

        public EscolaController(EscolasContext context)
        {
            _context = context;
        }

        // GET: Escola
        public async Task<IActionResult> Index()
        {
            var escolasContext = _context.Escolas.Include(e => e.IdTipoEscolaNavigation).Include(e => e.IdconcelhoNavigation).AsNoTracking();
            return View(await escolasContext.ToListAsync());
        }

        // GET: Escola/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escola = await _context.Escolas
                .Include(e => e.IdTipoEscolaNavigation)
                .Include(e => e.IdconcelhoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (escola == null)
            {
                return NotFound();
            }

            return View(escola);
        }

        // GET: Escola/Create
        public IActionResult Create()
        {
            ViewData["IdTipoEscola"] = new SelectList(_context.TipoEscolas, "IdTipoEscola", "IdTipoEscola");
            ViewData["Idconcelho"] = new SelectList(_context.Concelhos, "Id", "Id");
            return View();
        }

        // POST: Escola/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdTipoEscola,NomeEscola,Morada,CodigoPostal,ExtensaoCodPostal,Localidade,Telefone,Fax,Email,Website,Idconcelho,IdFreguesia,Estado,Ensinos,Latitude,Longitude,Gruponatureza,CodDgeec,CodDgpgf")] Escola escola)
        {
            if (ModelState.IsValid)
            {
                _context.Add(escola);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipoEscola"] = new SelectList(_context.TipoEscolas, "IdTipoEscola", "IdTipoEscola", escola.IdTipoEscola);
            ViewData["Idconcelho"] = new SelectList(_context.Concelhos, "Id", "Id", escola.Idconcelho);
            return View(escola);
        }

        // GET: Escola/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escola = await _context.Escolas.FindAsync(id);
            if (escola == null)
            {
                return NotFound();
            }
            ViewData["IdTipoEscola"] = new SelectList(_context.TipoEscolas, "IdTipoEscola", "IdTipoEscola", escola.IdTipoEscola);
            ViewData["Idconcelho"] = new SelectList(_context.Concelhos, "Id", "Id", escola.Idconcelho);
            return View(escola);
        }

        // POST: Escola/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdTipoEscola,NomeEscola,Morada,CodigoPostal,ExtensaoCodPostal,Localidade,Telefone,Fax,Email,Website,Idconcelho,IdFreguesia,Estado,Ensinos,Latitude,Longitude,Gruponatureza,CodDgeec,CodDgpgf")] Escola escola)
        {
            if (id != escola.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(escola);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscolaExists(escola.Id))
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
            ViewData["IdTipoEscola"] = new SelectList(_context.TipoEscolas, "IdTipoEscola", "IdTipoEscola", escola.IdTipoEscola);
            ViewData["Idconcelho"] = new SelectList(_context.Concelhos, "Id", "Id", escola.Idconcelho);
            return View(escola);
        }

        // GET: Escola/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escola = await _context.Escolas
                .Include(e => e.IdTipoEscolaNavigation)
                .Include(e => e.IdconcelhoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (escola == null)
            {
                return NotFound();
            }

            return View(escola);
        }

        // POST: Escola/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var escola = await _context.Escolas.FindAsync(id);
            _context.Escolas.Remove(escola);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscolaExists(int id)
        {
            return _context.Escolas.Any(e => e.Id == id);
        }
    }
}
