using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MongoDB.Bson;
using MongoDB.Driver;
using Proyecto_Bb_2.Models;

namespace Proyecto_Bb_2.Controllers
{
    public class ActividadesController : Controller
    {
        private readonly IMongoCollection<RegistroAc> _actividades;

        public ActividadesController (IMongoClient mongo)
        {
            var db = mongo.GetDatabase("Proyecto_Bb_2");
            _actividades = db.GetCollection<RegistroAc>("Actividades");
        }

        public async Task<IActionResult> Index()
        {
            var actividad = await _actividades.Find(_ => true).ToListAsync();
            return View(actividad);
        }

        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Titulo","Fecha", "Descripcion", "Notificacion")] RegistroAc actividad)
        {
            actividad.Fecha_creacion = DateTime.Now;
            actividad.Id = ObjectId.GenerateNewId().ToString();
            try
            {
                await _actividades.InsertOneAsync(actividad);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Error insertando documento");
                return View(actividad);
            }
        }
        public async Task<IActionResult> Editar(string Id)
        {
            var actividad = await _actividades.Find(a => a.Id == Id).FirstOrDefaultAsync();
            if (actividad == null)
            {
                return NotFound();
            }
            return View(actividad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(string Id, [Bind("Id","Titulo","Fecha","Descripcion","Notificacion")] RegistroAc registro)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var acoriginal = await _actividades.Find(a=>a.Id == Id).FirstOrDefaultAsync();
            if (acoriginal == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                registro.Id = acoriginal.Id;
                registro.Fecha_creacion = acoriginal.Fecha_creacion;
                try
                {
                    await _actividades.ReplaceOneAsync(a => a.Id == Id, registro);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    ModelState.AddModelError("", "Error en editar la actividad");
                    return View(registro);
                }
            }
            return View(registro);
        }
        public async Task<IActionResult> Eliminar(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var registro = await _actividades.Find(p => p.Id == Id).FirstOrDefaultAsync();
            if (registro == null)
            {
                return NotFound();
            }
            return View(registro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrar(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            await _actividades.DeleteOneAsync(p => p.Id == Id);
            return RedirectToAction(nameof(Index));
        }

    }
}
