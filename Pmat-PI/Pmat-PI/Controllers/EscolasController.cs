using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pmat_PI.Models;

namespace Pmat_PI
{
    [Authorize(Roles = "Admin")]
    public class EscolasController : Controller
    {
        private readonly ApplicationDbContextAlmostFinal _context;

        public EscolasController(ApplicationDbContextAlmostFinal context)
        {
            _context = context;
        }

        // GET: Escola
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NomeSortParm"] = sortOrder == "Nome de Escola" ? "nome_desc" : "nome_asc";
            ViewData["LocalidadeSortParm"] = sortOrder == "Localidade" ? "localidade_desc" : "localidade_asc";

            var escolas = from e in _context.Escolas.Include(e => e.IdTipoEscolaNavigation).Include(e => e.IdconcelhoNavigation) select e;
          
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
                escolas = escolas.Where(e => e.NomeEscola.Contains(searchString));
            }

            //TODO: REFAZER PARA OS TREINOS PQ AQUI N FAZ SENTIDO E NOT WORKING
            switch (sortOrder)
            {
                case "nome_desc":
                    escolas = escolas.OrderByDescending(e => e.NomeEscola);
                    break;
                case "nome_asc":
                    escolas = escolas.OrderBy(e => e.NomeEscola);
                    break;
                case "localidade_desc":
                    escolas = escolas.OrderByDescending(e => e.Localidade);
                    break;
                case "localidade_asc":
                    escolas = escolas.OrderBy(e => e.Localidade);
                    break;
                default:
                    escolas = escolas.OrderBy(e => e.Id);
                    break;
            }

            int pageSize = 50;

            return View(await PaginatedList<Escola>.CreateAsync(escolas.AsNoTracking(), pageNumber ?? 1, pageSize));
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
                .Include(e => e.IdconcelhoNavigation).AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (escola.IdFreguesia.HasValue) {
                ViewBag.freguesia = _context.Freguesia.Where(f => f.Id == escola.IdFreguesia).First();
            }
            if (escola == null)
            {
                return NotFound();
            }

            return View(escola);
        }

        // GET: Escola/Create
        public IActionResult Create()
        {
            PopulateDropDownList();
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
                TempData["successMessage"] = "Escola " + escola.NomeEscola + " criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownList(escola.IdconcelhoNavigation, escola.IdTipoEscolaNavigation);
            return View(escola);
        }

        // GET: Escola/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escola = await _context.Escolas.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (escola == null)
            {
                return NotFound();
            }
            if (escola.IdFreguesia != null) {
                var f = _context.Freguesia.Where(e => e.Id == escola.IdFreguesia).First();
                ViewBag.freguesia_nome = f.Nome;
                ViewBag.freguesia_id = f.Id;
            }
                

            PopulateDropDownList(escola.IdconcelhoNavigation, escola.IdTipoEscolaNavigation);
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
                TempData["successMessage"] = "Escola " + escola.NomeEscola + " editada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownList(escola.IdconcelhoNavigation, escola.IdTipoEscolaNavigation);
            return View(escola);
        }
        private void PopulateDropDownList(object selectedTipoEscola = null, object selectedConcelho = null)
        {
            var escolaQuery = from e in _context.TipoEscolas  orderby e.TipoEscola1 select e;
            var concelhoQuery = from c in _context.Concelhos orderby c.Nome select c;

            ViewData["IdTipoEscola"] = new SelectList(escolaQuery.AsNoTracking(), "IdTipoEscola", "TipoEscola1", selectedTipoEscola);
            ViewData["Idconcelho"] = new SelectList(concelhoQuery.AsNoTracking(), "Id", "Nome", selectedConcelho);
        }

         [Produces("application/json")]
        public JsonResult GetFreguesias(string concelho = null)
        {
            List<Freguesia> freguesias = null;
            if (!String.IsNullOrEmpty(concelho))
            {
                freguesias = _context.Freguesia.Where(e => e.ConcelhoNavigation.Nome.Contains(concelho)).OrderBy(e => e.Nome).ToList();
            }
            return new JsonResult(freguesias);
        }

        // POST: Escola/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var escola = await _context.Escolas.FindAsync(id);
            _context.Escolas.Remove(escola);
            await _context.SaveChangesAsync();
            TempData["successMessage"] = "Escola apagada.";
            return RedirectToAction(nameof(Index));
        }

        private bool EscolaExists(int id)
        {
            return _context.Escolas.Any(e => e.Id == id);
        }
    }
}