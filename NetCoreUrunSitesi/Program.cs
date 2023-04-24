using Core.Entities;
using DAL;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies; // Login sistemi kütüphanesi
using Service.Abstract;
using Service.Concrete;
using Service.ValidationRules;
//using WebAPIUsing.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IValidator<AppUser>, AppUserValidator>();

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DatabaseContext>(); //options => options.UseSqlServer() uygulamada sql server kullan
//builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // json dan çekmek için
//builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection yöntemiyle projemizde IRepository örneði istenirse Repository classýndan instance alýnýp kullanýma sunulur.
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
//builder.Services.AddTransient(typeof(ICacheService<>), typeof(CacheService<>));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // Admine giriþ yapmayan kullanýcýlarý buraya yönlendir
    x.AccessDeniedPath = "/AccesDenied";
    x.LogoutPath = "/Admin/Logout";
    x.Cookie.Name = "Admin";
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true;
});
// Authorization : Yetkilendirme : Önce servis olarak ekliyoruz
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin")); // Bundan sonra Controller lara Policy i belirtmeliyiz..
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim("Role", "User"));
});
//BasicAuthentication
/*builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
});*/

//Diðer Dependency Injection yöntemleri :

// AddSingleton : Uygulama ayaða kalkarken çalýþan ConfigureServices metodunda bu yöntem ile tanýmladýðýmýz her sýnýftan sadece bir örnek oluþturulur. Kim nereden çaðýrýrsa çaðýrsýn kendisine bu örnek gönderilir. Uygulama yeniden baþlayana kadar yenisi üretilmez.
// AddTransient : Uygulama çalýþma zamanýnda belirli koþullarda üretilir veya varolan örneði kullanýr. 
// AddScoped : Uygulama çalýþýrken her istek için ayrý ayrý nesne üretilir.

builder.Services.AddMemoryCache(); // Keþlemeyi aktif etmek için
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// builder.Services.AddOutputCache(); // sayfa çýktýlarýný önbelleklemek için

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("custom", policy =>
    {
        policy.Expire(TimeSpan.FromMinutes(1)); // kendi kuralýmýzý uyguladýk
    });
    options.AddBasePolicy(policy =>
    {
        policy.Expire(TimeSpan.FromMinutes(1)); // varsayýlan output ayarlarýný özelleþtirdik
    });
});

var app = builder.Build();

app.UseOutputCache(); //uygulamada sayfa önbelleklemeyi kullan
// Bu adýmdan sonra önbellekleme yapacaðýmýz get actionlarýna [OutputCache] veya [OutputCache(PolicyName="custom")]

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
