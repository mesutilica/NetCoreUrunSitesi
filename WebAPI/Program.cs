using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Service.Abstract;
using Service.Concrete;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI.Middlewares;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // aþaðýdaki kod include yaptýðýmýzda çýkan json sorununu çözüyor
            builder.Services.AddControllersWithViews()
                            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DatabaseContext>(); //options => options.UseSqlServer() uygulamada sql server kullan
            builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));// Dependency Injection yöntemiyle projemizde IRepository örneði istenirse Repository classýndan instance alýnýp kullanýma sunulur.
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            //{
            //    //x.LoginPath = "/Admin/Login"; // Admine giriþ yapmayan kullanýcýlarý buraya yönlendir
            //    x.Cookie.Name = "ApiAdmin";
            //});
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Validasyon yapmak istediðimiz alanlar
                    ValidateAudience = true, // Kitleyi Doðrula
                    ValidateIssuer = true, // Tokený vereni doðrula
                    ValidateLifetime = true, // Token yaþam süresini doðrula
                    ValidateIssuerSigningKey = true, // Tokený verenin Ýmzalama anahtarýný Doðrula
                    ValidIssuer = builder.Configuration["Token:Issuer"], // Tokený veren saðlayýcý
                    ValidAudience = builder.Configuration["Token:Audience"], // Tokený kullanacak kullanýcý
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])), // Tokený Ýmzalama Anahtarý
                    ClockSkew = TimeSpan.Zero // saat farký olmasýn
                };
            });
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddMemoryCache(); // Keþlemeyi aktif etmek için

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseCutomExceptionMiddleware();

            app.Run();
        }
    }
}