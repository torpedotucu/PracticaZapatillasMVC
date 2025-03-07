using Microsoft.AspNetCore.Mvc;
using PracticaZapatillasMVC.Models;
using PracticaZapatillasMVC.Repositories;

namespace PracticaZapatillasMVC.Controllers
{
    public class ZapasController : Controller
    {
        private RepositoryZapatillas repo;
        public ZapasController(RepositoryZapatillas repo)
        {
            this.repo=repo;
        }


        public async Task<IActionResult> Index()
        {
            List<Zapatilla>zapas=await this.repo.GetZapatillasAsync();
            return View(zapas);
        }

        public async Task<IActionResult>Detalles(int idZapas)
        {
            //var zapas = this.repo.DetallesZapatilla(idZapas);
            (int registro, Zapatilla zapas, ImagenZapatillas imagen)=await this.repo.PaginacionImagenesZapatillas(1, idZapas);
            ViewData["ZAPAS"]=zapas;
            return View();
        }

        public async Task<IActionResult> _ImagenesPartial(int?posicion, int idZapas)
        {
            if (posicion==null)
            {
                posicion=1;
            }
            ViewData["POSICION"]=posicion.Value;
            (int registro, Zapatilla zapas, ImagenZapatillas imagen)=await this.repo.PaginacionImagenesZapatillas(posicion.Value, idZapas);
            ViewData["REGISTRO"]=registro;
            ViewData["ZAPAS"]=zapas;
            int siguiente = posicion.Value+1;
            if (siguiente>registro)
            {
                siguiente=registro;
            }
            int anterior = posicion.Value-1;
            if (anterior<1)
            {
                anterior=1;
            }
            ViewData["ULTIMO"]=registro;
            ViewData["SIGUIENTE"]=siguiente;
            ViewData["ANTERIOR"]=anterior;

            return PartialView("_ImagenesPartial", imagen);

        }
    }
}
