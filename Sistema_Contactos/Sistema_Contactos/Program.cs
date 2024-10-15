using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Proyecto_Bb_2.Data;
using Proyecto_Bb_2.Servicios;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Confu de base de datos de los contactos
builder.Services.Configure<Db_contacto>(builder.Configuration.GetSection("Db_config"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var ConfigContacto = sp.GetRequiredService<IOptions<Db_contacto>>().Value;
    return new MongoClient(ConfigContacto.Db_connection);
});

//Conexion con la base de datos para actividades
builder.Services.Configure<Db_actividades>(builder.Configuration.GetSection("Db_config"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var ConfigActividad = sp.GetRequiredService<IOptions<Db_actividades>>().Value;
    return new MongoClient(ConfigActividad.Db_connection);
});

//conexion con email
// Cargar configuración de EmailConfig
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.AddTransient<EmailService>();
builder.Services.AddHostedService<Notificaciones>();
Console.WriteLine("Configuracion con la base de datos y servicio de Email establecido");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registro}/{action=Index}/{id?}");

app.Run();
