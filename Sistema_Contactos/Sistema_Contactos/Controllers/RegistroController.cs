using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Bb_2.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Proyecto_Bb_2.Controllers
{
    public class RegistroController : Controller
    {
        private readonly IMongoCollection<Registro> _registro;
        public RegistroController(IMongoClient mongo)
        {
            var Db = mongo.GetDatabase("Proyecto_Bb_2");
            _registro = Db.GetCollection<Registro>("Contactos");
        }
        public async Task<IActionResult> Index()
        {
            var Registros = await _registro.Find(_ => true).ToListAsync();
            return View(Registros);
        }
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Crear([Bind("Name","Edad","Celular","Correo")] Registro registro)
        {
            registro.Id = ObjectId.GenerateNewId().ToString();
            try
            {
                await _registro.InsertOneAsync(registro);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(" ","Error insertando documento");
                return View(registro);
            }
        }

        public async Task<IActionResult> Editar(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var registro = await _registro.Find(p => p.Id == Id).FirstOrDefaultAsync();
            if (registro == null)
            {
                return NotFound();
            }
            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Editar(string Id,[Bind("Id","Name", "Edad", "Celular", "Correo")] Registro registro)
        {
            if (Id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _registro.ReplaceOneAsync(p => p.Id == Id, registro);
                return RedirectToAction(nameof(Index));
            }
            return View(registro);
        }

        public async Task<IActionResult> BorrarC (string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var registro = await _registro.Find(p => p.Id==Id).FirstOrDefaultAsync();
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
            await _registro.DeleteOneAsync(p => p.Id == Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
