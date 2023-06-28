using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using PWManagerServiceModelEF;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using JsonSubTypes;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace PWManagerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            Configuration = builder.Configuration;

            builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(Appsettings.Instance.Db_connectionstring));
            builder.Services.AddScoped<TokenService, TokenService>();
            builder.Services.AddControllers();


            Serilog.Core.Logger logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .Enrich.FromLogContext()

#if DEBUG
              .WriteTo.Console()
              .WriteTo.File("Log\\debug.log")
#endif
#if RELEASE
              .WriteTo.File("Log\\release.log")
#endif
              .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });



            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ApiWithAuthBackendString,
                        ValidAudience = ApiWithAuthBackendString,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            //ToDO: n�her mit befassen -> .env oder so auslagern
                            Encoding.UTF8.GetBytes("!SomethingSecret!")
                        ),
                    };
                });

            builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                
            })
                .AddEntityFrameworkStores<DataContext>();


            //
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);



            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "https://isefpwmanagerwebapp.azurewebsites.net/");
                    });
            });

            // zur Limitierung von Aufrufen innerhalb eines Zeitraumes
            // Es wird je Aufruf unabh�ngig vom Endpunkt gez�hlt
            // abh�ngig vom Aufrufenden (IP-Adresse)?
            builder.Services.AddRateLimiter(_ => _
                .AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    // Anzahl der Anfragen in Zeitraum
                    options.PermitLimit = Appsettings.Instance.oRateLimit.PermitLimit;
                    // Zeitraum
                    options.Window = TimeSpan.FromMinutes(Appsettings.Instance.oRateLimit.TimeWindowInMinutes);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = Appsettings.Instance.oRateLimit.QueueLimit;
                }));




            WebApplication app = builder.Build();

            // aktiviert die Limitierung
            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.Logger.LogInformation("Environment: Development");
            }
            if (app.Environment.IsProduction())
            {
                app.Logger.LogInformation("Environment: Production");
            }


            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }

        /// <summary>
        /// Builder/App Configuration
        /// </summary>
        public static IConfiguration Configuration { get; private set; }
        public const string ApiWithAuthBackendString = "apiWithAuthBackend";//ToDo: Appsettings auslagern

    }
}