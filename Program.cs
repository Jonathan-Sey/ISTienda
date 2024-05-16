using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Tienda;
using Tienda.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//***********************************DEFINICON DE CONEXION **********************************
// configuracion de base de datos 
// dentro de <> Definimos el nombre de nestro archivo de contexto para generar los sets de nuestra tablas
builder.Services.AddDbContext<ApplicationDbContext>(
    // Cadena en la que espesificamos nuestra ruta de conexion en este caso DefaultConnection el cual
    // esta en nuestro archivo appsettings.json
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
//------------------------------------FIN DEFINICION DE CONEXION ------------------------



//****************** DEFINICION DE SERVICIOS DEL CONTENEDOR DE INYECCION DE INDEPENDENCIAS ***********

// definicion de politicas de integracion de servicios de autentificacion 
// mediante este servicio estamos configurando la autenticacion de usuarios mediante una politica
builder.Services.AddAuthorization(options=>
{
    //Definicion de politica mediante la definicon options el cual es un lambda 
    //El nombre de esta politica sera RequiredAdminOrStaff
    options.AddPolicy(
        // Nuestra politica RequireAdminOrStaff permite a usuarios Admin o staff
        // con eso podemos utilizar esta politica en controladores de roles
        "RequireAdminOrStaff", 
        policy => policy.RequireRole("Administrador", "Staff")
        );
});
//-------------- FIN CONTENEDOR DE INYECCIONES DE IDEPENDENCIAS -------------------------




// ******************** USO DE SERVICIOS cookies PARA LA AUTENTIFICACION ****************
// Definimos del setvicio de autenticacion al contenedor de cookies de dependencias.
builder.Services
    //Establecemos la configuracion predeterminada de cookies
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    // Definimos el metodo u accion que realizara la cookies mediante una exprecion lambda
    // con esto podremos espesificar que acciones tomara este servicio
    .AddCookie(options => 
    {
        // ---- definicion de acciones ----
        // acceso unicamente http, esto para mejorar la seguridad contra ataques de scripting
        options.Cookie.HttpOnly=true;
        // Definicion de tiempo de expiracion 
        options.ExpireTimeSpan=TimeSpan.FromMinutes(60);
        // Ruta el cual redirigira al usuario al verificar la informacion
        options.LoginPath="/Account/Login";
        // Ruta a la que se redirigirá al usuario si está autenticado pero que no tiene permiso para acceder a ciertos recurso
        options.AccessDeniedPath="/Account/AccessDenied";
    });
    // -------------------- FIN DEL SETVICIO DE COOKIES ----------------------------------


// ********************* CONFIGURACION DE SERVICIOS A UTILIZAR ***************************
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();


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

// Definicion de app.UseAuthentication();
// con esto el sistema realiza una solicitud de verificacion de sesion, Si el usuario no esta reguistrado, se redirige a una página de inicio de sesión o mostrara un mensaje de acceso denegado.
app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
