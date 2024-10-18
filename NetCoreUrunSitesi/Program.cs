using Core.Entities;
using Data;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies; // Login sistemi k�t�phanesi
using NetCoreUrunSitesi.Middlewares;
using Service.Abstract;
using Service.Concrete;
using Service.ValidationRules;
using System.Reflection;
using System.Security.Claims;
//using WebAPIUsing.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IValidator<AppUser>, AppUserValidator>();

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient();
//builder.Services.AddDbContext<DatabaseContext>(option => option.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddDbContext<DatabaseContext>(); //options => options.UseSqlServer() uygulamada sql server kullan
//builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // json dan �ekmek i�in
//builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection y�ntemiyle projemizde IRepository �rne�i istenirse Repository class�ndan instance al�n�p kullan�ma sunulur.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
//builder.Services.AddTransient(typeof(ICacheService<>), typeof(CacheService<>));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Account/Login"; // Admine giri� yapmayan kullan�c�lar� buraya y�nlendir
    x.AccessDeniedPath = "/AccesDenied";
    x.LogoutPath = "/Account/Logout";
    x.Cookie.Name = "Account";
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true;
});
// Authorization : Yetkilendirme : �nce servis olarak ekliyoruz
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin")); // Bundan sonra Controller lara Policy i belirtmeliyiz..
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});
//BasicAuthentication
/*builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
});*/

//Di�er Dependency Injection y�ntemleri :

// AddSingleton : Uygulama aya�a kalkarken �al��an ConfigureServices metodunda bu y�ntem ile tan�mlad���m�z her s�n�ftan sadece bir �rnek olu�turulur. Kim nereden �a��r�rsa �a��rs�n kendisine bu �rnek g�nderilir. Uygulama yeniden ba�layana kadar yenisi �retilmez.
// AddTransient : Her servis iste�inde yeni bir instance olu�turulur. 
// AddScoped : Gelen her bir web requesti i�in bir instance olu�turur ve gelen her ayn� requestte ayn� instance�� kullan�r, farkl� web requestler i�inde yeni bir instance olu�turur..

builder.Services.AddMemoryCache(); // Ke�lemeyi aktif etmek i�in
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// builder.Services.AddOutputCache(); // sayfa ��kt�lar�n� �nbelleklemek i�in

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("custom", policy =>
    {
        policy.Expire(TimeSpan.FromMinutes(1)); // kendi kural�m�z� uygulad�k
    });
    //options.AddBasePolicy(policy =>
    //{
    //    policy.Expire(TimeSpan.FromMinutes(1)); // varsay�lan output ayarlar�n� �zelle�tirdik
    //});
});

//AddAutoMapper 
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// app.UseOutputCache(); //uygulamada sayfa �nbelleklemeyi kullan
// Bu ad�mdan sonra �nbellekleme yapaca��m�z get actionlar�na [OutputCache] veya [OutputCache(PolicyName="custom")]

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseCutomExceptionMiddleware(); // e�er geli�tirme ortam�nda de�ilsek �zel global hata yakalamay� kullan
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication(); // Admin login sistemi i�in
app.UseAuthorization();

app.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
      );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{customurl?}/{controller=Home}/{action=Index}/{id?}");

app.Run();
