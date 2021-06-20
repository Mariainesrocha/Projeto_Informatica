using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Pmat_PI.Models;
using Microsoft.AspNetCore.Identity;

namespace Pmat_PI.Views
{
    [Authorize(Roles = "Admin")]
    public class AssociarProfsController : Controller
    {
        private readonly ApplicationDbContextAlmostFinal _context;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AssociarProfsController(ApplicationDbContextAlmostFinal context, UserManager<User> usrMgr, RoleManager<IdentityRole> role)
        {
            _context = context;
            userManager = usrMgr;
            roleManager = role;
        }

        [Produces("application/json")]
        public JsonResult SearchEscolas(string nomeEscola = null)
        {
            List<Escola> escolas = null;
            if (!String.IsNullOrEmpty(nomeEscola))
            {
                escolas = _context.Escolas.Where(e => e.NomeEscola.Contains(nomeEscola) || e.Id.ToString().Equals(nomeEscola)).OrderBy(e => e.NomeEscola).Take(10).ToList();
            }
            return new JsonResult(escolas);
        }

        [Produces("application/json")]
        public JsonResult SearchUsers(string user = null)
        {
            List<AspNetUser> users = null;
            if (!String.IsNullOrEmpty(user))
            {
                users = _context.AspNetUsers.Where(u => u.Name.Replace(" ", "").Contains(user.Replace(" ", "")) || u.Id.Equals(user.Replace(" ", ""))).OrderBy(u => u.Name).Take(10).ToList();
            }
            return new JsonResult(users);
        }

        // GET: AssociarProfs/Create 
        public IActionResult Create(string ?idUser, string ?idSchool)
        {
            ViewData["AnoLetivo"] = new SelectList(_context.AnoLetivos, "AnoLetivo1", "AnoLetivo1");
            ViewData["IdAnoEscolar"] = new SelectList(_context.AnoEscolars, "Id", "Ano");
            ViewData["IdProjeto"] = new SelectList(_context.Projetos, "Id", "Descricao");
            if (idUser != null)
            {
                ViewData["idUser"] = idUser;
            }
            else
            {
                ViewData["idUser"] = "";
            }

            if (idSchool != null)
            {
                ViewData["idSchool"] = idSchool;
            }
            else
            {
                ViewData["idSchool"] = "";
            }

            return View();
        }

        // GET: AssociarProfs/Create 
        public IActionResult CreateAluno(string ?idUser, string ?idSchool)
        {
            ViewData["AnoLetivo"] = new SelectList(_context.AnoLetivos, "AnoLetivo1", "AnoLetivo1");
            ViewData["IdAnoEscolar"] = new SelectList(_context.AnoEscolars, "Id", "Ano");
            ViewData["IdProjeto"] = new SelectList(_context.Projetos, "Id", "Descricao");

            if (idUser != null)
            {
                ViewData["idUser"] = idUser;
            }
            else {
                ViewData["idUser"] = "";
            }

            if (idSchool != null)
            {
                ViewData["idSchool"] = idSchool;
            }
            else
            {
                ViewData["idSchool"] = "";
            }

            return View();
        }

        // POST: AssociarProfs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,IdEscola,IdResponsavel,IdAnoEscolar,AnoLetivo,IdProjeto,Data")] UserEscola userEscola, string aluno)
        {
            if (ModelState.IsValid)
            {
                var u = await userManager.FindByIdAsync(userEscola.IdUser);
                Escola e = _context.Escolas.Where(i => i.Id.Equals(userEscola.IdEscola)).First();
                var userRoles = userManager.GetRolesAsync(u).Result;
                Projeto p = _context.Projetos.Where(i => i.Id.Equals(userEscola.IdProjeto)).First();

                Console.WriteLine("here");
                var usrEsc = _context.UserEscolas.Where(ue => ue.IdUser.Equals(u.Id)).FirstOrDefault();
                if (usrEsc != null)
                {
                    Console.WriteLine("here");
                    _context.Remove(usrEsc);
                }
                _context.Add(userEscola);
                await _context.SaveChangesAsync();

                UserEscolaHistorico historico = new UserEscolaHistorico();
                historico.IdUser = userEscola.IdUser;
                historico.IdEscola = userEscola.IdEscola;
                historico.IdResponsavel = userEscola.IdResponsavel;
                historico.IdAnoEscolar = userEscola.IdAnoEscolar;
                historico.AnoLetivo = userEscola.AnoLetivo;
                historico.IdProjeto = userEscola.IdProjeto;
                historico.Data = userEscola.Data;
                _context.Add(historico);
                await _context.SaveChangesAsync();
        

                //It's a teacher
                if (String.IsNullOrEmpty(aluno))
                {
                    TempData["msg"] = "Associacao entre professor(a) " + u.Name + " e a escola " + e.NomeEscola + " para o projeto " + p.Descricao + " criada!";
                    if (!userRoles.Contains("Professor")) {
                        await userManager.AddToRoleAsync(u,"Professor");
                    }
                    if (userRoles.Contains("Aluno"))
                    {
                        await userManager.RemoveFromRoleAsync(u, "Aluno");
                    }
                    return RedirectToAction(nameof(Create));
                }
                //It's a student
                TempData["msg"] = "Associacao entre aluno(a) " + u.Name + " e a escola " + e.NomeEscola + " para o projeto " + p.Descricao + " criada!";
                if (!userRoles.Contains("Aluno"))
                {
                    await userManager.AddToRoleAsync(u, "Aluno");
                }
                if (userRoles.Contains("Professor"))
                {
                    await userManager.RemoveFromRoleAsync(u, "Professor");
                }
                return RedirectToAction(nameof(CreateAluno));
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
