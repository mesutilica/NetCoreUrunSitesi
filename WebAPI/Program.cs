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
                //x.LoginPath = "/Admin/Login"; // Admine giri� yapmayan kullan�c�lar� buraya y�nlendir
                x.Cookie.Name = "ApiAdmin";
            });
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
                    ClockSkew = TimeSpan.Zero // zaman fark� olmas�n
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