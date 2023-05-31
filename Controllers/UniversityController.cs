using Microsoft.AspNetCore.Mvc;
using PocketTimetableBackend.Models.Db;
using PocketTimetableBackend.Models.Http;
using PocketTimetableBackend.Services;

namespace PocketTimetableBackend.Controllers
{
    [Route("pocket-timetable/api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly UniversityService service;

        public UniversityController(UniversityService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<JsonResult> GetUniversities()
        {
            var universities = await service.GetUniversities();

            return new JsonResult(
                new ControllerResponse<University>()
                {
                    Success = universities.Length != 0,
                    Data = universities
                }
            );
        }
    }
}
