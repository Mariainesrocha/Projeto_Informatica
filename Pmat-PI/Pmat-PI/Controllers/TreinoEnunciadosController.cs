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
    public class TreinoEnunciadosController : Controller
    {
        private readonly ApplicationDbContextAlmostFinal _context;

        public TreinoEnunciadosController(ApplicationDbContextAlmostFinal context)
        {
            _context = context;
        }

        // GET: TreinoEnunciados
        public async Task<IActionResult> Index(string searchTreino, string searchEnunciado, string searchUser, int? pageNumber)
        {
            ViewData["searchTreino"] = searchTreino;
            ViewData["searchEnunciado"] = searchEnunciado;
            ViewData["searchUser"] = searchUser;

            IQueryable<TreinoEnunciado> enunciados = _context.TreinoEnunciados.Include(p => p.IdUserNavigation).Include(p => p.IdTreinoNavigation).OrderBy(e=>e.Id);

            if (!String.IsNullOrEmpty(searchTreino))
            {
                enunciados = enunciados.Where(e => e.IdTreino.ToString().Equals(searchTreino));
            }

            if (!String.IsNullOrEmpty(searchEnunciado))
            {
                enunciados = enunciados.Where(e => e.Id.ToString().Equals(searchEnunciado));
            }

            if (!String.IsNullOrEmpty(searchUser))
            {
                enunciados = enunciados.Where(e => e.IdUser.Equals(searchUser));
            }

            int pageSize = 30;

            return View(await PaginatedList<TreinoEnunciado>.CreateAsync(enunciados.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: TreinoEnunciados/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treinoEnunciado = await _context.TreinoEnunciados
                .Include(t => t.IdTreinoNavigation)
                .Include(t => t.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            var userResps = _context.TreinoEnunNivelUserResps.Where(r => r.IdEnunciadoEquipa == id).OrderByDescending(r => r.IdNivel).ThenByDescending(r => r.Tentativa).ToList();
            if (userResps.Count() == 0)
            {
                ViewBag.userResps = null;
            }
            else
            {
                ViewBag.userResps = userResps;
            }

            if (userResps.Count() < 2)
            {
                ViewBag.but = false;
            }
            else {
                ViewBag.but = true;
            }

            if (treinoEnunciado == null)
            {
                return NotFound();
            }

            return View(treinoEnunciado);
        }


        // GET: TreinoEnunciados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treinoEnunciado = await _context.TreinoEnunciados
                .Include(t => t.IdTreinoNavigation)
                .Include(t => t.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treinoEnunciado == null)
            {
                return NotFound();
            }

            return View(treinoEnunciado);
        }


        public async Task<IActionResult> Recover(int id) {
            var treinoEnunciado = await _context.TreinoEnunciados.FirstOrDefaultAsync(m => m.Id == id);
            var userResps = _context.TreinoEnunNivelUserResps.Where(r => r.IdEnunciadoEquipa == id).OrderByDescending(r => r.IdNivel).ThenByDescending(r => r.Tentativa);
            var userResp1 = userResps.FirstOrDefault();
            var userResp2 = userResps.Skip(1).FirstOrDefault();
            Console.WriteLine("Piggy:" + userResps.ToList().Count());
            //Apagar a ultima reposta;
            //Atualizar o ultimo nivel e o tempo do enunciado;
            //Meter o status do enunciado a 1;
            ViewBag.but = true;
            _context.Remove(userResp1);
            treinoEnunciado.Tempo = userResp2.Tempo;
            treinoEnunciado.UltimoNivel = userResp2.IdNivel;
            treinoEnunciado.Status = 1;
            _context.Update(treinoEnunciado);
            _context.SaveChanges();
            
            return RedirectToAction("Details", new { id = id });
        }


        private bool TreinoEnunciadoExists(int id)
        {
            return _context.TreinoEnunciados.Any(e => e.Id == id);
        }
    }
}
