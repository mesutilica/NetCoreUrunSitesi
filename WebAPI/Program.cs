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
            // a�a��daki kod include yapt���m�zda ��kan json sorununu ��z�yor
            builder.Services.AddControllersWithViews()
                            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DatabaseContext>(); //options => options.UseSqlServer() uygulamada sql server kullan
            builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));// Dependency Injection y�ntemiyle projemizde IRepository �rne�i istenirse Repository class�ndan instance al�n�p kullan�ma sunulur.
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            //{
            //    //x.LoginPath = "/Admin/Login"; // Admine giri� yapmayan kullan�c�lar� buraya y�nlendir
            //    x.Cookie.Name = "ApiAdmin";
            //});
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Validasyon yapmak istedi�imiz alanlar
                    ValidateAudience = true, // Kitleyi Do�rula
                    ValidateIssuer = true, // Token� vereni do�rula
                    ValidateLifetime = true, // Token ya�am s�resini do�rula
                    ValidateIssuerSigningKey = true, // Token� verenin �mzalama anahtar�n� Do�rula
                    ValidIssuer = builder.Configuration["Token:Issuer"], // Token� veren sa�lay�c�
                    ValidAudience = builder.Configuration["Token:Audience"], // Token� kullanacak kullan�c�
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])), // Token� �mzalama Anahtar�
                    ClockSkew = TimeSpan.Zero // saat fark� olmas�n
                };
            });
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddMemoryCache(); // Ke�lemeyi aktif etmek i�in

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