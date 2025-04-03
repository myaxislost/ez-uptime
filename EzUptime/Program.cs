
using EzUptime.Services;
using EzUptime.Services.Monitoring;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

namespace EzUptime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<MonitoringService>();
            builder.Services.AddSingleton<ConfigService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", (HttpContext context) =>
            {
                context.Response.Redirect("/uptime");
            });

            var staticPath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("wwwroot", "assets"));
            if (!Directory.Exists(staticPath))
                Directory.CreateDirectory(staticPath);

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(staticPath),
                RequestPath = "/assets"
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "/uptime";
            });

            app.MapControllers();

            // restore
            app.Services.GetService<ConfigService>();

            app.Run();
        }
    }
}
