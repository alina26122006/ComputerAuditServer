
using ComputerAuditServer.Data;
using ComputerAuditServer.Services;
using Microsoft.EntityFrameworkCore;
using ComputerAuditServer.Data;
using ComputerAuditServer.Services;

namespace ComputerAuditServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // „D„€„q„p„r„|„‘„u„} „{„€„~„„„u„{„ƒ„„ „q„p„x„ „t„p„~„~„„‡ PostgreSQL
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // „Q„u„s„y„ƒ„„„‚„y„‚„…„u„} „ƒ„u„‚„r„y„ƒ„
            builder.Services.AddScoped<ComparisonService>();

            // „D„€„q„p„r„|„‘„u„} CORS „t„|„‘ „‚„p„x„‚„u„Š„u„~„y„‘ „x„p„„‚„€„ƒ„€„r „ƒ „{„|„y„u„~„„„ƒ„{„y„‡ „P„K
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            // „@„r„„„€„}„p„„„y„‰„u„ƒ„{„€„u „ƒ„€„x„t„p„~„y„u „q„p„x„ „t„p„~„~„„‡ „„‚„y „x„p„„…„ƒ„{„u
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            app.Run();
        }
    }
}
