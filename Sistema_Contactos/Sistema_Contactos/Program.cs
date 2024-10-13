using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Proyecto_Bb_2.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Conexion de base de datos de los contactos
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
