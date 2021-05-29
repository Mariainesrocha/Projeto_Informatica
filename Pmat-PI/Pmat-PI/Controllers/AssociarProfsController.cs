using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Pmat_PI.Models;

namespace Pmat_PI.Views
{
    [Authorize(Roles = "Admin")]
    public class AssociarProfsController : Controller
    {
        private readonly EscolasContext _context;

        public AssociarProfsController(EscolasContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        public JsonResult SearchEscolas(string nomeEscola = null)
        {
            List<Escola> escolas = null;
            if (!String.IsNullOrEmpty(nomeEscola))
            {
                escolas = _context.Escolas.Where(e => e.NomeEscola.Contains(nomeEscola)).OrderBy(e => e.NomeEscola).Take(10).ToList();
            }
            return new JsonResult(escolas);
        }

        [Produces("application/json")]
        public JsonResult SearchProfs(string professor = null)
        {
            List<AspNetUser> profs = null;
            if (!String.IsNullOrEmpty(professor))
            {
                profs = _context.AspNetUsers.Where(u => u.Name.Contains(professor)).OrderBy(u => u.Name).Take(10).ToList();
            }
            return new JsonResult(profs);
        }

        // GET: AssociarProfs/Create 
        public IActionResult Create()
        {
            ViewData["AnoLetivo"] = new SelectList(_context.AnoLetivos, "AnoLetivo1", "AnoLetivo1");
            ViewData["IdAnoEscolar"] = new SelectList(_context.AnoEscolars, "Id", "Ano");
            ViewData["IdProjeto"] = new SelectList(_context.Projetos, "Id", "Descricao");
            return View();
        }

        // POST: AssociarProfs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,IdEscola,IdResponsavel,IdAnoEscolar,AnoLetivo,IdProjeto,Data")] UserEscola userEscola)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userEscola);
                await _context.SaveChangesAsync();
                AspNetUser u = _context.AspNetUsers.Where(i => i.Id.Equals(userEscola.IdUser)).First();
                Escola e = _context.Escolas.Where(i => i.Id.Equals(userEscola.IdEscola)).First();
                Projeto p = _context.Projetos.Where(i => i.Id.Equals(userEscola.IdProjeto)).First();

                TempData["msg"] = "Associacao entre professor(a) "+ u.Name + " e a escola "+e.NomeEscola+" para o projeto "+p.Descricao+" criada!";
                return RedirectToAction(nameof(Create));
            }
            ViewData["AnoLetivo"] = new SelectList(_context.AnoLetivos, "AnoLetivo1", "AnoLetivo1", userEscola.AnoLetivo);
            ViewData["IdAnoEscolar"] = new SelectList(_context.AnoEscolars, "Id", "Ano", userEscola.IdAnoEscolar);
            ViewData["IdProjeto"] = new SelectList(_context.Projetos, "Id", "Descricao", userEscola.IdProjeto);
            return View(userEscola);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProj(string descricao,string URL)
        {
            Projeto p = new Projeto();
            p.Descricao = descricao;
            p.Url = URL;
            p.Id = _context.Projetos.Max(item => item.Id) + 1;

            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return null;
        }
    } 
}
