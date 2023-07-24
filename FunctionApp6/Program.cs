using FunctionApp6.Services;
using FunctionApp6.Data;
using FunctionApp6.Models;
using FunctionApp6;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((hostContext, services) =>
                {
                    string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
                    services.AddCors(options =>
                    {
                        options.AddPolicy(name: MyAllowSpecificOrigins,
                          builder =>
                          {
                              builder.WithOrigins(
                                "http://example.com", "*");
                          });
                    });

                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
                    });
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer("Server=tcp:server624.database.windows.net,1433;Initial Catalog=Restraunt;Persist Security Info=False;User ID=animesh624;Password=Summers@2023!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
                    services.AddScoped<IAPIServices, APIServices>();
                    services.AddDefaultIdentity<IdentityUser>();
               
                    services.AddIdentityCore<IdentityUser>(options => {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                    })
                     .AddEntityFrameworkStores<ApplicationDbContext>();



                })
                .Build();
            host.Run();
        }
    }
}