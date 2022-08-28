using BL;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Login sistemi kütüphanesi
using NetCoreUrunSitesi.Services;
using BL.Concrete;
using BL.Abstract;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  FluentValidation
builder.Services.AddFluentValidation(conf =>
{
    conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    conf.AutomaticValidationEnabled = false;
});

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<AppUsersApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer()); // uygulamada sql server kullan
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection yöntemiyle projemizde IRepository örneði istenirse Repository classýndan instance alýnýp kullanýma sunulur.
builder.Services.AddTransient<IProductRepository, ProductRepository>();
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin")); // Bundan sonra Controller lara Policy i belirtmeliyiz..
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim("Role", "User"));
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

app.Run();
