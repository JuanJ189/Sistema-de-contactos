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

        public async Task<IActionResult> Crear([Bind("Name","Edad","Celular","Correo")] Registro registro, IFormFile foto)
        {
            registro.Id = ObjectId.GenerateNewId().ToString();
            if (foto != null && foto.Length > 0)
            {
                var subir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidos");
                //Cambiar nombre
                var filename = Path.GetFileName(foto.FileName);
                var filepath = Path.Combine(subir, filename);
                //Guardar imagen
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }
                registro.Foto = $"/Subidos/{filename}";
            }
            else
            {
                registro.Foto = "/Defecto/Contac_img.jpg";
            }
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

        public async Task<IActionResult> Editar(string Id, Registro registro, IFormFile foto)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var registro_ex = await _registro.Find(p => p.Id==Id).FirstOrDefaultAsync();
            if (registro_ex == null)
            {
                return NotFound();
            }
            ModelState.Remove("Foto");
            //si se sube una nueva foto, reemplzar la anterior
            if (foto != null && foto.Length > 0)
            {
                var subir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidos");
                var filename = Path.GetFileName(foto.FileName);
                var filepath = Path.Combine(subir, filename);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }
                registro.Foto = $"/Subidos/{filename}";
            }
            else
            {
                registro.Foto = registro_ex.Foto;
            }
            if (ModelState.IsValid)
            {
                await _registro.ReplaceOneAsync(p => p.Id == Id, registro);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage);
                 }
                return View(registro);
            }
            
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
