using Microsoft.EntityFrameworkCore;
using PocketTimetableBackend.Contexts;
using PocketTimetableBackend.Models.Db;

namespace PocketTimetableBackend.Services
{
    public class UniversityService
    {
        private readonly UniversitiesContext context;

        public UniversityService(UniversitiesContext context)
        {
            this.context = context;
        }

        public async Task<University[]> GetUniversities()
        {
            return await context.Universities
                .ToArrayAsync();
        }
    }
}
