using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pmat_PI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmat_PI.Controllers
{
    public class MapController : Controller
    {

        private readonly ApplicationDbContextAlmostFinal _context;
        Dictionary<string, ArrayList> econcelhos;

        public MapController(ApplicationDbContextAlmostFinal context)
        {
            _context = context;
        }

        public IActionResult Map(ApplicationDbContextAlmostFinal context)
        {
            econcelhos = new Dictionary<string, ArrayList>();
           /* List<Escola> escolas = _context.Escolas.ToList();
            


            foreach (Escola e in escolas) {
                Dictionary<string, string> attrs = new Dictionary<string, string>();
                ArrayList al;

                if (e.Localidade == null || e.Localidade == "") {
                    continue;
                }

                if (!econcelhos.ContainsKey(e.Localidade.ToLower().Trim()))
                { 
                    attrs["Nome"] =  e.NomeEscola;

                    if (e.Latitude == null || e.Longitude == null)
                    {
                        continue;
                    }
                    else
                    {
                        attrs["Lat"] = e.Latitude;
                        attrs["Lon"] = e.Longitude;
                    }

                    if (e.CodDgeec != null)
                    {
                        attrs["Dgeec"] =  e.CodDgeec.ToString();
                    }
                    else
                    {
                        attrs["Dgeec"] =  "Desconhecido";
                    }

                    if (e.CodDgpgf != null)
                    {
                        attrs["Dgpgf"] =  e.CodDgpgf.ToString();
                    }
                    else
                    {
                        attrs["Dgpgf"] =  "Desconhecido";
                    }

                    al = new ArrayList();
                    al.Add(attrs);
                    econcelhos[e.Localidade.ToLower().Trim()] = al;
                }
                else {
                    al = econcelhos[e.Localidade.ToLower().Trim()];

                    attrs["Nome"] = e.NomeEscola;

                    if (e.Latitude == null || e.Longitude == null)
                    {
                        continue;
                    }
                    else
                    {
                        attrs["Lat"] = e.Latitude;
                        attrs["Lon"] = e.Longitude;
                    }

                    if (e.CodDgeec != null)
                    {
                        attrs["Dgeec"] = e.CodDgeec.ToString();
                    }
                    else
                    {
                        attrs["Dgeec"] = "Desconhecido";
                    }

                    if (e.CodDgpgf != null)
                    {
                        attrs["Dgpgf"] = e.CodDgpgf.ToString();
                    }
                    else
                    {
                        attrs["Dgpgf"] = "Desconhecido";
                    }

                    al.Add(attrs);
                }
            }
           */
            ViewBag.econcelhos = econcelhos;
            return View();
        }

    }
}
