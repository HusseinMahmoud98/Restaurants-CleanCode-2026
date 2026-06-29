using Microsoft.OpenApi.Models;
using Restaurants.API.Middlewares;
using Serilog;

namespace Restaurants.API.Extensions
{
    public static class WebApplicationsBuilderExensions
    {
       public static void AddPresenaton(this WebApplicationBuilder builder)
       {
            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // Configure Swagger to require JWT Bearer authentication.
            // "bearerAuth" is just the name of the scheme definition (can be any label),
            // while Scheme = "Bearer" is what actually tells Swagger to send tokens in the
            // Authorization header as "Bearer <token>". The empty [] means no scopes are required.
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                               Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth" //previuosly defined
                            }
                        },[]
                    }
                });
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

            //builder.Host.UseSerilog((context, configuration) =>
            //{
            //    configuration
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            //    .WriteTo.File("Logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            //    .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] | {SourceContent} | {NewLine}{Message:lj}{NewLine}{Exception}");
            //});
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddAuthentication();

        }
    }
}
