using Consultorio.Aplicacion.Servicios;
using Consultorio.Dominio.Repositorios;
using Consultorio.infraestructura.MongoDB.Context;
using Consultorio.infraestructura.MongoDB.Services;
using Consultorio.infraestructura.SqlServer.Contextos;
using Consultorio.infraestructura.MongoDB.Repositorios;
//using Consultorio.infraestructura.SqlServer.Repositorios;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;


namespace Consultorio.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            //Infraestructura MongoDB
            builder.Services.Configure<MongoDBDatabaseSettings>(
                builder.Configuration.GetSection("MongoDBDatabase"));

            builder.Services.AddSingleton<ClienteRepository>();



            //Seguridad
            builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


                }
            ).AddJwtBearer(opt =>
            opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "http://localhost:7256",
                    ValidAudience = "http://localhost:7256",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("passwordSuperSecreto"))
                }
            );

            //Implementacion de Serilog
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .WriteTo.Console()
            //    .WriteTo.SQLite(@"C:\Logs\Apiconsultorio.db")
            //    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            //    .CreateLogger();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            //Configurar conexion a base de datos, para el intyector de dependencias, definismoa como crear lo que se necesita en parametros constructores




            //Agregamos Healthchecks

            builder.Services.AddHealthChecks()
                .AddCheck("App Running", () => HealthCheckResult.Healthy("Api is working as expected")
                )
                .AddSqlServer(
                    connectionString: "Server = DESKTOP-RQBKGG3; Database = Consultorio; Trusted_connection = yes", //Duda
                    healthQuery: "select 1",
                    name: "SQL Server Status",
                    failureStatus: HealthStatus.Unhealthy
                );

            builder.Services.AddHealthChecksUI().AddInMemoryStorage();



            var conectionString = builder.Configuration.GetConnectionString("SQLServer");
            builder.Services.AddDbContext<Context>(opt => opt.UseSqlServer(conectionString));

            builder.Services.AddScoped<ClienteService>();
            builder.Services.AddScoped<ConsultasServices>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<DoctorService>();


            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IConsultasRepository, ConsultaRepository>();
            //builder.Services.AddScoped < IConsultasRepository, ConsultasRepository, IClienteRepository ClienteRepository, IRepositoryDoctor DoctorRepository > ();
            builder.Services.AddScoped<IRepositoryDoctor, DoctorRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();




            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "WebAPI",
                    Description = "Product WebAPI"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                     new OpenApiSecurityScheme
                     {
                     Reference = new OpenApiReference
                     {
                     Id = "Bearer",
                     Type = ReferenceType.SecurityScheme
                     }
                     },
                     new List<string>()
                     }
                     });
                });

            var app = builder.Build();



            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())

            {

                app.UseSwagger();

                //app.UseSwaggerUI();

                app.UseSwaggerUI(options => {

                    options.SwaggerEndpoint("/swagger/V1/swagger.json", "Product WebAPI");

                });

            }







            //var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //mapeo de endpoint en el archivo
            

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.MapControllers();

            app.MapHealthChecks("/health-check");

            app.MapHealthChecks("/health-details", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecksUI();
            //app.UseRouting().UseEndpoints(config => config.MapHealthChecksUI());


            app.Run();
        }
    }
}