using Core.Entities;
using Data;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies; // Login sistemi kütüphanesi
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
//builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // json dan çekmek için
//builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection yöntemiyle projemizde IRepository örneði istenirse Repository classýndan instance alýnýp kullanýma sunulur.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
//builder.Services.AddTransient(typeof(ICacheService<>), typeof(CacheService<>));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Account/Login"; // Admine giriþ yapmayan kullanýcýlarý buraya yönlendir
    x.AccessDeniedPath = "/AccesDenied";
    x.LogoutPath = "/Account/Logout";
    x.Cookie.Name = "Account";
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true;
});
// Authorization : Yetkilendirme : Önce servis olarak ekliyoruz
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

//Diðer Dependency Injection yöntemleri :

// AddSingleton : Uygulama ayaða kalkarken çalýþan ConfigureServices metodunda bu yöntem ile tanýmladýðýmýz her sýnýftan sadece bir örnek oluþturulur. Kim nereden çaðýrýrsa çaðýrsýn kendisine bu örnek gönderilir. Uygulama yeniden baþlayana kadar yenisi üretilmez.
// AddTransient : Her servis isteðinde yeni bir instance oluþturulur. 
// AddScoped : Gelen her bir web requesti için bir instance oluþturur ve gelen her ayný requestte ayný instance’ý kullanýr, farklý web requestler içinde yeni bir instance oluþturur..

builder.Services.AddMemoryCache(); // Keþlemeyi aktif etmek için
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// builder.Services.AddOutputCache(); // sayfa çýktýlarýný önbelleklemek için

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("custom", policy =>
    {
        policy.Expire(TimeSpan.FromMinutes(1)); // kendi kuralýmýzý uyguladýk
    });
    //options.AddBasePolicy(policy =>
    //{
    //    policy.Expire(TimeSpan.FromMinutes(1)); // varsayýlan output ayarlarýný özelleþtirdik
    //});
});

//AddAutoMapper 
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// app.UseOutputCache(); //uygulamada sayfa önbelleklemeyi kullan
// Bu adýmdan sonra önbellekleme yapacaðýmýz get actionlarýna [OutputCache] veya [OutputCache(PolicyName="custom")]

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseCutomExceptionMiddleware(); // eðer geliþtirme ortamýnda deðilsek özel global hata yakalamayý kullan
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication(); // Admin login sistemi için
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
