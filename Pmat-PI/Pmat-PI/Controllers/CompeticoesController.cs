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
using MimeKit;
using Ionic.Zip;

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
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competicao == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            IQueryable<Prova> provas = from p in _context.Provas where !(from sp in _context.SubProvas select sp.IdProvaFilho).Contains(p.Id) && !(from sp in _context.SubProvas select sp.IdProvaPai).Contains(p.Id) && (p.IdCompeticao.Equals(id)) select p;

            IQueryable<Prova> provasPai = from p in _context.Provas join sp in _context.SubProvas on p.Id equals sp.IdProvaPai where p.IdCompeticao.Equals(id) select p;
            dynamic model = new System.Dynamic.ExpandoObject();
            model.provas = provas;
            model.provasPai = provasPai;
            ViewData["compId"] = competicao.Id;

            List<Prova> provasL = provas.ToList();
            Dictionary<int, bool> mapa = new Dictionary<int, bool>();

            foreach (Prova p in provasL)
            {
                // Verify if HTML already exists
                string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults" + "\\" + p.Id + ".html";
                if (!String.IsNullOrEmpty(p.IdCompeticao.ToString()))
                {
                    path = Directory.GetCurrentDirectory() + "\\CompetitionsResults\\" + p.IdCompeticao + "\\" + p.Id + ".html";
                }
                mapa[p.Id]=false;
                if (System.IO.File.Exists(path))
                {
                    mapa[p.Id] = true;
                }
            }
            ViewBag.mapa = mapa;

            return View(model);  
        }

        public async Task<IActionResult> VerFilhos(int? compId, int? paiId)
        {
            if (compId == null || paiId == null)
            {
                Response.StatusCode = 404; 
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos.FirstOrDefaultAsync(m => m.Id == compId);
            if (competicao == null)
            {
                Response.StatusCode = 404; 
                return View(nameof(NotFound));
            }
            IQueryable<Prova> provas = from p in _context.Provas where (from sp in _context.SubProvas where sp.IdProvaPai==paiId select sp.IdProvaFilho).Contains(p.Id) && (p.IdCompeticao.Equals(compId)) select p;
           
            return View("ProvasFilho",provas.ToList());
        }

        public async Task<IActionResult> VerEscolas(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos.FirstOrDefaultAsync(m => m.Id == id);
            if (competicao == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }
            ViewBag.CompeticaoID = competicao.Id;

            IQueryable<ProvaEscola> provas_esc = from pe in _context.ProvaEscolas join p in _context.Provas.Where(p => p.IdCompeticao.Equals(competicao.Id)) on pe.IdProva equals p.Id select pe;
            IQueryable<Escola> escolas = (from e in _context.Escolas join pe in provas_esc on e.Id equals pe.IdEscola select e).Distinct();

            return View(escolas);
        }

        [HttpPost, ActionName("RemoverEscola")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverEscola(int? id,int? compID)
        {
            if (id == null || compID == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            //remover
            IQueryable<ProvaEscola> provas_esc = from pe in _context.ProvaEscolas.Where(pe => pe.IdEscola.Equals(id)) join p in _context.Provas.Where(p => p.IdCompeticao.Equals(compID)) on pe.IdProva equals p.Id select pe;
            foreach(ProvaEscola pe in provas_esc)
            {
                _context.ProvaEscolas.Remove(pe);
            }
            await _context.SaveChangesAsync();           

            return RedirectToAction(nameof(VerEscolas), new { id = compID });
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
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos.FindAsync(id);
            if (competicao == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
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
                Response.StatusCode = 404;
                return View(nameof(NotFound));
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
        public IActionResult InscreverEscolas(string concelho, string nomeEscola)
        {
            IQueryable<Escola> escolas = null;
            var flag = false;
            if (!String.IsNullOrEmpty(concelho) && (!String.IsNullOrEmpty(nomeEscola)))
            {
                escolas = _context.Escolas.Where(e => e.NomeEscola.Contains(nomeEscola) && e.IdconcelhoNavigation.Nome.ToLower().Contains(concelho));
                flag = (escolas == null) ? true : false;
            }
            else if (!String.IsNullOrEmpty(nomeEscola))
            {
                escolas = _context.Escolas.Where(e => e.NomeEscola.Contains(nomeEscola));
                flag = (escolas == null) ? true : false;
            }
            else if (!String.IsNullOrEmpty(concelho))
            {
                escolas = _context.Escolas.Where(e => e.IdconcelhoNavigation.Nome.ToLower().Contains(concelho));
                flag = (escolas == null) ? true : false;
            }

            ViewBag.concelhos = (from c in _context.Concelhos orderby c.Nome select c).AsNoTracking();
            ViewBag.escolas = escolas;
            ViewBag.flag = flag;
            AnoLetivo al = _context.AnoLetivos.OrderBy(a => a.AnoLetivo1).Last();
            ViewBag.competicao = _context.Competicaos.Where(c => c.DataInicio >= al.Inicio  && c.DataFim <= al.Fim);

            return View();
        }

        [Produces("application/json")]
        public JsonResult SearchProvas(string nomeProva = null)
        {
            List<Prova> provas = null;
            if (!String.IsNullOrEmpty(nomeProva))
            {
                provas = (from p in _context.Provas where !(from sp in _context.SubProvas select sp.IdProvaFilho).Contains(p.Id) && !(from sp in _context.SubProvas select sp.IdProvaPai).Contains(p.Id) && p.NomeProva.Replace(" ", "").Contains(nomeProva.Replace(" ", "")) select p).Take(20).ToList();
            }
            foreach (Prova p in provas) {
                Console.WriteLine(p.NomeProva);
                  }
            return new JsonResult(provas);
        }

        [HttpPost]
        public async Task<ActionResult> Inscrever(string competicao, string[] escolas)
        {
            ViewBag.msg = null;
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
                        ProvaEscola pe = new ProvaEscola();

                        pe.AnoLetivo = al.AnoLetivo1;
                        pe.DataRegisto = DateTime.Now;
                        pe.IdEscola = int.Parse(e);
                        pe.IdProva = pp.Id;

                        _context.ProvaEscolas.Add(pe);                        
                    }
                    await _context.SaveChangesAsync();
                }
                return Content("Sucesso: escola(s) inscrita(s) nas provas da competição " + competicao + " com sucesso!");
            }
            else {
                return Content("Erro: competição nao selecionada ou escolas nao escolhidas");
            }
        }

        // Generate HTML
        public async Task<IActionResult> GenerateHTML(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos
               .FirstOrDefaultAsync(m => m.Id == id);
            if (competicao == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            IQueryable<Prova> provas = from p in _context.Provas where !(from sp in _context.SubProvas select sp.IdProvaFilho).Contains(p.Id) && !(from sp in _context.SubProvas select sp.IdProvaPai).Contains(p.Id) && (p.IdCompeticao.Equals(id)) select p;

            IQueryable<Prova> provasPai = from p in _context.Provas join sp in _context.SubProvas on p.Id equals sp.IdProvaPai where p.IdCompeticao.Equals(id) select p;

            List<Prova> provasL = provas.ToList();
            List<Prova> provasPaiL = provasPai.ToList();

            foreach (Prova p in provasL) {
                // Generate HTML file and Save it 
                create_saveFile(p);
            }

            foreach (Prova pai in provasPaiL)
            {
                IQueryable<Prova> filhos = from f in _context.Provas where (from sp in _context.SubProvas where sp.IdProvaPai == pai.Id select sp.IdProvaFilho).Contains(f.Id) && (f.IdCompeticao.Equals(pai.IdCompeticao)) select f;
                List<Prova> filhosL = filhos.ToList();
                foreach (Prova filho in filhosL)
                {
                    create_saveFile(filho);
                }
            }
            
            // Return to details page
            return RedirectToAction("Details", new { id = competicao.Id });
        }

        public async Task<IActionResult> DeleteHTML(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos
               .FirstOrDefaultAsync(m => m.Id == id);
            if (competicao == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            IQueryable<Prova> provas = from p in _context.Provas where !(from sp in _context.SubProvas select sp.IdProvaFilho).Contains(p.Id) && !(from sp in _context.SubProvas select sp.IdProvaPai).Contains(p.Id) && (p.IdCompeticao.Equals(id)) select p;

            IQueryable<Prova> provasPai = from p in _context.Provas join sp in _context.SubProvas on p.Id equals sp.IdProvaPai where p.IdCompeticao.Equals(id) select p;

            List<Prova> provasL = provas.ToList();
            List<Prova> provasPaiL = provasPai.ToList();

            foreach (Prova p in provasL)
            {
                // Delete HTML File
                string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults" + "\\" + p.Id + ".html";
                if (!String.IsNullOrEmpty(p.IdCompeticao.ToString()))
                {
                    path = Directory.GetCurrentDirectory() + "\\CompetitionsResults\\" + p.IdCompeticao + "\\" + p.Id + ".html";
                }
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }

            foreach (Prova pai in provasPaiL) {
                IQueryable<Prova> filhos = from f in _context.Provas where (from sp in _context.SubProvas where sp.IdProvaPai == pai.Id select sp.IdProvaFilho).Contains(f.Id) && (f.IdCompeticao.Equals(pai.IdCompeticao)) select f;
                List<Prova> filhosL = filhos.ToList();
                foreach (Prova filho in filhosL) {
                    // Delete HTML File
                    string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults" + "\\" + filho.Id + ".html";
                    if (!String.IsNullOrEmpty(filho.IdCompeticao.ToString()))
                    {
                        path = Directory.GetCurrentDirectory() + "\\CompetitionsResults\\" + filho.IdCompeticao + "\\" + filho.Id + ".html";
                    }
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }

            string checkP = Directory.GetCurrentDirectory() + "\\CompetitionsResults";
            foreach (var directory in Directory.GetDirectories(checkP))
            {
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }

            // Return to details page
            return RedirectToAction("Details", new { id = competicao.Id });
        }

        public async Task<IActionResult> DownloadHtml(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            var competicao = await _context.Competicaos
               .FirstOrDefaultAsync(m => m.Id == id);
            if (competicao == null)
            {
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }

            string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults\\" + competicao.Id;
            if (Directory.Exists(path))
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(path);
                    MemoryStream output = new MemoryStream();
                    zip.Save(output);
                    return File(output.ToArray(), "application/zip", competicao.Nome+".zip");
                }
            }

            // Return to details page
            return RedirectToAction("Details", new { id = competicao.Id });
        }

        public void create_saveFile(Prova prova)
        {
            string path = Directory.GetCurrentDirectory() + "\\CompetitionsResults";
            string[] template_lines = System.IO.File.ReadAllLines(path + "\\00template.html");

            if (!String.IsNullOrEmpty(prova.IdCompeticao.ToString()))
            {
                path = Directory.GetCurrentDirectory() + "\\CompetitionsResults\\" + prova.IdCompeticao;
                template_lines = System.IO.File.ReadAllLines(path + "\\..\\00template.html");
                if (!Directory.Exists(path))
                {
                    DirectoryInfo dinfo = Directory.CreateDirectory(path);
                }
            }


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
                     bool beforeTable = true;
                    string contentBeforeTable = "";
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

                            if (line.Contains("Estado"))
                            {
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
            List<ProvaEquipaEnunciado> provaEnunciados =
                _context.ProvaEquipaEnunciados.Include(e => e.IdEquipaNavigation.IdEscolaNavigation.IdconcelhoNavigation.DistritoNavigation)
                .Where(e => e.IdProva.Equals(prova.Id))
                .OrderByDescending(u => u.UltimoNivel).ThenBy(t => t.Tempo).ToList();

            int counter = 0;
            foreach (ProvaEquipaEnunciado enunciado in provaEnunciados)
            {
                counter += 1;
                var alunos = _context.EquipaAlunos.Where(e => e.IdEquipa.Equals(enunciado.IdEquipa)).Include(z => z.IdUserNavigation).ToList();
                string alunosNomes = "";

                foreach (EquipaAluno equipa_aluno in alunos)
                {
                    alunosNomes += equipa_aluno.IdUserNavigation.Name + "\n ";
                }
                string row = "<tr>" +
                "<td>" + counter + "</td>" +
                "<td>" + enunciado.IdEquipa + "</td>" +
                "<td>" + alunosNomes + "</td>" +
                "<td>" + enunciado.UltimoNivel + "</td>" +
                "<td>" + enunciado.Tempo + "</td>" +
                "<td>" + enunciado.Data + "</td>" +
                "<td>" + enunciado.IdEquipaNavigation.IdEscola + "</td>" +
                "<td>" + enunciado.IdEquipaNavigation.IdEscolaNavigation.NomeEscola + "</td>" +
                "<td>" + (enunciado.IdEquipaNavigation.IdEscolaNavigation.IdconcelhoNavigation.DistritoNavigation.Nome != null ? enunciado.IdEquipaNavigation.IdEscolaNavigation.IdconcelhoNavigation.DistritoNavigation.Nome : "Unknown") + "</td>" +
                "<td>" + (enunciado.Status != null ? enunciado.Status.ToString() : "Unknown") + "</td></tr>";

                tableContent += row;

                //TimeSpan enunciadoTime =  TimeSpan.Parse(enunciado.Tempo);
                //double tempo = enunciadoTime.TotalSeconds;
            }
            return tableContent;
        }

    }
}

////Scaffold-DbContext "Server=localhost;Database=pmate2-demo;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -context 'ApplicationDbContextTemp' -Tables AspNetUsers,SubProva,Competicao,Prova,CicloEnsino  -force
