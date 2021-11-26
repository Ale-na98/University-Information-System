using System.Net;
using BusinessLogic;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace module_10
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddXmlSerializerFormatters()
                .AddFluentValidation();

            services
                .AddTransient<IValidator<StudentDto>, StudentValidator>()
                .AddTransient<IValidator<TeacherDto>, TeacherValidator>()
                .AddTransient<IValidator<LectureDto>, LectureValidator>()
                .AddTransient<IValidator<HometaskDto>, HometaskValidator>()
                .AddTransient<IValidator<AttendanceDto>, AttendanceValidator>();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" )
            {
                services
                .AddBusinessLogic(GetConnectionStringFromEnvironment());
            }
            else
            {
                services
                .AddBusinessLogic(Configuration.GetConnectionString("UniversityDb"));
            }

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RestApi", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApi v1"));
            }
            app.UseRouting();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/html";

                    await context.Response.WriteAsync("{\r\n");
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;
                    if (exception is BusinessException)
                    {
                        await context.Response.WriteAsync($"  \"error\": \"{exception.Message}\"\r\n");
                    }
                    else
                    {
                        await context.Response.WriteAsync($"  \"error\": \"Unknown exception occurred\"\r\n");
                    }
                    await context.Response.WriteAsync("}\r\n");
                    logger.LogError(exception, exception.Message + exception.StackTrace);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static string GetConnectionStringFromEnvironment()
        {
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var databaseUri = new Uri(connectionUrl);
            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }
    }
}
