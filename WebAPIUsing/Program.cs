using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service.Abstract;
using Service.Concrete;
using System.Security.Claims;
using WebAPIUsing.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpClient();
/*builder.Services.AddHttpClient("GitHub", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.github.com/");
    // The GitHub API requires two headers. The Use-Agent header is added
    // dynamically through UserAgentHeaderHandler
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Authorization, "Bearer ");
});*/
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Account/Login"; // Admine giriþ yapmayan kullanýcýlarý buraya yönlendir
    x.AccessDeniedPath = "/AccesDenied";
    x.LogoutPath = "/Account/Logout";
    x.Cookie.Name = "Account";
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true;
});
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin")); // Bundan sonra Controller lara Policy i belirtmeliyiz..
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin","User"));
});

//builder.Services.AddHttpClient<AppUsersApiService>(opt =>
//{
//    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
//});
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

app.UseAuthentication();
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Authorization", "Bearer " + context.Session.GetString("token"));
//    //context.Response.Headers["Authorization"] = "Bearer " + context.Session.GetString("token");
//    await next();
//});

// custom ExceptionHandlingMiddleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
