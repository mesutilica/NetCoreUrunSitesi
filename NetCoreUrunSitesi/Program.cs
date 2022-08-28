using BL;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Login sistemi k�t�phanesi
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
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection y�ntemiyle projemizde IRepository �rne�i istenirse Repository class�ndan instance al�n�p kullan�ma sunulur.
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // Admine giri� yapmayan kullan�c�lar� buraya y�nlendir
    x.AccessDeniedPath = "/AccesDenied";
    x.LogoutPath = "/Admin/Logout";
    x.Cookie.Name = "Admin";
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true;
});
// Authorization : Yetkilendirme : �nce servis olarak ekliyoruz
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

//Di�er Dependency Injection y�ntemleri :

// AddSingleton : Uygulama aya�a kalkarken �al��an ConfigureServices metodunda bu y�ntem ile tan�mlad���m�z her s�n�ftan sadece bir �rnek olu�turulur. Kim nereden �a��r�rsa �a��rs�n kendisine bu �rnek g�nderilir. Uygulama yeniden ba�layana kadar yenisi �retilmez.
// AddTransient : Uygulama �al��ma zaman�nda belirli ko�ullarda �retilir veya varolan �rne�i kullan�r. 
// AddScoped : Uygulama �al���rken her istek i�in ayr� ayr� nesne �retilir.

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

app.UseAuthentication(); // Admin login sistemi i�in
app.UseAuthorization();

app.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
      );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
