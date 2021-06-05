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

namespace Pmat_PI.Views
{
    [Authorize(Roles = "Admin")]
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

            IQueryable<Prova> provasPai = from p in _context.Provas where !(from sp in _context.SubProvas select sp.IdProvaFilho).Contains(p.Id) && !(from sp in _context.SubProvas select sp.IdProvaPai).Contains(p.Id) && (p.IdCompeticao.Equals(id)) select p;

            IQueryable<Prova> provas = from p in _context.Provas join sp in _context.SubProvas on p.Id equals sp.IdProvaFilho where p.IdCompeticao.Equals(id) select p;
            dynamic model = new System.Dynamic.ExpandoObject();
            model.provas = provas;
            model.provasPai = provasPai;

            return View(model);  
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

        // GET: Competicoes/InscreverEscolas
        public IActionResult InscreverEscolas(string concelho, string nomeEscola, string currentFilter)
        {
            IQueryable<Escola> escolas = null;
            if (!String.IsNullOrEmpty(concelho) && (!String.IsNullOrEmpty(nomeEscola)))
            {
                escolas = _context.Escolas.Where(e => e.NomeEscola.Contains(nomeEscola) && e.IdconcelhoNavigation.Nome.ToLower().Contains(concelho));
            }
            else if (!String.IsNullOrEmpty(nomeEscola))
            {
                escolas = _context.Escolas.Where(e => e.NomeEscola.Contains(nomeEscola));
            }
            else if (!String.IsNullOrEmpty(concelho))
            {
                escolas = _context.Escolas.Where(e => e.IdconcelhoNavigation.Nome.ToLower().Contains(concelho));
            }

            ViewBag.concelhos = (from c in _context.Concelhos orderby c.Nome select c).AsNoTracking();
            ViewBag.escolas = escolas;
            AnoLetivo al = _context.AnoLetivos.OrderBy(a => a.AnoLetivo1).Last();
            ViewBag.competicao = _context.Competicaos.Where(c => c.DataInicio >= al.Inicio  && c.DataFim <= al.Fim);

            return View();
        }

        [HttpPost]
        public async Task Inscrever(string competicao, string[] escolas)
        {
            Console.WriteLine("Competicao: "+ competicao);
            Console.WriteLine("Num escolas: " + escolas.Length);

            if (!String.IsNullOrEmpty(competicao) && escolas.Length > 0)
            {
                AnoLetivo al = _context.AnoLetivos.OrderBy(a => a.AnoLetivo1).Last();

                IQueryable<Prova> provasPai = from p in _context.Provas where !(from sp in _context.SubProvas select sp.IdProvaFilho).Contains(p.Id) && !(from sp in _context.SubProvas select sp.IdProvaPai).Contains(p.Id) && (p.IdCompeticaoNavigation.Nome.ToLower().Contains(competicao)) select p;

                IQueryable<Prova> provas = from p in _context.Provas join sp in _context.SubProvas on p.Id equals sp.IdProvaFilho where p.IdCompeticaoNavigation.Nome.ToLower().Contains(competicao) select p;

                provas.Concat(provasPai);
                foreach (string e in escolas)
                {
                    foreach (Prova pp in provas)
                    {
                        //Console.WriteLine(_context.ProvaEscolas.Any(pe => pe.AnoLetivo.Equals(al.AnoLetivo1) && pe.IdProva.Equals(pp.Id) && pe.IdEscola.Equals(int.Parse(e))));
                        
                      
                            ProvaEscola pe = new ProvaEscola();

                            pe.AnoLetivo = al.AnoLetivo1;
                            pe.DataRegisto = DateTime.Now;
                            pe.IdEscola = int.Parse(e);
                            pe.IdProva = pp.Id;

                            _context.ProvaEscolas.Add(pe);
                        //Console.WriteLine("Escola ja inscrita anteriomente nesta prova");
                        
                    }
                    var x = await _context.SaveChangesAsync();
                }
                TempData["msg"] = "Sucesso: escola(s) inscrita(s) nas provas da competição "+competicao +" com sucesso!";
                return;
            }
            else {
                Console.WriteLine("Erro: competição nao selecionada ou escolas nao escolhidas");
                return; 
            }
        }
    }
}
