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
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Globalization;
using MimeKit;

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
        public async Task<IActionResult> Details(int? id, int? pageNumber)
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


            // Verify if HTML already exists
            string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults" + "\\" + prova.Id + ".html";
            ViewData["FileExists"] = false;
            if (System.IO.File.Exists(path))
            {
                ViewData["FileExists"] = true;
            }

            // Get Enunciados
            int pageSize = 20;
            var enunciados_alunos = (from e in _context.ProvaEquipaEnunciados.Where(e => e.IdProva.Equals(id)).OrderByDescending(e => e.UltimoNivel)
                             join aluno in _context.EquipaAlunos.Include(aluno => aluno.IdUserNavigation) on e.IdEquipa equals aluno.IdEquipa   
                             select new Join_EnunciadoAlunos( e.Id,e.IdEquipa,e.UltimoNivel,e.Tempo,aluno.IdUserNavigation.Id, aluno.IdUserNavigation.Name ))
                             .Skip(pageSize * (pageNumber ?? 1 - 1))
                             .Take(pageSize)
                             .ToList();

            ViewModel_ProvaDetails prova_details = new ViewModel_ProvaDetails();
            prova_details.prova = prova;
            prova_details.enunciados =  enunciados_alunos;

               return View(prova_details);
        }
        // Generate HTML
        public async Task<IActionResult> GenerateHTML(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prova = await _context.Provas
                .Include(p => p.IdCompeticaoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prova == null)
            {
                return NotFound();
            }

            // Generate HTML file and Save it 
            create_saveFile(prova);

            // Return to details page
            return RedirectToAction("Details", new { id = prova.Id });
        }

        // Delete HTML
        public async Task<IActionResult> DeleteHTML(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prova = await _context.Provas
                .Include(p => p.IdCompeticaoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prova == null)
            {
                return NotFound();
            }

            // Delete HTML File
            string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults" + "\\" + prova.Id + ".html";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            // Return to details page
            return RedirectToAction("Details", new { id = prova.Id });
        }

        // Open HTML
        public async Task<IActionResult> OpenHTML(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prova = await _context.Provas
                .Include(p => p.IdCompeticaoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prova == null)
            {
                return NotFound();
            }

            // Delete HTML File
            string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults" + "\\" + prova.Id + ".html";
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, MimeTypes.GetMimeType(path), Path.GetFileName(path));
            }

            // Return to details page
            return RedirectToAction("Details", new { id = prova.Id });
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




        public void create_saveFile(Prova prova)
        {
            string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults";


            if (!Directory.Exists(path))
            {
                // Create the Directory 
                DirectoryInfo dinfo = Directory.CreateDirectory(path);
            }

            if (!System.IO.File.Exists(path + "\\" + prova.Id + ".html"))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(path + "\\" + prova.Id + ".html"))
                {
                    string[] template_lines = System.IO.File.ReadAllLines(path+"\\00template.html");

                    bool beforeTable = true;
                    string contentBeforeTable="";
                    string tableContent = "";
                    string contentAfterTable = "";
                  

                    // Use Template and fill the Document with this exam's results
                    foreach (string line in template_lines)
                    {        
                        if (beforeTable)
                        {
                            if (line.Contains("<h5>"))
                            {
                                contentBeforeTable += "<h4> Prova : " + prova.NomeProva + "</h4>";
                            }
                            else
                            {
                                contentBeforeTable += line;
                            }
                           
                            if (line.Contains("Estado")) {
                                contentBeforeTable += "</tr><tr> ";
                                // Fill table with students results
                                tableContent = getProvaResults(prova);
                                // Change beforeTable to false
                                beforeTable = false;
                            }
                        }
                        else
                        {
                            contentAfterTable += line;
                        }
                    }
                    sw.Write(contentBeforeTable);
                    sw.Write(tableContent);
                    sw.Write(contentAfterTable);
                }
            }
        }

        public string getProvaResults(Prova prova)
        {
            string tableContent = "";
            Console.WriteLine(prova.Id);
            Console.WriteLine("----");
            List<ProvaEquipaEnunciado> provaEnunciados = 
                _context.ProvaEquipaEnunciados.Include(e=> e.IdEquipaNavigation.IdEscolaNavigation.IdconcelhoNavigation.DistritoNavigation)
                .Where(e => e.IdProva.Equals(prova.Id))
                .OrderByDescending(u => u.UltimoNivel).ThenBy(t => t.Tempo).ToList();
                
            int counter = 0;
            foreach (ProvaEquipaEnunciado enunciado in provaEnunciados)
            {
                counter += 1;
                var alunos = _context.EquipaAlunos.Where(e => e.IdEquipa.Equals(enunciado.IdEquipa)).Include(z => z.IdUserNavigation).ToList();
                
                string alunosNomes = "";
                string escolaridade = "";
                foreach(EquipaAluno equipa_aluno in alunos)
                {
                    alunosNomes += equipa_aluno.IdUserNavigation.Name + "\n ";
                    var user_escolas = _context.UserEscolas.Where(e => e.IdUser.Equals(equipa_aluno.IdUser)).Include(e => e.IdAnoEscolarNavigation).ToList();
                    foreach(UserEscola user_escola in user_escolas)
                    {
                        escolaridade += user_escola.IdAnoEscolarNavigation.Ano + "\n";
                    }
                }
          
                string row = "<tr>" +
                    "<td>" + counter + "</td>" +
                    "<td>" + enunciado.IdEquipa + "</td>" +
                    "<td>" + alunosNomes  + "</td>" +
                    "<td>" + escolaridade + "</td>" +
                    "<td>" + enunciado.UltimoNivel + "</td>" +
                    "<td>" + enunciado.Tempo  + "</td>" +
                    "<td>" + enunciado.Data + "</td>" +
                    "<td>" + enunciado.IdEquipaNavigation.IdEscola  + "</td>" +
                    "<td>" + enunciado.IdEquipaNavigation.IdEscolaNavigation.NomeEscola + "</td>" +
                    "<td>" + (enunciado.IdEquipaNavigation.IdEscolaNavigation.IdconcelhoNavigation.DistritoNavigation.Nome != null ? enunciado.IdEquipaNavigation.IdEscolaNavigation.IdconcelhoNavigation.DistritoNavigation.Nome : "Unknown") + "</td>" +
                    "<td>"+  (enunciado.Status != null ? enunciado.Status.ToString() : "Unknown") + "</td></tr>";
                
                tableContent += row;
       


               //TimeSpan enunciadoTime =  TimeSpan.Parse(enunciado.Tempo);
               //double tempo = enunciadoTime.TotalSeconds;


            }
            return tableContent;
        }
    }
}
