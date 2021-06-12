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

            IQueryable<TreinoEnunciado> enunciados = _context.TreinoEnunciados.Include(p => p.IdUserNavigation).Include(p => p.IdTreinoNavigation);

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
        public async Task<IActionResult> Details(int? id)
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


        private bool TreinoEnunciadoExists(int id)
        {
            return _context.TreinoEnunciados.Any(e => e.Id == id);
        }
    }
}
