using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pmat_PI.Models;
using Microsoft.AspNetCore.Identity;

namespace Pmat_PI
{
    //[Authorize(Roles = "Admin")]
    public class TreinosController : Controller
    {
        private UserManager<User> userManager;
        private readonly treinoContext _context;

        public TreinosController(treinoContext context, UserManager<User> usrMgr)
        {
            userManager = usrMgr;
            _context = context;
        }

        // GET: Treinos
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var treinos = from t in _context.Treinos select t;

            int pageSize = 10;
            return View(await PaginatedList<Treino>.CreateAsync(treinos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Treinos/Create
        public IActionResult Create()
        {
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta");
            ViewData["logged_id"] = userManager.GetUserId(HttpContext.User);
            return View();
        }

        // POST: Treinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAuthor,IdCompeticao,NomeProva,DataCriacao,MaxEscolas,MaxTentJogo,TempoTotalJogo,NumNiveis,VidasPorNivel,NumElemsEquipa,Calculadora,Estilo,Url,TreinoVisivel,RefIdCicloEnsino,Plataforma")] Treino treino)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id", treino.IdAuthor);
            return View(treino);
        }

        // GET: Treinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treino = await _context.Treinos.FindAsync(id);
            if (treino == null)
            {
                return NotFound();
            }
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id", treino.IdAuthor);
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta", treino.IdCompeticao);
            ViewData["logged_id"] = userManager.GetUserId(HttpContext.User);
            return View(treino);
        }

        // POST: Treinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAuthor,IdCompeticao,NomeProva,DataCriacao,MaxEscolas,MaxTentJogo,TempoTotalJogo,NumNiveis,VidasPorNivel,NumElemsEquipa,Calculadora,Estilo,Url,TreinoVisivel,RefIdCicloEnsino,Plataforma")] Treino treino)
        {
            if (id != treino.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreinoExists(treino.Id))
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
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id", treino.IdAuthor);
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta", treino.IdCompeticao);
            return View(treino);
        }

        // GET: Treinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treino = await _context.Treinos
                .Include(t => t.IdAuthorNavigation)
                .Include(t => t.IdCompeticaoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treino == null)
            {
                return NotFound();
            }

            return View(treino);
        }

        // POST: Treinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treino = await _context.Treinos.FindAsync(id);
            _context.Treinos.Remove(treino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreinoExists(int id)
        {
            return _context.Treinos.Any(e => e.Id == id);
        }
    }
}
