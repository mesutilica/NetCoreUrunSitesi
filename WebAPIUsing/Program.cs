using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service.Abstract;
using Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
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

builder.Services.AddMemoryCache(); // Keþlemeyi aktif etmek için
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
        pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
      );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{customurl?}/{controller=Home}/{action=Index}/{id?}");

app.Run();
