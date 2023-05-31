using Microsoft.EntityFrameworkCore;
using PocketTimetableBackend.Contexts;
using PocketTimetableBackend.Models.Db;

namespace PocketTimetableBackend.Services
{
    public class GroupService
    {
        private readonly UniversitiesContext context;

        public GroupService(UniversitiesContext context)
        {
            this.context = context;
        }

        public async Task<Group[]> GetGroupsByUniversityId(long id)
        {
            return await context.Groups
                .Where(group => group.UniversityId == id)
                .ToArrayAsync();
        }
    }
}
