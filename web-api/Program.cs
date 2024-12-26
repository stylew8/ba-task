
using Microsoft.EntityFrameworkCore;
using web_api.DAL;
using web_api.Models;
using web_api.Services;

namespace web_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(
                    builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty
                ));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IElectricityService, ElectricityService>();
            builder.Services.AddScoped<ICsvDataService, CsvDataService>();
            builder.Services.AddScoped<IElectricityDataRepository, ElectricityDataRepository>();

            builder.Services.AddControllers();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
