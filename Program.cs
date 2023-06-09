using PocketTimetableBackend.Contexts;
using PocketTimetableBackend.Services;

namespace PocketTimetableBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UniversitiesContext>();
            builder.Services.AddTransient<Parser>();
            builder.Services.AddTransient<UniversityService>();
            builder.Services.AddTransient<GroupService>();

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