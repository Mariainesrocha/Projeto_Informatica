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
    [Authorize(Roles = "Admin")]
    public class ProvasController : Controller
    {
        private UserManager<User> userManager;
        private readonly ApplicationDbContextAlmostFinal _context;

        public ProvasController(ApplicationDbContextAlmostFinal context, UserManager<User> usrMgr)
        {
            userManager = usrMgr;
            _context = context;
        }

        // GET: Provas
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

            var provas = from p in _context.Provas select p;

            int pageSize = 10;
            return View(await PaginatedList<Prova>.CreateAsync(provas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Provas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prova = await _context.Provas
                .Include(p => p.IdAuthorNavigation)
                .Include(p => p.IdCompeticaoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prova == null)
            {
                return NotFound();
            }

            return View(prova);
        }

        // GET: Provas/Create
        public IActionResult Create()
        {
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta");
            ViewData["logged_id"] = userManager.GetUserId(HttpContext.User);
            return View();
        }

        // POST: Provas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAuthor,IdCompeticao,NomeProva,DataCriacao,MaxEscolas,MaxTentJogo,TempoTotalJogo,NumNiveis,VidasPorNivel,NumElemsEquipa,Calculadora,DataInscFinal,DataProva,InicioPreInscricao,FimPreInscricao,InicioInscricaoEquipas,FimInscricaoEquipas,FimProva,Estilo,Url,TreinoVisivel,RefIdCicloEnsino,Plataforma")] Prova prova)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prova);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id", prova.IdAuthor);
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta", prova.IdCompeticao);
            return View(prova);
        }

        // GET: Provas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prova = await _context.Provas.FindAsync(id);
            if (prova == null)
            {
                return NotFound();
            }
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id", prova.IdAuthor);
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta", prova.IdCompeticao);
            ViewData["logged_id"] = userManager.GetUserId(HttpContext.User);
            return View(prova);
        }

        // POST: Provas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAuthor,IdCompeticao,NomeProva,DataCriacao,MaxEscolas,MaxTentJogo,TempoTotalJogo,NumNiveis,VidasPorNivel,NumElemsEquipa,Calculadora,DataInscFinal,DataProva,InicioPreInscricao,FimPreInscricao,InicioInscricaoEquipas,FimInscricaoEquipas,FimProva,Estilo,Url,TreinoVisivel,RefIdCicloEnsino,Plataforma")] Prova prova)
        {
            if (id != prova.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prova);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvaExists(prova.Id))
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
            ViewData["IdAuthor"] = new SelectList(_context.AspNetUsers, "Id", "Id", prova.IdAuthor);
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta", prova.IdCompeticao);
            return View(prova);
        }

        // GET: Provas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prova = await _context.Provas
                .Include(p => p.IdAuthorNavigation)
                .Include(p => p.IdCompeticaoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prova == null)
            {
                return NotFound();
            }

            return View(prova);
        }

        // POST: Provas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prova = await _context.Provas.FindAsync(id);
            _context.Provas.Remove(prova);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvaExists(int id)
        {
            return _context.Provas.Any(e => e.Id == id);
        }
    }
}
