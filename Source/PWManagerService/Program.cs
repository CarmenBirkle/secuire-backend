using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;


namespace PWManagerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            Logger logger = new LoggerConfiguration()
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

            Configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // zur Limitierung von Aufrufen innerhalb eines Zeitraumes
            // Es wird je Aufruf unabhängig vom Endpunkt gezählt
            // abhängig vom Aufrufenden (IP-Adresse)?
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Builder/App Configuration
        /// </summary>
        public static IConfiguration Configuration { get; private set; }
    }
}