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
    public class TreinosController : Controller
    {
        private UserManager<User> userManager;
        private readonly ApplicationDbContextAlmostFinal _context;

        public TreinosController(ApplicationDbContextAlmostFinal context, UserManager<User> usrMgr)
        {
            userManager = usrMgr;
            _context = context;
        }

        // GET: Treinos
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            var treinos = from t in _context.Treinos.Include(t => t.RefIdCicloEnsinoNavigation) select t;
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
                treinos = treinos.Where(t => t.NomeProva.ToLower().Contains(searchString.ToLower()));
            }

            int pageSize = 25;
            return View(await PaginatedList<Treino>.CreateAsync(treinos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var treino = await _context.Treinos
                .Include(t => t.RefIdCicloEnsinoNavigation).Include(u => u.IdAuthorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (treino == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }
            return View(treino);
        }

        // GET: Treinos/Create
        public IActionResult Create()
        {
            ViewData["RefIdCicloEnsino"] = new SelectList(_context.CicloEnsinos, "Id", "Descritivo");
            ViewData["IdCompeticao"] = new SelectList(_context.Competicaos, "Id", "Etiqueta");
            return View();
        }

        // POST: Treinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProva,DataCriacao,MaxEscolas,MaxTentJogo,TempoTotalJogo,NumNiveis,VidasPorNivel,NumElemsEquipa,Calculadora,Estilo,Url,TreinoVisivel,RefIdCicloEnsino,Plataforma")] Treino treino)
        {
            if (ModelState.IsValid)
            {
                treino.IdAuthor = userManager.GetUserId(HttpContext.User);
                _context.Add(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treino);
        }

        // GET: Treinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var treino = await _context.Treinos.FindAsync(id);
            if (treino == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }
            ViewData["RefIdCicloEnsino"] = new SelectList(_context.CicloEnsinos, "Id", "Descritivo", treino.RefIdCicloEnsino);
            return View(treino);
        }

        // POST: Treinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeProva,DataCriacao,MaxEscolas,MaxTentJogo,TempoTotalJogo,NumNiveis,VidasPorNivel,NumElemsEquipa,Calculadora,Estilo,Url,TreinoVisivel,RefIdCicloEnsino,Plataforma")] Treino treino)
        {
            if (id != treino.Id)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    treino.IdAuthor = userManager.GetUserId(HttpContext.User);
                    _context.Update(treino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreinoExists(treino.Id))
                    {
                        Response.StatusCode = 404;
                        return View(nameof(NotFound));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RefIdCicloEnsino"] = new SelectList(_context.CicloEnsinos, "Id", "Descritivo", treino.RefIdCicloEnsino);

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