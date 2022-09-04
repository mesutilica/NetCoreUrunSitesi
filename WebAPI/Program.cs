using BL;
using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer()); // uygulamada sql server kullan
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                //x.LoginPath = "/Admin/Login"; // Admine giriþ yapmayan kullanýcýlarý buraya yönlendir
                x.Cookie.Name = "ApiAdmin";
            });
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
                    ClockSkew = TimeSpan.Zero // zaman farký olmasýn
                };
            });
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}