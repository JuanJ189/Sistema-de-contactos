using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Proyecto_Bb_2.Models;
using Proyecto_Bb_2.Servicios;

namespace Proyecto_Bb_2.Servicios
{
    public class Notificaciones : BackgroundService
    {
        private readonly IMongoCollection<RegistroAc> _actividades;
        private readonly EmailService _emailService;
        private readonly ILogger<Notificaciones> _logger;

        public Notificaciones(IMongoClient mongoClient, EmailService emailService, ILogger<Notificaciones> logger)
        {
            var Db = mongoClient.GetDatabase("Proyecto_Bb_2");
            _actividades = Db.GetCollection<RegistroAc>("Actividades");
            _emailService = emailService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var actividades = await _actividades.Find(_ => true).ToListAsync();

                foreach(var ac in actividades)
                {
                    await EnviarNotificacion(ac);
                }

                await Task.Delay(TimeSpan.FromMinutes(0.30), stoppingToken);
            }
        }
        private async Task EnviarNotificacion(RegistroAc actividad)
        {
            var fechaActividad = actividad.Fecha;

            var ahora = DateTime.Now;

            var tiempo_restante = fechaActividad - ahora;

            var buscar = Builders<RegistroAc>.Filter.Eq(a => a.Id, actividad.Id);

            if (actividad.Notificacion)
            {
                Console.WriteLine($"{ahora}");
                Console.WriteLine($"{fechaActividad}");
                Console.WriteLine($"{tiempo_restante.TotalMinutes}");
                if (tiempo_restante.TotalMinutes <= 60 && tiempo_restante.TotalMinutes > 30 && actividad.P_Noti)
                {
                    await _emailService.EnviarEmail(
                        $"Recodatorio: {actividad.Titulo} empezará en menos de una hora",
                        $"<h2>{actividad.Titulo}</h2>" +
                        $"<p> Estimado: Juan Jose Tito</p> " +
                        $"<p>Este es un recordatorio de que tienes una actividad programada para las {actividad.Fecha.ToString("HH:mm")}.</p>" +
                        $"<h3>Detalles de la actividad</h3>" +
                        $"<p>Descripcion: {actividad.Descripcion}</p>" +
                        $"<p>Fecha de la actividad: {actividad.Fecha}</p>" +
                        $"<p>Asegúrese de estar preparado para esta actividad. " +
                        $"Si necesitas reprogramarla puedes hacer en la Aplicacion</p>" +
                        $"<p>Atentamente: Tu sistema</p>");
                    Console.WriteLine($"{actividad.Titulo}: Primera notificacion enviada");
                    var update = Builders<RegistroAc>.Update.Set(a => a.P_Noti, false);
                    await _actividades.UpdateOneAsync(buscar, update);
                }
                else if (tiempo_restante.TotalMinutes <= 30 && tiempo_restante.TotalMinutes > 0 && actividad.S_Noti)
                {
                    await _emailService.EnviarEmail(
                        $"Recodatorio: {actividad.Titulo} empezará en menos de media hora",
                        $"<h2>{actividad.Titulo}</h2>" +
                        $"<p> Estimado: Juan Jose Tito</p> " +
                        $"<p>Este es un recordatorio de que tienes una actividad programada para las {actividad.Fecha.ToString("HH:mm")}.</p>" +
                        $"<h3>Detalles de la actividad</h3>" +
                        $"<p>Descripcion: {actividad.Descripcion}</p>" +
                        $"<p>Fecha de la actividad: {actividad.Fecha}</p>" +
                        $"<p>Asegúrese de estar preparado para esta actividad. " +
                        $"Si necesitas reprogramarla puedes hacer en la Aplicacion</p>" +
                        $"<p>Atentamente: Tu sistema</p>");
                    Console.WriteLine($"{actividad.Titulo}: Segunda notificacion enviada");
                    var update = Builders<RegistroAc>.Update.Set(a => a.S_Noti, false);
                    await _actividades.UpdateOneAsync(buscar, update);
                }
                else if (tiempo_restante.TotalMinutes <= 10 && tiempo_restante.TotalMinutes > 0 && actividad.T_Noti)
                {
                    await _emailService.EnviarEmail(
                        $"Recodatorio: {actividad.Titulo} empezará en menos de diez minutos",
                        $"<h2>{actividad.Titulo}</h2>" +
                        $"<p> Estimado: Juan Jose Tito</p> " +
                        $"<p>Este es un recordatorio de que tienes una actividad programada para las {actividad.Fecha.ToString("HH:mm")}.</p>" +
                        $"<h3>Detalles de la actividad</h3>" +
                        $"<p>Descripcion: {actividad.Descripcion}</p>" +
                        $"<p>Fecha de la actividad: {actividad.Fecha}</p>" +
                        $"<p>Asegúrese de estar preparado para esta actividad. " +
                        $"Si necesitas reprogramarla puedes hacer en la Aplicacion</p>" +
                        $"<p>Atentamente: Tu sistema</p>");
                    Console.WriteLine($"{actividad.Titulo}: Tercera notificacion enviada");
                    var update = Builders<RegistroAc>.Update.Set(a => a.T_Noti, false);
                    await _actividades.UpdateOneAsync(buscar, update);
                }
                else if (tiempo_restante.TotalMinutes <= 0 && tiempo_restante.TotalMinutes >-60 && actividad.C_Noti)
                {

                    await _emailService.EnviarEmail(
                        $"Recodatorio: {actividad.Titulo} acaba de comenzar",
                        $"<h2>{actividad.Titulo}</h2>" +
                        $"<p> Estimado: Juan Jose Tito</p> " +
                        $"<p>Este es un recordatorio de tu actividad {actividad.Titulo} acaba de comenzar.</p>" +
                        $"<h3>Detalles de la actividad</h3>" +
                        $"<p>Descripcion: {actividad.Descripcion}</p>" +
                        $"<p>Fecha de la actividad: {actividad.Fecha}</p>" +
                        $"<p>Asegúrese de estar preparado para esta actividad. " +
                        $"Si necesitas reprogramarla puedes hacer en la Aplicacion</p>" +
                        $"<p>Atentamente: Tu sistema</p>");
                    Console.WriteLine($"Cuarta notificacion enviada notificacion enviada de la actividad {actividad.Titulo}");
                    var update = Builders<RegistroAc>.Update.Set(a => a.C_Noti, false);
                    await _actividades.UpdateOneAsync(buscar, update);
                }
                else if (tiempo_restante.TotalMinutes <= -60)
                {
                    await _actividades.DeleteOneAsync(buscar);
                    await _emailService.EnviarEmail(
                        $"¡Aviso!: {actividad.Titulo} se acaba de elimnar ",
                        $"<h2>{actividad.Titulo}</h2>" +
                        $"<p> Estimado: Juan Jose Tito</p> " +
                        $"<p>Este es un recordatorio de que tienes una actividad programada para las {actividad.Fecha.ToString("HH:mm")}.</p>" +
                        $"<h3>Detalles de la actividad</h3>" +
                        $"<p>Descripcion: {actividad.Descripcion}</p>" +
                        $"<p>Fecha de la actividad: {actividad.Fecha}</p>" +
                        $"<p>Asegúrese de estar preparado para esta actividad. " +
                        $"Si necesitas reprogramarla puedes hacer en la Aplicacion</p>" +
                        $"<p>Atentamente: Tu sistema</p>");
                    Console.WriteLine($"{actividad.Titulo} se acaba de eliminar y se envio la ultima notificacion");
                }
            }
        }
    }
}
